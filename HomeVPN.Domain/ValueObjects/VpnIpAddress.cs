
using System.Net;
using System.Net.Sockets;

namespace HomeVPN.Domain.ValueObjects;

public readonly record struct VpnIpAddress
{
    public IPAddress Value { get; }

    public VpnIpAddress(string value)
    {
        if (string.IsNullOrWhiteSpace(value) ||
            !IPAddress.TryParse(value, out var parsed) ||
            parsed.AddressFamily != AddressFamily.InterNetwork ||
            value.Trim().Count(c => c == '.') != 3 ||
            !string.Equals(
                parsed.ToString(),
                value.Trim(),
                StringComparison.Ordinal))
        {
            throw new ArgumentException(
                "A canonical IPv4 address is required.",
                nameof(value));
        }

        Value = parsed;
    }

    public override string ToString() =>
        Value?.ToString() ?? string.Empty;
}
