
using HomeVPN.Domain.Entities;
using HomeVPN.Domain.ValueObjects;
using Xunit;

namespace HomeVPN.UnitTests.DomainTests;

public class VpnNetworkTests
{
    private static string ValidKey =>
        Convert.ToBase64String(
            Enumerable.Repeat((byte)1, 32).ToArray());

    [Theory]
    [InlineData("10.8.0.0/24", "10.8.0.2", true)]
    [InlineData("10.8.0.0/24", "192.168.1.2", false)]
    [InlineData("10.8.0.0/24", "10.8.0.0", true)]
    [InlineData("10.8.0.0/24", "10.8.0.255", true)]
    [InlineData("0.0.0.0/0", "203.0.113.5", true)]
    [InlineData("10.8.0.2/32", "10.8.0.2", true)]
    public void Contains_ChecksMembership(
        string cidr,
        string ip,
        bool expected)
    {
        var network = new VpnNetwork(cidr);
        var address = new VpnIpAddress(ip);

        Assert.Equal(expected, network.Contains(address));
    }

    [Theory]
    [InlineData("10.8.0.0/24", "10.8.0.2", true)]
    [InlineData("10.8.0.0/24", "10.8.0.0", false)]
    [InlineData("10.8.0.0/24", "10.8.0.255", false)]
    [InlineData("10.8.0.0/31", "10.8.0.1", true)]
    [InlineData("10.8.0.2/32", "10.8.0.2", true)]
    public void IsUsableHost_ChecksHostRules(
        string cidr,
        string ip,
        bool expected)
    {
        var network = new VpnNetwork(cidr);
        var address = new VpnIpAddress(ip);

        Assert.Equal(
            expected,
            network.IsUsableHost(address));
    }

    [Theory]
    [InlineData("10.8.0.0/33")]
    [InlineData("10.8.0.0/-1")]
    [InlineData("10.8.0.1/24")]
    [InlineData("not-a-cidr")]
    [InlineData("10.8.0.0/abc")]
    public void Constructor_RejectsInvalidCidr(string cidr)
    {
        Assert.Throws<ArgumentException>(
            () => new VpnNetwork(cidr));
    }

    [Theory]
    [InlineData("999.1.2.3")]
    [InlineData("2001:db8::1")]
    [InlineData("10.8.0")]
    public void IpAddress_RejectsInvalidIpv4(string ip)
    {
        Assert.Throws<ArgumentException>(
            () => new VpnIpAddress(ip));
    }

    [Fact]
    public void AddClient_RejectsAddressOutsideNetwork()
    {
        var server = new VpnServer(
            "Home VPN", "10.8.0.0/24", "10.8.0.1");

        var client = new VpnClient(
            "Laptop", ValidKey, "192.168.1.2");

        Assert.Throws<InvalidOperationException>(
            () => server.AddClient(client));

        Assert.Null(client.ServerId);
    }

    [Fact]
    public void AddClient_RejectsNetworkAddress()
    {
        var server = new VpnServer(
            "Home VPN", "10.8.0.0/24", "10.8.0.1");

        var client = new VpnClient(
            "Laptop", ValidKey, "10.8.0.0");

        Assert.Throws<InvalidOperationException>(
            () => server.AddClient(client));
    }
}
