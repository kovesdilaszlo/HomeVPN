
using HomeVPN.Domain.ValueObjects;
using Xunit;

namespace HomeVPN.UnitTests.DomainTests;

public class WireGuardPublicKeyTests
{
    private static string ValidKey =>
        Convert.ToBase64String(
            Enumerable.Range(1, 32)
                .Select(i => (byte)i)
                .ToArray());

    [Fact]
    public void Constructor_WithValidKey_CreatesKey()
    {
        var key = new WireGuardPublicKey(ValidKey);

        Assert.Equal(ValidKey, key.Value);
    }

    [Fact]
    public void Constructor_WithEmptyKey_ThrowsException()
    {
        Assert.Throws<ArgumentException>(
            () => new WireGuardPublicKey(""));
    }

    [Fact]
    public void Constructor_WithInvalidBase64_ThrowsException()
    {
        Assert.Throws<ArgumentException>(
            () => new WireGuardPublicKey("not-a-key"));
    }

    [Fact]
    public void Constructor_WithWrongByteLength_ThrowsException()
    {
        var invalidKey = Convert.ToBase64String(
            new byte[16]);

        Assert.Throws<ArgumentException>(
            () => new WireGuardPublicKey(invalidKey));
    }

    [Fact]
    public void Constructor_WithNonCanonicalBase64_ThrowsException()
    {
        var invalidKey = ValidKey.TrimEnd('=');

        Assert.Throws<ArgumentException>(
            () => new WireGuardPublicKey(invalidKey));
    }
}
