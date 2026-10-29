# Docker Architecture

**Status:** Proposed | **Version:** 0.1

## Services
| Service | Privilege | Purpose | Exposure |
|---|---|---|---|
| `wireguard` | `NET_ADMIN`, kernel support | VPN tunnel and LAN forwarding | UDP 51820 only |
| `homevpn-agent` | Restricted, audited privileged operations | Apply/revoke peers and read status | Internal only |
| `homevpn-api` | Non-root | Authenticated management API | Private only |
| `homevpn-web` | Non-root | Blazor interface | LAN/VPN only |
| `database` (optional) | Non-root | Device metadata | Internal only |

## Separation
The management API must not receive Docker socket access or broad host capabilities. The privileged agent is an explicit trust boundary; authenticate requests between API and agent and allowlist operations. The exact agent implementation is not yet selected.

## Persistence
Persist WireGuard server state, non-secret metadata and application configuration using dedicated volumes or protected bind mounts. Never bake keys into images.

## Networking
The WireGuard container may require a host route, forwarding rules, `NET_ADMIN`, and access to kernel WireGuard support. Use minimal capabilities and test host firewall interactions. The admin API must not publish a public port.

## Build
The Visual Studio-generated `HomeVPN.API/Dockerfile` is a starting point. A Compose manifest will be added only after service boundaries and paths are verified. `docker compose up -d --build` is a **future deployment command**, not evidence that the stack currently works.

## Operations
Define health checks, restart policies, pinned image versions, secure volume ownership, backups and log redaction before production.
