namespace HomeVPN.Application.DTOs;

public sealed record CreateVpnServerRequest(
    string Name,
    string NetworkCidr,
    string ServerIpAddress);
