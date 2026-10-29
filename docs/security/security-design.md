# Security Design

**Status:** Mandatory design requirements | **Version:** 0.1

## Threat model
Threats include stolen client config, leaked server key, compromised admin credentials, exposed management API, malicious input to privileged agent, container breakout and unauthorized LAN access.

## Controls
1. **Protocol:** use WireGuard's maintained implementation; do not invent cryptography.
2. **Admin authentication:** enforce authentication and authorization before any sensitive endpoint is deployed.
3. **Network boundary:** bind admin API/UI to LAN or VPN only; never publish it to the public Internet.
4. **Privilege separation:** API unprivileged; WireGuard-specific capabilities isolated; no Docker socket in API.
5. **Agent interface:** mutually authenticated private channel, fixed command set, strict input validation and rate limits.
6. **Keys:** generate with CSPRNG; limit filesystem access; never log, commit or include in crash reports.
7. **Client export:** one-time, short-lived, no-store responses; QR code only over protected session.
8. **Audit:** log who changed which peer and when, but not private keys or full config content.
9. **Updates:** patch Linux, Docker, WireGuard and .NET images regularly.
10. **Backup:** encrypt backups and rehearse restoration.

## Repository hygiene
Ignore `.env`, generated `.conf`, `*.key`, `*.pfx`, secrets directories, Docker volumes, `bin`, `obj`, `.vs`. Review `git status` before commit. If a secret was ever committed, rotate it; `.gitignore` cannot erase Git history.

## Release gate
Do not expose a management endpoint until auth, authorization, input validation, secret handling, audit logging and negative tests are in place.
