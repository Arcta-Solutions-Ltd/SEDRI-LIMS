# YUNIQL Translated Workspace Builder

This directory contains a PowerShell script that composes a YUNIQL workspace from a base set of migrations and an language overlay. If you don't require a custom language then you can run the scripts found in the base directory without this one.

- Script: [yuniqlWorkspaces/make-translated-base.ps1](yuniqlWorkspaces/make-translated-base.ps1)
- Inputs: Base migrations, language-specific migrations
- Output: A new workspace folder with versioned directories suffixed to indicate content provenance

## Prerequisites
- PowerShell (Windows PowerShell 5.1 or PowerShell 7+)
- This directory must contain:
  - [yuniqlWorkspaces/base](yuniqlWorkspaces/base)
  - [yuniqlWorkspaces/base-languages](yuniqlWorkspaces/base-languages)
- The output directory you choose must NOT already exist; the script aborts to prevent accidental overwrite.

## Parameters
- `Language` (string, default: `english`)
  - Case-insensitive. When `english`, no language overlay is applied.
  - For non-English values, a matching subfolder must exist at `LanguagesPath/<language>`.
- `OutputPath` (string, default: `./tmp`)
  - Destination folder for the generated workspace. Must not already exist.
- `BasePath` (string, default: `./base/`)
  - Source folder for the base migrations.
- `LanguagesPath` (string, default: `./base-languages`)
  - Root folder containing language-specific overlays.

## Behavior
- If `OutputPath` already exists, the script exits with an error.
- Copies all content from `BasePath` into `OutputPath`.
- Renames version directories under `OutputPath` that match `v*` by appending a suffix:
  - Base copy: `_020-base`
  - Language overlay (when non-English language applied): `_040-language_<language>`
- When `Language` is not `english` and the corresponding folder exists in `LanguagesPath`, the overlay is copied into `OutputPath` using the same version-folder suffixing pattern.
- Final message: "Successfully created YUNIQL workspace in <OutputPath>".

## Usage
Run commands from the `yuniqlWorkspaces` directory (or adjust paths accordingly):

```powershell
# Base only (English)
pwsh -NoProfile -File .\make-translated-base.ps1

# Base only with custom output path
pwsh -NoProfile -File .\make-translated-base.ps1 -OutputPath .\workspace-out

# Base + non-English language overlay (language folder must exist)
pwsh -NoProfile -File .\make-translated-base.ps1 -Language spanish -OutputPath .\workspace-es

# Using explicit source roots
pwsh -NoProfile -File .\make-translated-base.ps1 -BasePath .\base -LanguagesPath .\base-languages -Language russian -OutputPath .\workspace-ru
```

## Validation
After the script completes, inspect the output directory:
- Versioned folders (matching `v*`) copied from base should end with `_020-base`.
- If a non-English language was specified, corresponding versioned folders added by the overlay should end with `_040-language_<language>`.

## Troubleshooting
- "Directory \"<OutputPath>\" is not empty"
  - Choose a new `OutputPath` or delete the existing folder before rerunning.
- "Language is not present in languages directory"
  - Verify the language folder exists at `<LanguagesPath>/<language>` (e.g., `base-languages/russian`).
- Permission or access errors
  - Run your terminal with sufficient permissions or pick a writable `OutputPath`.

## Notes
- Paths are relative to the current working directory; adjust if running from elsewhere.
- Use quotes for arguments containing spaces.
- The script currently supports composing only base + base-language overlays; client and client-language overlays have been removed.

## Migration ID conventions (base workspace)

- **System seed ids** in yuniql scripts must be **below 10000**; ids ≥ 10000 are reserved for user-created rows after `ALTER SEQUENCE … RESTART WITH 10000` in base seed scripts.
- Assign explicit ids when inserting system seed data; do not omit `id` and rely on the sequence after the 10000 restart.
- When adding seed data or fixes, create a **new numbered script** in the current version folder (for example `v3.00.05/008-….sql`); **do not amend** scripts in already-released version folders.
- Remediation for seed rows that incorrectly received ids ≥ 10000: `base/v3.00.05/008-remap-system-seed-ids-below-10000.sql`.
- Cypress test SQL (`arcportal/cypress/data/`) is excluded — it simulates user data.

_Last updated: 2026-01-07_
