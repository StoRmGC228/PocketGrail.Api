# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

```bash
# Build
dotnet build PocketGrail.Api.sln

# Run (port 5014)
dotnet run --project PocketGrail.Api/PocketGrail.Api.csproj

# EF Core migrations (run from solution root)
dotnet ef migrations add <Name> --project PocketGrail.Infrastructure --startup-project PocketGrail.Api
dotnet ef database update --project PocketGrail.Infrastructure --startup-project PocketGrail.Api
```

## Architecture

Clean Architecture with four projects:

- **PocketGrail.Domain** — entities (`User`, `Campaign`, `CampaignParticipant`, `BaseEntity`), enums (`UserRole`). No dependencies on other layers.
- **PocketGrail.Application** — service interfaces, DTOs, `JwtProvider`, `AuthService`, `CampaignService`, `CodeGeneratorService`. Depends only on Domain.
- **PocketGrail.Infrastructure** — EF Core (`PocketGrailDbContext`), repository implementations, email (MailKit), Cloudinary. Depends on Application + Domain.
- **PocketGrail.Api** — ASP.NET Core 8 controllers (`AuthController`, `CampaignsController`), SignalR hub (`CampaignHub`), global error middleware, DI wiring via `ServicesConfiguration`.

## Key Domain Concepts

- **UserRole:** `DungeonMaster` or `Player`. DMs own campaigns; Players join them.
- **Campaign:** Has a 6-char alphanumeric `ConnectionCode` (unique, retry-generated), a `PasswordHash` (BCrypt), an image via Cloudinary, and a `Participants` collection of `CampaignParticipant`.
- **Two-factor auth:** Login sends a 6-digit email code (Scriban-templated HTML), cached in `IMemoryCache`. `/verify` exchanges the code for a JWT stored in `MySecretCookies` (HttpOnly, SameSite=None).
- **Authorization policies:** `DungeonMasterOnly` and `PlayerAndAbove` are registered in `AuthConfiguration.cs`.

## Data Access

PostgreSQL via EF Core 8 (Npgsql). DbContext in `PocketGrail.Infrastructure`. Repositories follow the interface-per-aggregate pattern defined in `PocketGrail.Application/Interfaces/`.

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
