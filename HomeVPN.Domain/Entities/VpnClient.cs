
using HomeVPN.Domain.Enums;
using HomeVPN.Domain.ValueObjects;

namespace HomeVPN.Domain.Entities;

public sealed class VpnClient
{
    public Guid Id { get; private set; }

    public string Name { get; private set; }

    public string PublicKey { get; private set; }

    public string VpnIpAddress { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }

    public VpnClientStatus Status { get; private set; }

    public Guid? ServerId { get; private set; }

    private VpnClient()
    {
        Name = string.Empty;
        PublicKey = string.Empty;
        VpnIpAddress = string.Empty;
        Status = VpnClientStatus.Disabled;
    }

    public VpnClient(
        string name,
        string publicKey,
        string vpnIpAddress)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(
                "Client name is required.",
                nameof(name));
        }

        var key = new WireGuardPublicKey(publicKey);
        var address = new VpnIpAddress(vpnIpAddress);

        Id = Guid.NewGuid();
        Name = name.Trim();
        PublicKey = key.ToString();
        VpnIpAddress = address.ToString();
        CreatedAtUtc = DateTime.UtcNow;
        Status = VpnClientStatus.Active;
    }

    public void AssignToServer(Guid serverId)
    {
        if (serverId == Guid.Empty)
        {
            throw new ArgumentException(
                "A valid server ID is required.",
                nameof(serverId));
        }

        if (ServerId.HasValue && ServerId != serverId)
        {
            throw new InvalidOperationException(
                "Client is already assigned to another server.");
        }

        ServerId = serverId;
    }

    public void RemoveFromServer(Guid serverId)
    {
        if (ServerId != serverId)
        {
            throw new InvalidOperationException(
                "Client is not assigned to this server.");
        }

        ServerId = null;
    }

    public void Disable()
    {
        Status = VpnClientStatus.Disabled;
    }

    public void Activate()
    {
        Status = VpnClientStatus.Active;
    }
}
