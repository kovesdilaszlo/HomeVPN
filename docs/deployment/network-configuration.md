# Network Configuration

**Status:** Proposed examples only | **Version:** 0.1

## Example addressing
| Network | Example |
|---|---|
| LAN | `192.168.1.0/24` |
| Router | `192.168.1.1` |
| Mini PC | `192.168.1.10` |
| VPN | `10.77.0.0/24` |
| VPN server | `10.77.0.1` |
| Windows peer | `10.77.0.2` |
| Mobile peer | `10.77.0.3` |
| UDP listener | `51820` |

## Split tunnel
Clients use `AllowedIPs = 10.77.0.0/24, 192.168.1.0/24`. This routes only VPN and home LAN traffic into the tunnel. The remote network must not overlap the home LAN subnet, otherwise routes may conflict.

## LAN return path
Two options: **SNAT/MASQUERADE** on the VPN gateway, simpler but hides peer source addresses from LAN hosts; or **routed mode** with a static route to the VPN subnet on the home router, preserving peer source addresses. Choose one and test firewall behavior.

## Router
Forward UDP `51820` to the mini PC only if its WAN endpoint is reachable. If the WAN IP is private or CGNAT-range (`100.64.0.0/10`), ordinary IPv4 forwarding is insufficient. Consider public IPv4 from ISP, reachable IPv6, or a relay/VPS architecture.

## Firewall
Allow WireGuard UDP inbound, permit only intended VPN-to-LAN forwarding and related return traffic, and block administrative endpoints from the public Internet. Docker's firewall integration can interact with UFW/nftables; validate on the host rather than assuming default rules are sufficient.

## Diagnostics
- `ip route` / `ip -br addr`: addresses and routes.
- `wg show`: tunnel handshake and counters.
- `ss -lun`: UDP listener.
- `ping` or TCP connection to an authorized LAN service.
- Compare remote and home LAN CIDRs for overlap.
