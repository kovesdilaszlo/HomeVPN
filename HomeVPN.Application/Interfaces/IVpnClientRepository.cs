
using HomeVPN.Domain.Entities;

namespace HomeVPN.Application.Interfaces;

public interface IVpnServerRepository
{
    Task<VpnServer?> GetByIdAsync(
        Guid serverId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<VpnServer>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task AddAsync(
        VpnServer server,
        CancellationToken cancellationToken = default);

    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);
}
