using HomeVPN.Domain.Enums;

namespace HomeVPN.Application.DTOs;

public sealed record VpnClientDto
    (
    Guid Id,
    string Name,
    string PublicKey,
    string VpnIpAddress,
    VpnClientStatus Status,
    Guid? ServerId,
    DateTime CreatedAtUtc
    );
