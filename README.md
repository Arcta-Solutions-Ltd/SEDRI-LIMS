# Sedri LIMS

An open source, microbiology-focused Laboratory Information Management System (LIMS).

Sedri LIMS covers the day-to-day workflow of a microbiology lab: specimen accessioning,
culture and antimicrobial susceptibility testing (AST), organism/breakpoint coding against
guideline bodies such as CLSI/EUCAST, expert rules, and configurable reporting — with
multi-laboratory and role-based access support.

## Overview

- **Backend**: [.NET 8](https://dotnet.microsoft.com/) Web API (`arc.api`), with domain, data
  access, and identity split into their own class libraries.
- **Frontend**: React SPA (`arcportal`), built with Create React App (`react-app-rewired`).
- **Database**: PostgreSQL, versioned with [yuniql](https://yuniql.io/) migrations.

Much of the application's behaviour (forms, pages, queries, events, views) is
configuration-driven rather than hardcoded, resolved through factory classes in `arc.app/Config/`.

## Repository Layout

```
sedri-lims/
├── arc.api/              # Web API host (.NET 8) — controllers, appsettings, startup
├── arc.app/               # Configuration-driven application layer (Queries/Events/Forms/Pages)
├── arc.common/            # Shared models, utilities, logging abstractions
├── arc.data/               # Data access (repositories, commands/queries per feature area)
├── arc.data.model/         # Data models used for SQL parameter/type inference
├── arc.domain/             # Domain logic
├── arc.identity/           # Authentication (local JWT, optional Azure AD)
├── arcportal/               # React SPA frontend
├── yuniqlWorkspaces/        # Versioned PostgreSQL schema/seed migrations (yuniql)
└── arc.sln                  # Solution file
```

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js](https://nodejs.org/) (LTS) and npm
- [PostgreSQL](https://www.postgresql.org/) (local instance, default port `5432`)
- [yuniql CLI](https://yuniql.io/docs/) — used to apply the versioned database migrations under
  `yuniqlWorkspaces/base`

## Getting Started

### 1. Clone

```powershell
git clone <this-repository-url>
cd sedri-lims
```

### 2. Database

Create a local PostgreSQL database (e.g. `SedriLims`), then apply the base migrations:

```powershell
cd yuniqlWorkspaces/base
yuniql run --platform postgresql -c "Server=localhost;Port=5432;Database=SedriLims;User Id=postgres;Password=<your-password>" -a -p "."
```

A convenience wrapper is also available: `yuniqlWorkspaces/base/run.ps1` (prompts for a database
name if not supplied). See [yuniqlWorkspaces/README.md](yuniqlWorkspaces/README.md) if you want a
non-English base workspace composed from `yuniqlWorkspaces/base-languages`.

> **Guideline data is not included.** CLSI and EUCAST breakpoints, expert rules and QC target
> ranges are copyrighted by their publishers, so the migrations don't include them. A new
> database has the schema and reference data (organisms, organism groups, antibiotics) but no
> breakpoints, expert rules or QC ranges. Add the breakpoints and expert rules your laboratory is
> licensed to use through the app's breakpoint and expert-rule screens, or load them directly into
> the `breakpoint`, `resultline` and `expertrule*` tables. Load QC ranges into `qcantibiotics`.

### 3. Backend (API)

```powershell
# From the repo root
cp arc.api/appsettings.example.json arc.api/appsettings.json
```

Edit `arc.api/appsettings.json`:

- Set `ConnectionStrings:arcConnection` to point at the database you created above.
- Set `Authentication:Local:Key` to your own long random secret (used to sign local JWTs).
- Leave `Authentication:AzureAd:Enabled` as `false` unless you're wiring up your own Azure AD
  tenant (fill in your own `TenantId`/`ClientId`/`Audience` if you do).

Then build and run:

```powershell
dotnet restore arc.sln
dotnet build arc.sln -c Debug
dotnet run --project arc.api
```

The API listens on `https://localhost:5001` by default (see `Kestrel:Endpoints:Https` in
`appsettings.json`).

### 4. Frontend

```powershell
cd arcportal
cp .env.example .env
npm install
npm start
```

The app runs at `http://localhost:3000` and calls the API at the URL configured in
`REACT_APP_LIMS_API_BASE_URL` (`.env`). The backend allows CORS for this origin via
`Urls:webclient` in `arc.api/appsettings.json`.

### 5. First-run setup wizard

The first time you open the app against a new, empty database, the login screen detects that the
system hasn't been configured yet and automatically opens a setup wizard. Complete its three
steps:

1. **Language** — the default language for the system.
2. **Settings** — whether the system supports multiple users and multiple laboratories.
3. **Users** — create the two initial accounts:
   - a **System Administrator**, for system-wide configuration, and
   - an **Organisation Administrator**, for day-to-day administration within the organisation.

Both accounts need a username and password; first and last names are optional. When you finish
the wizard the accounts are created and you can log in with either one.

The wizard only appears while the system is unconfigured — once setup is complete, the login
screen loads normally and further users are managed from within the app.

## Contributing

See [CONTRIBUTING.md](CONTRIBUTING.md) for how to report issues and submit changes.

## License

Licensed under the [Apache License 2.0](LICENSE).

Copyright 2026 Arcta Solution Ltd.

This project depends on third-party open-source packages; see
[THIRD-PARTY-NOTICES.md](THIRD-PARTY-NOTICES.md) for their licenses and
attribution.
