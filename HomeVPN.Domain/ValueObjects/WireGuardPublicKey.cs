
namespace HomeVPN.Domain.ValueObjects;

public readonly record struct WireGuardPublicKey
{
    public string Value { get; }

    public WireGuardPublicKey(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException(
                "WireGuard public key is required.",
                nameof(value));
        }

        var normalized = value.Trim();

        byte[] decoded;

        try
        {
            decoded = Convert.FromBase64String(normalized);
        }
        catch (FormatException)
        {
            throw new ArgumentException(
                "WireGuard public key must be valid Base64.",
                nameof(value));
        }

        if (decoded.Length != 32 ||
            normalized.Length != 44 ||
            Convert.ToBase64String(decoded) != normalized)
        {
            throw new ArgumentException(
                "WireGuard public key must contain " +
                "32 bytes in canonical Base64 format.",
                nameof(value));
        }

        Value = normalized;
    }

    public override string ToString() =>
        Value ?? string.Empty;
}
