# HomeVPN

**Status:** Planning / initial solution scaffolding  
**Documentation version:** 0.1  
**Target runtime:** Linux mini PC, Docker Compose  
**Stack:** C# / .NET 10, ASP.NET Core Web API, Blazor, WireGuard

HomeVPN is a self-hosted management system for securely accessing a private home LAN from Windows and mobile devices. WireGuard provides the VPN tunnel; HomeVPN provides administrative workflows. **This repository is not a new VPN cryptographic implementation.**

## Scope
- Split-tunnel access to selected home LAN subnets.
- Device enrollment, revocation, and connection status (planned).
- Authenticated administration via Web API and Blazor (planned).
- Linux deployment using Docker Compose (planned).

## Project layout
- `HomeVPN.Domain`: entities, value objects, domain rules.
- `HomeVPN.Application`: use cases and ports/interfaces.
- `HomeVPN.Infrastructure`: persistence and integration adapters.
- `HomeVPN.API`: administrative HTTP interface.
- `HomeVPN.Web`: Blazor administration interface.
- `HomeVPN.UnitTests`, `HomeVPN.IntegrationTests`: automated tests.

## Documentation
- [Functional requirements](docs/requirements/functional-requirements.md)
- [Non-functional requirements](docs/requirements/non-functional-requirements.md)
- [System architecture](docs/architecture/system-architecture.md)
- [Clean Architecture](docs/architecture/clean-architecture.md)
- [Docker architecture](docs/architecture/docker-architecture.md)
- [Domain model](docs/design/domain-model.md)
- [API design](docs/design/api-design.md)
- [WireGuard integration](docs/design/wireguard-integration.md)
- [Installation](docs/deployment/installation.md)
- [Docker setup](docs/deployment/docker-setup.md)
- [Network configuration](docs/deployment/network-configuration.md)
- [Security design](docs/security/security-design.md)
- [Test strategy](docs/testing/test-strategy.md)
- [Test cases](docs/testing/test-cases.md)

## Current state
The Visual Studio solution and project structure have been created. No production-ready VPN provisioning or authenticated API is assumed implemented. All endpoint examples and deployment steps are **proposals** until verified in the codebase.

## Security warning
Never commit private keys, generated peer configuration files, `.env`, secrets, or database credentials. Do not expose the administration API publicly. Deploy only after completing the security checklist and tests.

## Development
Open `HomeVPN.slnx` in Visual Studio 2026 and build the solution using .NET 10. The repository's existing project names and physical paths should be checked before copying the documentation into place.

## License
To be determined.
