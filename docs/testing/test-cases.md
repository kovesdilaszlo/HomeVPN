# Test Cases

**Status:** Planned | **Version:** 0.1

| ID | Scenario | Expected result | Type |
|---|---|---|---|
| TC-001 | Create client with valid unique VPN IP | Accepted and stored | Unit |
| TC-002 | Create client with duplicate IP | Conflict returned | Unit |
| TC-003 | Create client outside VPN subnet | Validation failure | Unit |
| TC-004 | Create client with invalid name | Validation failure | Unit |
| TC-005 | Anonymous GET clients | 401 or authentication challenge | API |
| TC-006 | Non-admin tries revoke | 403 | API |
| TC-007 | Admin provisions peer | Peer appears in WireGuard | Integration |
| TC-008 | Restart WireGuard service | Peer remains configured | Integration |
| TC-009 | Revoke peer | Peer removed from live and persisted config | Integration |
| TC-010 | Mobile client connects over cellular | Handshake succeeds | E2E |
| TC-011 | VPN client accesses allowed LAN host | Connection succeeds | E2E |
| TC-012 | VPN client accesses disallowed LAN host | Traffic blocked per policy | E2E |
| TC-013 | Client browses public Internet | Uses normal local Internet path | E2E |
| TC-014 | Agent command injection attempt | Rejected; no command executed | Security |
| TC-015 | One-time client config retrieved twice | Second attempt denied | Security |
| TC-016 | Review Git and app logs | No private key exposure | Security |

## Recording template
For each executed case, record: test ID, build/commit, environment, prerequisites, steps, expected result, actual result, pass/fail, evidence and issue reference.
