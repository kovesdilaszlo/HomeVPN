# Non-Functional Requirements

**Status:** Proposed | **Version:** 0.1

| ID | Category | Requirement / verification |
|---|---|---|
| NFR-01 | Security | Use WireGuard rather than custom cryptography; test encryption through working tunnel. |
| NFR-02 | Least privilege | API container runs unprivileged without Docker socket, host networking or WireGuard keys. |
| NFR-03 | Secret protection | Private keys are never logged or committed; secret files use restrictive permissions. |
| NFR-04 | Reliability | Peer configuration persists across container/host restarts; test restart. |
| NFR-05 | Availability | Service recovers after reboot via Docker restart policy and persistent storage. |
| NFR-06 | Maintainability | Domain and Application are independent of Linux, Docker and ASP.NET Core. |
| NFR-07 | Portability | .NET 10 application builds on Windows and targets Linux containers. |
| NFR-08 | Testability | Business rules are unit-tested; privileged integration is isolated and testable. |
| NFR-09 | Observability | Structured logs record operations without credentials or full client configs. |
| NFR-10 | Network isolation | Only VPN UDP port is exposed to the Internet; admin API is private. |
| NFR-11 | Backup | Encrypted backup and documented restore procedure for configuration and metadata. |
| NFR-12 | Usability | Clear validation errors and one-time onboarding instructions. |

## Performance and sizing
No throughput or concurrent-client SLO is committed until mini PC hardware and uplink are known. Baseline measurements will include handshake time, LAN latency and transfer throughput.

## Compliance and privacy
Store only necessary client metadata, audit events and operational state. Define retention periods before deployment.
