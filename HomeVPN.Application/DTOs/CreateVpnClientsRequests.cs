namespace HomeVPN.Application.DTOs;

public sealed record CreateVpnClientRequest(
    string Name,
    string PublicKey,
    string VpnIpAddress);
