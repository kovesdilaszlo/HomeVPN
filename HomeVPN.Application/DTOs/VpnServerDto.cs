
namespace HomeVPN.Application.DTOs;

public sealed record VpnServerDto(
    Guid Id,
    string Name,
    string NetworkCidr,
    string ServerIpAddress,
    IReadOnlyList<VpnClientDto> Clients);
