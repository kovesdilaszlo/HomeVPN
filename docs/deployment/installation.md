# Installation Guide

**Status:** Planned, not yet validated end-to-end | **Version:** 0.1

## Prerequisites
- Linux mini PC (Ubuntu Server 24.04 LTS is a candidate).
- Docker Engine and Docker Compose plugin.
- Router administration access and stable mini PC LAN address.
- Working WireGuard support in the host kernel.
- Public IPv4/IPv6 connectivity or an alternative CGNAT traversal design.
- Windows and mobile WireGuard clients.

## Preflight
1. Identify LAN subnet, router address and mini PC NIC using `ip -br addr` and `ip route`.
2. Compare router WAN IPv4 with public IPv4; determine whether CGNAT/double NAT is present.
3. Select a non-overlapping VPN subnet (example `10.77.0.0/24`).
4. Reserve mini PC LAN IP in router DHCP settings.
5. Decide how administrators will reach the private UI.

## Deployment phases
1. Install and verify Docker on the mini PC.
2. Configure protected secrets and persistent storage.
3. Start and verify WireGuard container **without** public exposure of the management API.
4. Configure router UDP port forwarding only when a reachable public endpoint exists.
5. Test one client over mobile data.
6. Enable management agent and authenticated API after security review.
7. Enable Blazor UI on LAN/VPN only.

## Validation
- `docker compose ps` shows healthy services (once Compose exists).
- `wg show` reports the peer and recent handshake.
- Client reaches `10.77.0.1` and an authorized LAN service.
- Client's normal Internet traffic remains outside the tunnel.

## Rollback
Keep protected backups of the prior WireGuard configuration. Stop the stack, restore known-good configuration and restart after validating file permissions. Document exact commands once Compose paths are finalized.
