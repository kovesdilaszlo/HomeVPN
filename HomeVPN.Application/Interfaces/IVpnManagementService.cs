
using HomeVPN.Application.DTOs;

namespace HomeVPN.Application.Interfaces;

public interface IVpnManagementService
{
    Task<VpnServerDto> CreateServerAsync(
        CreateVpnServerRequest request,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<VpnServerDto>> GetServersAsync(
        CancellationToken cancellationToken = default);

    Task<VpnServerDto> GetServerAsync(
        Guid serverId,
        CancellationToken cancellationToken = default);

    Task<VpnClientDto> AddClientAsync(
        Guid serverId,
        CreateVpnClientRequest request,
        CancellationToken cancellationToken = default);

    Task DisableClientAsync(
        Guid serverId,
        Guid clientId,
        CancellationToken cancellationToken = default);

    Task ActivateClientAsync(
        Guid serverId,
        Guid clientId,
        CancellationToken cancellationToken = default);

    Task RemoveClientAsync(
        Guid serverId,
        Guid clientId,
        CancellationToken cancellationToken = default);
}
