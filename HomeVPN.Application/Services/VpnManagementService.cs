
using HomeVPN.Application.DTOs;
using HomeVPN.Application.Interfaces;
using HomeVPN.Domain.Entities;

namespace HomeVPN.Application.Services;

public sealed class VpnManagementService
    : IVpnManagementService
{
    private readonly IVpnServerRepository _repository;

    public VpnManagementService(
        IVpnServerRepository repository)
    {
        _repository = repository ??
            throw new ArgumentNullException(nameof(repository));
    }

    public async Task<VpnServerDto> CreateServerAsync(
        CreateVpnServerRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var server = new VpnServer(
            request.Name,
            request.NetworkCidr,
            request.ServerIpAddress);

        await _repository.AddAsync(
            server, cancellationToken);

        await _repository.SaveChangesAsync(
            cancellationToken);

        return MapServer(server);
    }

    public async Task<IReadOnlyList<VpnServerDto>> GetServersAsync(
        CancellationToken cancellationToken = default)
    {
        var servers = await _repository.GetAllAsync(
            cancellationToken);

        return servers.Select(MapServer).ToList();
    }

    public async Task<VpnServerDto> GetServerAsync(
        Guid serverId,
        CancellationToken cancellationToken = default)
    {
        var server = await GetRequiredServerAsync(
            serverId, cancellationToken);

        return MapServer(server);
    }

    public async Task<VpnClientDto> AddClientAsync(
        Guid serverId,
        CreateVpnClientRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var server = await GetRequiredServerAsync(
            serverId, cancellationToken);

        var client = new VpnClient(
            request.Name,
            request.PublicKey,
            request.VpnIpAddress);

        server.AddClient(client);

        await _repository.SaveChangesAsync(
            cancellationToken);

        return MapClient(client);
    }

    public async Task DisableClientAsync(
        Guid serverId,
        Guid clientId,
        CancellationToken cancellationToken = default)
    {
        var server = await GetRequiredServerAsync(
            serverId, cancellationToken);

        var client = GetRequiredClient(server, clientId);

        client.Disable();

        await _repository.SaveChangesAsync(
            cancellationToken);
    }

    public async Task ActivateClientAsync(
        Guid serverId,
        Guid clientId,
        CancellationToken cancellationToken = default)
    {
        var server = await GetRequiredServerAsync(
            serverId, cancellationToken);

        var client = GetRequiredClient(server, clientId);

        client.Activate();

        await _repository.SaveChangesAsync(
            cancellationToken);
    }

    public async Task RemoveClientAsync(
        Guid serverId,
        Guid clientId,
        CancellationToken cancellationToken = default)
    {
        var server = await GetRequiredServerAsync(
            serverId, cancellationToken);

        server.RemoveClient(clientId);

        await _repository.SaveChangesAsync(
            cancellationToken);
    }

    private async Task<VpnServer> GetRequiredServerAsync(
        Guid serverId,
        CancellationToken cancellationToken)
    {
        return await _repository.GetByIdAsync(
            serverId, cancellationToken)
            ?? throw new KeyNotFoundException(
                $"VPN server '{serverId}' was not found.");
    }

    private static VpnClient GetRequiredClient(
        VpnServer server,
        Guid clientId)
    {
        return server.Clients.FirstOrDefault(
            c => c.Id == clientId)
            ?? throw new KeyNotFoundException(
                $"VPN client '{clientId}' was not found.");
    }

    private static VpnClientDto MapClient(VpnClient client)
    {
        return new VpnClientDto(
            client.Id,
            client.Name,
            client.PublicKey,
            client.VpnIpAddress,
            client.Status,
            client.ServerId,
            client.CreatedAtUtc);
    }

    private static VpnServerDto MapServer(VpnServer server)
    {
        return new VpnServerDto(
            server.Id,
            server.Name,
            server.NetworkCidr,
            server.ServerIpAddress,
            server.Clients.Select(MapClient).ToList());
    }
}
