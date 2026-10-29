# Functional Requirements

**Status:** Proposed | **Version:** 0.1

## Goal
Enable an authorized administrator to manage WireGuard access to a home LAN for Windows and mobile clients.

| ID | Requirement | Priority | Acceptance criterion |
|---|---|---|---|
| FR-01 | Authenticate administrators | Must | Anonymous requests to management endpoints are rejected. |
| FR-02 | Register a VPN device | Must | Unique device name and tunnel IP are validated before creation. |
| FR-03 | Generate WireGuard key material securely | Must | Keys are generated using a trusted CSPRNG or WireGuard tooling. |
| FR-04 | Provision a peer on the WireGuard server | Must | Provisioned peer appears in `wg show` and survives restart. |
| FR-05 | Export a client configuration | Must | Config contains correct endpoint, server key, tunnel address and routes. |
| FR-06 | List registered devices | Must | API returns metadata, never private keys. |
| FR-07 | Revoke a device | Must | Revoked peer cannot establish new sessions after removal. |
| FR-08 | Report server and peer status | Should | Status includes interface state and last handshake when available. |
| FR-09 | Present management UI | Should | Blazor can manage devices through authenticated API. |
| FR-10 | Audit administrative changes | Must | Actor, operation, time and outcome recorded without secrets. |
| FR-11 | Rotate device credentials | Should | Old peer credentials are invalidated during replacement. |
| FR-12 | Export mobile QR code | Should | QR is shown once over a protected admin session. |

## Actors
**Administrator:** manages VPN devices and deployment. **VPN client:** connects using a WireGuard configuration; it does not call the management API.

## Out of scope for v1
Full-tunnel Internet egress, multi-site routing, multi-admin roles, automatic public DNS registration, and writing a new VPN protocol.

## Use case: enroll device
1. Administrator signs in.
2. Administrator provides a device name and requested address (or selects automatic allocation).
3. Application checks name/IP uniqueness and policy.
4. Privileged WireGuard agent creates key material and installs the peer.
5. Application records non-secret metadata and returns a one-time configuration securely.
6. Administrator imports configuration on the client and verifies connectivity.

## Open decisions
Public IPv4 vs CGNAT/IPv6; actual LAN subnet; host firewall; persistent data store; client-config delivery and one-time download policy.
