
using HomeVPN.Domain.ValueObjects;

namespace HomeVPN.Domain.Entities;

public sealed class VpnServer
{
    private readonly List<VpnClient> _clients = new();

    public Guid Id { get; private set; }

    public string Name { get; private set; }

    public string NetworkCidr { get; private set; }

    public string ServerIpAddress { get; private set; }

    public IReadOnlyCollection<VpnClient> Clients =>
        _clients.AsReadOnly();

    public VpnServer(
        string name,
        string networkCidr,
        string serverIpAddress)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(
                "Server name is required.",
                nameof(name));
        }

        var network = new VpnNetwork(networkCidr);
        var serverIp = new VpnIpAddress(serverIpAddress);

        if (!network.IsUsableHost(serverIp))
        {
            throw new ArgumentException(
                "Server IP must be a usable address " +
                "within the VPN network.",
                nameof(serverIpAddress));
        }

        Id = Guid.NewGuid();
        Name = name.Trim();
        NetworkCidr = network.ToString();
        ServerIpAddress = serverIp.ToString();
    }

    public void AddClient(VpnClient client)
    {
        ArgumentNullException.ThrowIfNull(client);

        var network = new VpnNetwork(NetworkCidr);
        var address = new VpnIpAddress(client.VpnIpAddress);

        if (!network.IsUsableHost(address))
        {
            throw new InvalidOperationException(
                "Client IP is not a usable address " +
                "within the VPN network.");
        }

        if (client.VpnIpAddress == ServerIpAddress)
        {
            throw new InvalidOperationException(
                "Client IP is reserved for the VPN server.");
        }

        if (_clients.Any(c => c.Id == client.Id))
        {
            throw new InvalidOperationException(
                "Client already exists on this server.");
        }

        if (_clients.Any(c =>
            c.VpnIpAddress == client.VpnIpAddress))
        {
            throw new InvalidOperationException(
                "VPN IP address is already assigned.");
        }

        if (_clients.Any(c =>
            c.PublicKey == client.PublicKey))
        {
            throw new InvalidOperationException(
                "Public key is already registered.");
        }

        client.AssignToServer(Id);
        _clients.Add(client);
    }

    public void RemoveClient(Guid clientId)
    {
        var client = _clients.FirstOrDefault(
            c => c.Id == clientId)
            ?? throw new KeyNotFoundException(
                "VPN client was not found.");

        client.RemoveFromServer(Id);
        _clients.Remove(client);
    }
}
