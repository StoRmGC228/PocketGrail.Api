# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

```bash
# Build
dotnet build PocketGrail.Api.sln

# Run (port 5014)
dotnet run --project PocketGrail.Api/PocketGrail.Api.csproj

# EF Core migrations (run from solution root)
dotnet ef migrations add <Name> --project PocketGrail.DataAccess --startup-project PocketGrail.Api
dotnet ef database update --project PocketGrail.DataAccess --startup-project PocketGrail.Api
```

## Architecture

Clean Architecture with five projects:

- **PocketGrail.Domain** — Rich domain aggregates (`Character`, `Campaign`, `User`), value objects (`CharacterStats`, `CharacterWallet`), supporting types, domain exceptions. No project dependencies.
- **PocketGrail.DataAccess** — EF Core (`PocketGrailDbContext`), all persistence entities, Fluent API configurations, migrations, repository implementations and interfaces. No project dependencies (EF Core + Npgsql NuGet only).
- **PocketGrail.Infrastructure** — Email (MailKit/Scriban) and Cloudinary integrations only. No project dependencies.
- **PocketGrail.Application** — Thin orchestration layer: service interfaces, DTOs, mappers (DataAccess ↔ Domain ↔ DTO), `AuthService`, `CampaignService`, `CharacterService`. Depends on Domain + DataAccess + Infrastructure. DI composition root (`AddApplicationServices`).
- **PocketGrail.Api** — ASP.NET Core 8 controllers, SignalR hub (`CampaignHub`), global error middleware. References Application and DataAccess. DI wired via `AddPocketGrailServices` → `AddApplicationServices`.

## Key Domain Concepts

- **UserRole:** `DungeonMaster` or `Player`. DMs own campaigns; Players join them.
- **Campaign:** Has a 6-char alphanumeric `ConnectionCode` (unique, retry-generated), a `PasswordHash` (BCrypt), an image via Cloudinary, and a `Participants` collection of `CampaignParticipant`.
- **Two-factor auth:** Login sends a 6-digit email code (Scriban-templated HTML), cached in `IMemoryCache`. `/verify` exchanges the code for a JWT stored in `MySecretCookies` (HttpOnly, SameSite=None).
- **Authorization policies:** `DungeonMasterOnly` and `PlayerAndAbove` are registered in `AuthConfiguration.cs`.

## Data Access

PostgreSQL via EF Core 8 (Npgsql). `PocketGrailDbContext` lives in `PocketGrail.DataAccess/Context/`. Repository interfaces and implementations are co-located in `PocketGrail.DataAccess/Interfaces/` and `PocketGrail.DataAccess/Repositories/`.

Key repository methods to be aware of: `GetByCodeAsync`, `IsUserParticipantAsync`, `CodeExistsAsync` on `ICampaignRepository`.

## Environment Variables

All secrets come from environment variables (never appsettings.json):

| Variable | Purpose |
|---|---|
| `JWT_SECRET` | JWT signing key (required) |
| `POCKET_GRAIL_CONNECTION_STRING` | PostgreSQL connection string |
| `EMAIL_SENDER_ADDRESS`, `EMAIL_SENDER_NAME`, `SMTP_HOST`, `SMTP_PORT`, `SMTP_USERNAME` | MailKit SMTP |
| `CLOUDINARY_CLOUD_NAME`, `CLOUDINARY_API_KEY`, `CLOUDINARY_API_SECRET` | Cloudinary |

## Real-time

SignalR hub at `/hubs/campaign` emits `ParticipantJoined` and `ParticipantLeft` events. The hub is called from `CampaignService` via `IHubContext<CampaignHub>`.

## CORS

Allows credentials from `localhost`, `*.ngrok-free.app`, and `*.ngrok.io`. Update `ServicesConfiguration.cs` when adding new allowed origins.
