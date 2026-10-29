
using HomeVPN.Domain.Entities;
using HomeVPN.Domain.Enums;
using Xunit;

namespace HomeVPN.UnitTests.DomainTests;

public class VpnServerTests
{
    private static string Key(byte seed) =>
        Convert.ToBase64String(
            Enumerable.Repeat(seed, 32).ToArray());

    private static VpnServer CreateServer() =>
        new("Home VPN", "10.8.0.0/24", "10.8.0.1");

    [Fact]
    public void Constructor_WithValidData_CreatesServer()
    {
        var server = CreateServer();

        Assert.NotEqual(Guid.Empty, server.Id);
        Assert.Equal("Home VPN", server.Name);
        Assert.Equal("10.8.0.0/24", server.NetworkCidr);
        Assert.Equal("10.8.0.1", server.ServerIpAddress);
        Assert.Empty(server.Clients);
    }

    [Fact]
    public void Constructor_WithServerIpOutsideNetwork_Throws()
    {
        Assert.Throws<ArgumentException>(() =>
            new VpnServer(
                "Home VPN",
                "10.8.0.0/24",
                "192.168.1.1"));
    }

    [Fact]
    public void AddClient_WithValidClient_AddsClient()
    {
        var server = CreateServer();
        var client = new VpnClient(
            "Laptop", Key(1), "10.8.0.2");

        server.AddClient(client);

        Assert.Single(server.Clients);
        Assert.Contains(client, server.Clients);
        Assert.Equal(server.Id, client.ServerId);
    }

    [Fact]
    public void AddClient_WithDuplicateClient_Throws()
    {
        var server = CreateServer();
        var client = new VpnClient(
            "Laptop", Key(1), "10.8.0.2");

        server.AddClient(client);

        Assert.Throws<InvalidOperationException>(
            () => server.AddClient(client));
    }

    [Fact]
    public void AddClient_WithDuplicateIp_Throws()
    {
        var server = CreateServer();

        server.AddClient(new VpnClient(
            "Laptop", Key(1), "10.8.0.2"));

        var second = new VpnClient(
            "Phone", Key(2), "10.8.0.2");

        Assert.Throws<InvalidOperationException>(
            () => server.AddClient(second));

        Assert.Single(server.Clients);
    }

    [Fact]
    public void AddClient_WithDuplicatePublicKey_Throws()
    {
        var server = CreateServer();

        server.AddClient(new VpnClient(
            "Laptop", Key(1), "10.8.0.2"));

        var second = new VpnClient(
            "Phone", Key(1), "10.8.0.3");

        Assert.Throws<InvalidOperationException>(
            () => server.AddClient(second));
    }

    [Fact]
    public void AddClient_AssignedToAnotherServer_Throws()
    {
        var firstServer = CreateServer();
        var secondServer = new VpnServer(
            "Server 2",
            "10.8.0.0/24",
            "10.8.0.1");

        var client = new VpnClient(
            "Laptop", Key(1), "10.8.0.2");

        firstServer.AddClient(client);

        Assert.Throws<InvalidOperationException>(
            () => secondServer.AddClient(client));

        Assert.Empty(secondServer.Clients);
    }

    [Fact]
    public void AddClient_WithServerIp_Throws()
    {
        var server = CreateServer();
        var client = new VpnClient(
            "Laptop", Key(1), "10.8.0.1");

        Assert.Throws<InvalidOperationException>(
            () => server.AddClient(client));
    }

    [Fact]
    public void RemoveClient_WithExistingClient_RemovesClient()
    {
        var server = CreateServer();
        var client = new VpnClient(
            "Laptop", Key(1), "10.8.0.2");

        server.AddClient(client);
        server.RemoveClient(client.Id);

        Assert.Empty(server.Clients);
        Assert.Null(client.ServerId);
    }

    [Fact]
    public void RemoveClient_WithUnknownId_Throws()
    {
        var server = CreateServer();

        Assert.Throws<KeyNotFoundException>(
            () => server.RemoveClient(Guid.NewGuid()));
    }

    [Fact]
    public void DisableClient_ChangesStatus()
    {
        var client = new VpnClient(
            "Laptop", Key(1), "10.8.0.2");

        client.Disable();

        Assert.Equal(
            VpnClientStatus.Disabled,
            client.Status);
    }

    [Fact]
    public void ActivateClient_ChangesStatus()
    {
        var client = new VpnClient(
            "Laptop", Key(1), "10.8.0.2");

        client.Disable();
        client.Activate();

        Assert.Equal(
            VpnClientStatus.Active,
            client.Status);
    }
}
