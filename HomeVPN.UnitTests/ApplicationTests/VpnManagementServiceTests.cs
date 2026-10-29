
using HomeVPN.Application.DTOs;
using HomeVPN.Application.Interfaces;
using HomeVPN.Application.Services;
using HomeVPN.Domain.Entities;
using HomeVPN.Domain.Enums;
using Xunit;

namespace HomeVPN.UnitTests.ApplicationTests;

public class VpnManagementServiceTests
{
    private static string Key(byte seed) =>
        Convert.ToBase64String(
            Enumerable.Repeat(seed, 32).ToArray());

    [Fact]
    public async Task CreateServerAsync_CreatesServer()
    {
        var repository = new FakeVpnServerRepository();
        var service = new VpnManagementService(repository);

        var result = await service.CreateServerAsync(
            new CreateVpnServerRequest(
                "Home VPN",
                "10.8.0.0/24",
                "10.8.0.1"));

        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal("Home VPN", result.Name);
        Assert.Equal("10.8.0.1", result.ServerIpAddress);
        Assert.Equal(1, repository.SaveCount);
    }

    [Fact]
    public async Task GetServersAsync_ReturnsServers()
    {
        var repository = new FakeVpnServerRepository();
        var service = new VpnManagementService(repository);

        await service.CreateServerAsync(
            new CreateVpnServerRequest(
                "Home VPN",
                "10.8.0.0/24",
                "10.8.0.1"));

        var result = await service.GetServersAsync();

        Assert.Single(result);
    }

    [Fact]
    public async Task GetServerAsync_UnknownId_Throws()
    {
        var service = new VpnManagementService(
            new FakeVpnServerRepository());

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => service.GetServerAsync(Guid.NewGuid()));
    }

    [Fact]
    public async Task AddClientAsync_AddsClient()
    {
        var repository = new FakeVpnServerRepository();
        var service = new VpnManagementService(repository);

        var server = await CreateServerAsync(service);

        var client = await service.AddClientAsync(
            server.Id,
            new CreateVpnClientRequest(
                "Laptop", Key(1), "10.8.0.2"));

        Assert.Equal("Laptop", client.Name);
        Assert.Equal(server.Id, client.ServerId);

        var updated = await service.GetServerAsync(server.Id);

        Assert.Single(updated.Clients);
        Assert.Equal(2, repository.SaveCount);
    }

    [Fact]
    public async Task AddClientAsync_DuplicateIp_Throws()
    {
        var service = new VpnManagementService(
            new FakeVpnServerRepository());

        var server = await CreateServerAsync(service);

        await service.AddClientAsync(
            server.Id,
            new CreateVpnClientRequest(
                "Laptop", Key(1), "10.8.0.2"));

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => service.AddClientAsync(
                server.Id,
                new CreateVpnClientRequest(
                    "Phone", Key(2), "10.8.0.2")));
    }

    [Fact]
    public async Task DisableClientAsync_DisablesClient()
    {
        var service = new VpnManagementService(
            new FakeVpnServerRepository());

        var server = await CreateServerAsync(service);

        var client = await service.AddClientAsync(
            server.Id,
            new CreateVpnClientRequest(
                "Laptop", Key(1), "10.8.0.2"));

        await service.DisableClientAsync(
            server.Id, client.Id);

        var updated = await service.GetServerAsync(server.Id);

        Assert.Equal(
            VpnClientStatus.Disabled,
            Assert.Single(updated.Clients).Status);
    }

    [Fact]
    public async Task ActivateClientAsync_ActivatesClient()
    {
        var service = new VpnManagementService(
            new FakeVpnServerRepository());

        var server = await CreateServerAsync(service);

        var client = await service.AddClientAsync(
            server.Id,
            new CreateVpnClientRequest(
                "Laptop", Key(1), "10.8.0.2"));

        await service.DisableClientAsync(
            server.Id, client.Id);

        await service.ActivateClientAsync(
            server.Id, client.Id);

        var updated = await service.GetServerAsync(server.Id);

        Assert.Equal(
            VpnClientStatus.Active,
            Assert.Single(updated.Clients).Status);
    }

    [Fact]
    public async Task RemoveClientAsync_RemovesClient()
    {
        var service = new VpnManagementService(
            new FakeVpnServerRepository());

        var server = await CreateServerAsync(service);

        var client = await service.AddClientAsync(
            server.Id,
            new CreateVpnClientRequest(
                "Laptop", Key(1), "10.8.0.2"));

        await service.RemoveClientAsync(
            server.Id, client.Id);

        var updated = await service.GetServerAsync(server.Id);

        Assert.Empty(updated.Clients);
    }

    [Fact]
    public async Task RemoveClientAsync_UnknownClient_Throws()
    {
        var service = new VpnManagementService(
            new FakeVpnServerRepository());

        var server = await CreateServerAsync(service);

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => service.RemoveClientAsync(
                server.Id, Guid.NewGuid()));
    }

    private static Task<VpnServerDto> CreateServerAsync(
        VpnManagementService service)
    {
        return service.CreateServerAsync(
            new CreateVpnServerRequest(
                "Home VPN",
                "10.8.0.0/24",
                "10.8.0.1"));
    }

    private sealed class FakeVpnServerRepository
        : IVpnServerRepository
    {
        private readonly Dictionary<Guid, VpnServer> _servers =
            new();

        public int SaveCount { get; private set; }

        public Task<VpnServer?> GetByIdAsync(
            Guid serverId,
            CancellationToken cancellationToken = default)
        {
            _servers.TryGetValue(serverId, out var server);

            return Task.FromResult(server);
        }

        public Task<IReadOnlyList<VpnServer>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            IReadOnlyList<VpnServer> result =
                _servers.Values.ToList();

            return Task.FromResult(result);
        }

        public Task AddAsync(
            VpnServer server,
            CancellationToken cancellationToken = default)
        {
            _servers.Add(server.Id, server);

            return Task.CompletedTask;
        }

        public Task SaveChangesAsync(
            CancellationToken cancellationToken = default)
        {
            SaveCount++;

            return Task.CompletedTask;
        }
    }
}
