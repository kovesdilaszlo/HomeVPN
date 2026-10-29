
using System.Buffers.Binary;
using System.Net;

namespace HomeVPN.Domain.ValueObjects;

public sealed record VpnNetwork
{
    public VpnIpAddress NetworkAddress { get; }
    public int PrefixLength { get; }

    public VpnNetwork(string cidr)
    {
        if (string.IsNullOrWhiteSpace(cidr))
            throw new ArgumentException(
                "CIDR is required.",
                nameof(cidr));

        var parts = cidr.Split('/');

        if (parts.Length != 2 ||
            !int.TryParse(parts[1], out var prefix) ||
            prefix is < 0 or > 32 ||
            !string.Equals(
                prefix.ToString(),
                parts[1],
                StringComparison.Ordinal))
        {
            throw new ArgumentException(
                "Invalid IPv4 CIDR prefix.",
                nameof(cidr));
        }

        var address = new VpnIpAddress(parts[0]);
        var bits = ToUInt32(address.Value);
        var mask = Mask(prefix);

        if ((bits & mask) != bits)
        {
            throw new ArgumentException(
                "CIDR must use its network address.",
                nameof(cidr));
        }

        NetworkAddress = address;
        PrefixLength = prefix;
    }

    public bool Contains(VpnIpAddress address) =>
        (ToUInt32(address.Value) & Mask(PrefixLength))
            == ToUInt32(NetworkAddress.Value);

    public bool IsUsableHost(VpnIpAddress address)
    {
        if (!Contains(address))
            return false;

        // /31 point-to-point and /32 single-host networks.
        if (PrefixLength >= 31)
            return true;

        var value = ToUInt32(address.Value);
        var network = ToUInt32(NetworkAddress.Value);
        var mask = Mask(PrefixLength);

        return value != network &&
               value != (network | ~mask);
    }

    public override string ToString() =>
        $"{NetworkAddress}/{PrefixLength}";

    private static uint ToUInt32(IPAddress address) =>
        BinaryPrimitives.ReadUInt32BigEndian(
            address.GetAddressBytes());

    private static uint Mask(int prefix) =>
        prefix == 0
            ? 0u
            : uint.MaxValue << (32 - prefix);
}
