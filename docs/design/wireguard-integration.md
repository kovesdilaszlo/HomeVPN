# WireGuard Integration Design

**Status:** Proposed | **Version:** 0.1

## Principle
Use the official WireGuard implementation for tunneling and cryptography. HomeVPN manages peer lifecycle; it does not implement cryptographic primitives.

## Privileged agent responsibilities
- Generate peer keypairs and preshared keys securely.
- Validate allowed IPs, endpoint policy and peer uniqueness.
- Apply peer changes using supported WireGuard tooling.
- Persist canonical configuration safely and atomically.
- Return operational status and error codes without leaking secrets.

## Enrollment transaction
1. Application validates input and reserves a unique tunnel IP.
2. Agent creates keys and stages the peer configuration.
3. Agent applies changes to the live interface.
4. Agent persists the canonical state.
5. Application commits metadata and issues a one-time client config.
6. On failure, reconcile live and persistent state; do not silently claim success.

## Configuration shape (illustrative)
```ini
[Interface]
Address = 10.77.0.2/32
PrivateKey = <CLIENT_PRIVATE_KEY>

[Peer]
PublicKey = <SERVER_PUBLIC_KEY>
PresharedKey = <PSK>
Endpoint = vpn.example.net:51820
AllowedIPs = 10.77.0.0/24, 192.168.1.0/24
PersistentKeepalive = 25
```

## Revocation
Remove peer from live state and persistent configuration, invalidate exported configuration delivery, and record an audit event. A device's locally stored client config must be considered compromised if leaked.

## Cautions
Never concatenate untrusted user input into shell commands. Prefer argument-safe execution and strict allowlists. Do not share a writable WireGuard config volume directly with an Internet-facing API.
