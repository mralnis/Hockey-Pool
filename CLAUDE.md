# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

Solution targets **.NET 10**. The EF Core CLI is pinned via the local tool manifest at `HockeyPool/.config/dotnet-tools.json`; run `dotnet tool restore` once before using `dotnet ef`.

- Build: `dotnet build HockeyPool.sln`
- Run the web app: `dotnet run --project HockeyPool` (DB is created/migrated/seeded automatically on startup)
- Add EF migration: `dotnet ef migrations add <Name> --project Infrastructure --startup-project HockeyPool`
- Apply migrations manually: `dotnet ef database update --project Infrastructure --startup-project HockeyPool`

There is no test project in this solution.

## Architecture

ASP.NET Core Blazor Server app with interactive server components, MudBlazor UI, ASP.NET Core Identity, and EF Core. Two projects:

- **HockeyPool** (`Microsoft.NET.Sdk.Web`) — Blazor Server host, Razor components, Identity wiring, HTTP pipeline.
- **Infrastructure** (`Microsoft.NET.Sdk`) — `ApplicationDbContext`, EF Core models, migrations, and the repository/UnitOfWork layer. The web project references it and exposes its namespaces via global `<Using>` entries in `HockeyPool.csproj`, so `HockeyPool.Infrastructure.Data` and `…Models` are available everywhere without explicit `using` statements.

### Startup flow (`HockeyPool/Program.cs`)

DI registration is split into extension methods under `HockeyPool/Configuration/`:

- `AddHockeyPoolAuthentication` — cascading auth state, Identity cookies, `IdentityRevalidatingAuthenticationStateProvider`.
- `AddHockeyPoolIdentity` — `IdentityCore<ApplicationUser>` with **relaxed password rules** (min length 3, no digit/case/symbol requirements), `RequireConfirmedAccount = false`, custom `AppErrorDescriber`, no-op email sender.
- `AddHockeyPoolDatabase` — registers `ApplicationDbContext` and calls `AddRepositories` (which registers `UnitOfWork` scoped). **Note:** `DatabaseConfiguration.cs` hardcodes `UseSqlite("Data Source=database.dat")` and ignores the `DefaultConnection` string in `appsettings.json` (which still mentions SQL Server LocalDB) — change this file if the provider needs to change.
- `SetupHttpPipeline` — dev vs. prod error handling and HTTPS redirect.

After `app.Build()`, `app.SetupHockeyPoolDBAsync()` (in `Infrastructure/Data/DataSetup.cs`) runs `EnsureCreatedAsync` → `MigrateAsync` (errors swallowed) → `SeedAsync`. Seeding (in `ApplicationDbContext.SeedAsync`) populates Countries, the active tournament + matchups (currently `IIHF 2026` — see `SeedWC2026` / `SeedWC2026Matchups`), the `Admin` role, and **creates a default admin user `admin`/`admin`** if no users exist. Edit these seed methods when rolling over to a new tournament.

### Data access pattern

Components do **not** inject `ApplicationDbContext` directly. They inject `UnitOfWork` (`Infrastructure/Data/UnitOfWork.cs`), which lazily exposes typed repositories: `CountryRepository`, `MatchupRepository`, `PredictionsRepository`, `TournamentRepository`, `UserRepository`, `TournamentWinnerRepository`, `PredictionLogRepository`, `RoleRepository`. Generic CRUD lives in `GenericRepository<T>`; domain queries live in the specific repos under `Infrastructure/Data/Repos/`. `MatchupRepository` is constructed with `TournamentRepository` because matchup behavior depends on the active tournament's `MatchupClosingTime`.

### Domain model

Tournament → has many Matchups (Home/Guest `Country` + `GameTime`). Users (`ApplicationUser` extends `IdentityUser`) join tournaments via `PlayerTournament` and submit `Prediction`s per matchup. `PredictionLog` audits prediction edits. `TournamentWinner` records final standings. Scoring weights live on `Tournament` (`PointsForPerfect`, `PointForDifference`, `PointsForWinnerOnly`) and the cutoff window in `MatchupClosingTime` (minutes before `GameTime`).

### UI structure

Razor components under `HockeyPool/Components/` are grouped by feature: `BettingOverview` (current matchups + prediction entry, with `ScoreDialog`), `BettingHistory`, `Winners`, `Rules`, `Admin` (tournament/matchup/country/prediction/user management, restricted to the `Admin` role), `Account` (Identity pages, custom `IdentityRedirectManager` and revalidating auth state provider), and `Layout` (`MainLayout`, `NavMenu`, `ServerTime`). Routing is in `Components/Routes.razor` using `AuthorizeRouteView` + `RedirectToLogin`. MudBlazor is registered via `AddMudServices()`.

## Deployment notes

From `HockeyPool/ReadMe.txt`: when hosting on Azure App Service, set the app setting `WEBSITE_TIME_ZONE = FLE Standard Time` so server-side times (matchup cutoffs, `ServerTime` component, `DateTimeExtensions`) match the expected Latvian/European timezone.
