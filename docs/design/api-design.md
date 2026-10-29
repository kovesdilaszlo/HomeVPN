# API Design

**Status:** Proposed, not implemented | **Version:** 0.1

## Base path
`/api/v1`

| Method | Route | Purpose | Authorization |
|---|---|---|---|
| GET | `/vpn/status` | VPN health and tunnel status | Admin |
| GET | `/clients` | List clients without secret keys | Admin |
| POST | `/clients` | Enroll a device | Admin |
| GET | `/clients/{id}` | Read device metadata | Admin |
| DELETE | `/clients/{id}` | Revoke device | Admin |
| POST | `/clients/{id}/rotate` | Rotate credentials | Admin |

## Sample enrollment request
```json
{
  "name": "laptop",
  "requestedTunnelAddress": "10.77.0.2"
}
```

## Sample non-secret response
```json
{
  "id": "1d3d91c5-9c1d-4eab-89b1-8f6f1c81a0e2",
  "name": "laptop",
  "tunnelAddress": "10.77.0.2",
  "status": "Pending"
}
```

## Security contract
All management endpoints require authentication and admin authorization. Do not deploy anonymous create/revoke/config-export endpoints. For one-time config delivery, prevent caching (`Cache-Control: no-store`), redact logs, and use short-lived access.

## Error handling
Use `application/problem+json` with HTTP 400 for invalid input, 401/403 for auth failures, 404 for unknown device, 409 for IP/key conflict, 503 for unavailable agent.

## Versioning
Start with `/api/v1`; avoid breaking changes without explicit versioning.
