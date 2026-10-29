# Docker Setup

**Status:** Planned | **Version:** 0.1

## Development environment
Use Visual Studio 2026 with .NET 10 SDK and Linux Dockerfile support for `HomeVPN.API`. Docker Desktop may be used for API development on Windows; actual WireGuard routing must be verified on the target Linux host.

## Files to introduce
- `compose.yaml`: service definitions and private networks.
- `.env.example`: documented non-secret environment variable names.
- `.dockerignore`: exclude build artifacts and sensitive files from build context.
- `docker/wireguard/`: configuration templates and operational notes.

## Secure defaults
- Do not mount Docker socket into the API.
- Do not run the API container privileged.
- Grant WireGuard-specific capabilities only to the WireGuard container.
- Publish only WireGuard UDP to the Internet.
- Keep API and agent on private networks with explicit authentication.
- Use fixed image versions rather than floating `latest` in production.
- Mount secrets read-only where feasible and protect persistent key volumes.

## Development workflow
1. Build `HomeVPN.API` locally.
2. Run API tests.
3. Build API Docker image.
4. Add Compose incrementally after privileged-agent design is settled.
5. Test restart and persistence on the Linux mini PC.

## Known limitation
A Visual Studio-generated Dockerfile alone is not a working multi-container VPN deployment. A validated Compose manifest, network configuration and host firewall rules are still required.
