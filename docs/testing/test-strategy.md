# Test Strategy

**Status:** Planned | **Version:** 0.1

## Test levels
- **Domain unit tests:** IP/CIDR validation, client uniqueness, status transitions.
- **Application unit tests:** enrollment/revocation orchestration using fake repositories and fake agent.
- **API integration tests:** authentication, authorization, validation, response shapes and error codes.
- **Agent integration tests:** WireGuard command execution, persistence and rollback on Linux test environment.
- **Network end-to-end tests:** Windows/mobile clients reach authorized LAN targets over an external connection.

## Test environment
Use isolated test keys and dedicated subnets. Never run destructive integration tests against a production WireGuard interface. Docker-based tests may not reproduce every host networking behavior; include tests on target Linux hardware.

## Exit criteria for v1
- All automated tests pass.
- Unauthenticated requests are denied.
- Enrollment and revocation work after service restart.
- VPN client reaches permitted LAN resource.
- Non-LAN Internet traffic stays on the client's normal route.
- No private keys appear in Git, logs or ordinary API responses.
- Backup and restore have been exercised.

## Evidence
Record command outputs with secrets redacted, test environment, date, results, failures and fixes.
