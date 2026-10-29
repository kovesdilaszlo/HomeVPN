
using HomeVPN.Domain.Entities;
using Xunit;

namespace HomeVPN.UnitTests.DomainTests;

public class VpnClientTests
{
    private static string ValidKey =>
        Convert.ToBase64String(
            Enumerable.Range(1, 32)
                .Select(i => (byte)i)
                .ToArray());

    [Fact]
    public void Constructor_WithValidData_CreatesClient()
    {
        var client = new VpnClient(
            "My Laptop",
            ValidKey,
            "10.8.0.2");

        Assert.Equal("My Laptop", client.Name);
        Assert.Equal(ValidKey, client.PublicKey);
        Assert.Equal("10.8.0.2", client.VpnIpAddress);
        Assert.NotEqual(Guid.Empty, client.Id);
    }

    [Fact]
    public void Constructor_WithEmptyName_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() =>
            new VpnClient(
                "",
                ValidKey,
                "10.8.0.2"));
    }

    [Fact]
    public void Constructor_WithInvalidIp_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() =>
            new VpnClient(
                "My Laptop",
                ValidKey,
                "invalid-ip"));
    }

    [Fact]
    public void Constructor_WithInvalidPublicKey_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() =>
            new VpnClient(
                "My Laptop",
                "invalid-key",
                "10.8.0.2"));
    }
}
