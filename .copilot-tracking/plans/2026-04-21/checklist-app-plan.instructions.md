---
applyTo: '.copilot-tracking/changes/2026-04-21/checklist-app-changes.md'
---
<!-- markdownlint-disable-file -->
# Implementation Plan: Shared Checklist App — New Blazor/.NET Aspire Solution

## Overview

Build a new C# / Blazor / .NET Aspire solution from scratch for collaborative, mobile-first real-time checklists targeting RV owners, using ASP.NET Core Web API, SignalR, EF Core, Azure SQL, and Azure App Service.

## Objectives

### User Requirements

* Build a new C# / Blazor / .NET Aspire app from scratch — Source: Research file `.copilot-tracking/research/2026-04-21/checklist-app-research.md`
* ASP.NET Core Web API backend with EF Core and Azure SQL Database — Source: Research file
* SignalR for real-time task-completion updates shared between users watching the same set — Source: Research file
* Blazor front-end — mobile-responsive, simple, fast — Source: Research file
* .NET Aspire orchestration host / AppHost project — Source: Research file
* Azure deployment via App Service with Bicep IaC — Source: Research file
* Azure DevOps YAML pipelines (CI/CD) — Source: Research file
* GitHub Actions YAML workflows (CI/CD) — Source: Research file
* SQL DACPAC project to deploy schema + sample data — Source: Research file

### Derived Objectives

* Two+ users sharing a CheckSet must see task-completion updates in real time without refresh — Derived from: SignalR group-based routing requirement
* App must work on mobile browsers (Bootstrap responsive, no native app) — Derived from: Primary use case description
* Old code is reference-only; new solution must not import or copy old code directly — Derived from: Explicit "do NOT copy old code" requirement
* Authentication via Microsoft Entra ID (same pattern as old code) — Derived from: Old code uses `IdentityDbContext<AppUser>` and Azure AD policies
* App Service Plan must be at least S1 tier to support WebSocket-based SignalR — Derived from: SignalR WebSocket requirement + Azure constraints

## Context Summary

### Project Files

* `old-source/CheckList.Core/Models/ProjectEntities.cs` — EF DbContext with 9 entity DbSets; reference for new AppDbContext
* `old-source/CheckList.Core/Models/Tables/` — All entity classes (CheckAction, CheckSet, CheckList, CheckCategory, TemplateSet, TemplateList, TemplateCategory, TemplateAction, AppUser, Customer)
* `old-source/CheckList.Core/Hub/ActionHub.cs` — Typed SignalR hub reference pattern; group join/leave, BroadcastMessage interface
* `old-source/CheckList.Core/API/*.cs` — Repository pattern reference; 8 controllers with CRUD operations
* `old-source/Database/LylesCheckList.sql` — RV sample data: Trip Prep, Hitching, Arrival lists
* `old-source/Database/ChangingLanesLists.sql` — Additional RV sample data
* `old-source/Database/GenerateDatabase.sql` — Schema reference for DACPAC
* `checklist-hve-core.sln` — Existing solution file; will need to be updated with new project references
* `.github/copilot-instructions.md` — Project conventions (file-scoped namespaces, PascalCase, Bootstrap themes, DI pattern)

### References

* `.copilot-tracking/research/2026-04-21/checklist-app-research.md` — Full research document with data model, API design, Blazor UI design, Bicep resource list, and CI/CD patterns
* https://github.com/lluppesms/dadabase.demo — Golden-pattern reference for pipelines and Bicep

### Standards References

* `.github/copilot-instructions.md` — File-scoped namespaces, Bootstrap themes, component CSS, DI pattern, XML doc comments

## Implementation Checklist

### [x] Implementation Phase 1: Solution Structure Initialization

<!-- parallelizable: false -->

* [x] Step 1.1: Create the `src/` directory and initialize Aspire solution projects including `CheckList.Tests`
  * Details: .copilot-tracking/details/2026-04-21/checklist-app-details.md § "Step 1.1"
* [x] Step 1.2: Create `CheckList.AppHost` project (Aspire orchestration)
  * Details: .copilot-tracking/details/2026-04-21/checklist-app-details.md § "Step 1.2"
* [x] Step 1.3: Create `CheckList.ServiceDefaults` project (Aspire shared defaults)
  * Details: .copilot-tracking/details/2026-04-21/checklist-app-details.md § "Step 1.3"
* [x] Step 1.4: Create `CheckList.Api` project — includes `Microsoft.Identity.Web` package
  * Details: .copilot-tracking/details/2026-04-21/checklist-app-details.md § "Step 1.4"
* [x] Step 1.5: Create `CheckList.Web` project — includes `Microsoft.Identity.Web` and `Microsoft.Identity.Web.UI` packages
  * Details: .copilot-tracking/details/2026-04-21/checklist-app-details.md § "Step 1.5"
* [x] Step 1.6: Create `CheckList.Shared` project (shared DTOs)
  * Details: .copilot-tracking/details/2026-04-21/checklist-app-details.md § "Step 1.6"
* [x] Step 1.7: Update `checklist-hve-core.sln` to include all new projects
  * Details: .copilot-tracking/details/2026-04-21/checklist-app-details.md § "Step 1.7"

### [ ] Implementation Phase 2A: Data Layer

<!-- parallelizable: true -->

* [ ] Step 2A.1: Create domain entities in `CheckList.Api/Data/Models/` — `CheckAction` includes `SetId` (denormalized)
  * Details: .copilot-tracking/details/2026-04-21/checklist-app-details.md § "Step 2A.1"
* [ ] Step 2A.2: Create `AppDbContext` in `CheckList.Api/Data/`
  * Details: .copilot-tracking/details/2026-04-21/checklist-app-details.md § "Step 2A.2"
* [ ] Step 2A.3: Create repository interfaces in `CheckList.Api/Repositories/Interfaces/`
  * Details: .copilot-tracking/details/2026-04-21/checklist-app-details.md § "Step 2A.3"
* [ ] Step 2A.4: Create repository implementations in `CheckList.Api/Repositories/Implementations/`
  * Details: .copilot-tracking/details/2026-04-21/checklist-app-details.md § "Step 2A.4"
* [ ] Step 2A.5: Create DTOs in `CheckList.Shared/DTOs/`
  * Details: .copilot-tracking/details/2026-04-21/checklist-app-details.md § "Step 2A.5"
* [ ] Step 2A.6: Create SQL DACPAC project — domain tables only; Identity tables via EF migrations
  * Details: .copilot-tracking/details/2026-04-21/checklist-app-details.md § "Step 2A.6"
* [ ] Step 2A.7: Validate Phase 2A — build data layer projects
  * Run: `dotnet build src/CheckList.Api/CheckList.Api.csproj`
  * Run: `dotnet build src/CheckList.Shared/CheckList.Shared.csproj`

### [ ] Implementation Phase 2B: Azure Infrastructure and CI/CD

<!-- parallelizable: true -->

* [ ] Step 2B.1: Create `infra/bicep/main.bicep` — includes `keyVaultRoleAssignment` module after `appService`
  * Details: .copilot-tracking/details/2026-04-21/checklist-app-details.md § "Step 2B.1"
* [ ] Step 2B.2: Create `infra/bicep/modules/appServicePlan.bicep`
  * Details: .copilot-tracking/details/2026-04-21/checklist-app-details.md § "Step 2B.2"
* [ ] Step 2B.3: Create `infra/bicep/modules/appService.bicep` — outputs `appServicePrincipalId`
  * Details: .copilot-tracking/details/2026-04-21/checklist-app-details.md § "Step 2B.3"
* [ ] Step 2B.4: Create `infra/bicep/modules/sqlServer.bicep` and `sqlDatabase.bicep`
  * Details: .copilot-tracking/details/2026-04-21/checklist-app-details.md § "Step 2B.4"
* [ ] Step 2B.5: Create `infra/bicep/modules/keyVault.bicep` (vault only) + `keyVaultRoleAssignment.bicep` (separate)
  * Details: .copilot-tracking/details/2026-04-21/checklist-app-details.md § "Step 2B.5"
* [ ] Step 2B.6: Create `infra/bicep/modules/appInsights.bicep` and `logAnalytics.bicep`
  * Details: .copilot-tracking/details/2026-04-21/checklist-app-details.md § "Step 2B.6"
* [ ] Step 2B.7: Create GitHub Actions workflows — build includes DACPAC + EF bundle; deploy includes schema step
  * Details: .copilot-tracking/details/2026-04-21/checklist-app-details.md § "Step 2B.7"
* [ ] Step 2B.8: Create Azure DevOps pipelines — deploy pipeline includes `DeploySchema` stage
  * Details: .copilot-tracking/details/2026-04-21/checklist-app-details.md § "Step 2B.8"

### [ ] Implementation Phase 3: API Layer

<!-- parallelizable: false -->
<!-- Depends on: Phase 2A (entities and repositories must exist) -->

* [ ] Step 3.1: Create SignalR hub and interface in `CheckList.Api/Hubs/`
  * Details: .copilot-tracking/details/2026-04-21/checklist-app-details.md § "Step 3.1"
* [ ] Step 3.2: Create minimal API endpoint groups — toggle-complete uses `action.SetId` directly
  * Details: .copilot-tracking/details/2026-04-21/checklist-app-details.md § "Step 3.2"
* [ ] Step 3.3: Configure `CheckList.Api/Program.cs` — all 8 repository registrations included
  * Details: .copilot-tracking/details/2026-04-21/checklist-app-details.md § "Step 3.3"
* [ ] Step 3.4: Validate Phase 3 — build API project
  * Run: `dotnet build src/CheckList.Api/CheckList.Api.csproj`

### [ ] Implementation Phase 4: Blazor Web App

<!-- parallelizable: false -->
<!-- Depends on: Phase 3 (API contracts and DTOs must exist) -->

* [ ] Step 4.1: Configure `CheckList.Web/Program.cs` (HttpClient, SignalR, auth)
  * Details: .copilot-tracking/details/2026-04-21/checklist-app-details.md § "Step 4.1"
* [ ] Step 4.2: Create `App.razor`, `Routes.razor`, `MainLayout.razor`
  * Details: .copilot-tracking/details/2026-04-21/checklist-app-details.md § "Step 4.2"
* [ ] Step 4.3: Create Blazor pages: Home, Sets, SetDetail
  * Details: .copilot-tracking/details/2026-04-21/checklist-app-details.md § "Step 4.3"
* [ ] Step 4.4: Create `ActiveList.razor` — SignalR hub URL resolved from API service discovery config
  * Details: .copilot-tracking/details/2026-04-21/checklist-app-details.md § "Step 4.4"
* [ ] Step 4.5: Create Templates and Admin pages
  * Details: .copilot-tracking/details/2026-04-21/checklist-app-details.md § "Step 4.5"
* [ ] Step 4.6: Create shared Blazor components (`NavMenu`, `CheckActionItem`, `SetCard`)
  * Details: .copilot-tracking/details/2026-04-21/checklist-app-details.md § "Step 4.6"
* [ ] Step 4.7: Create component-scoped CSS files (`.razor.css`) for all interactive components
  * Details: .copilot-tracking/details/2026-04-21/checklist-app-details.md § "Step 4.7"
* [ ] Step 4.8: Validate Phase 4 — build Blazor project
  * Run: `dotnet build src/CheckList.Web/CheckList.Web.csproj`

### [ ] Implementation Phase 5: Aspire Wiring

<!-- parallelizable: false -->
<!-- Depends on: Phases 3 and 4 (all named projects must exist) -->

* [ ] Step 5.1: Configure `CheckList.AppHost/Program.cs` — wire up SQL, API, Web projects
  * Details: .copilot-tracking/details/2026-04-21/checklist-app-details.md § "Step 5.1"
* [ ] Step 5.2: Configure `CheckList.ServiceDefaults/Extensions.cs` — health checks, telemetry, resilience
  * Details: .copilot-tracking/details/2026-04-21/checklist-app-details.md § "Step 5.2"
* [ ] Step 5.3: Validate Phase 5 — build AppHost project
  * Run: `dotnet build src/CheckList.AppHost/CheckList.AppHost.csproj`

### [ ] Implementation Phase 6: Validation

<!-- parallelizable: false -->
<!-- Depends on: All previous phases -->

* [ ] Step 6.1: Run full solution build
  * Run: `dotnet build checklist-hve-core.sln`
* [ ] Step 6.2: Run any unit tests added during implementation
  * Run: `dotnet test checklist-hve-core.sln`
* [ ] Step 6.3: Validate Bicep templates
  * Run: `az bicep build --file infra/bicep/main.bicep` (if az CLI available)
* [ ] Step 6.4: Fix minor validation issues inline
* [ ] Step 6.5: Report blocking issues requiring additional research

## Planning Log

See `.copilot-tracking/plans/logs/2026-04-21/checklist-app-log.md` for discrepancy tracking, implementation paths considered, and suggested follow-on work.

## Dependencies

* .NET 9 SDK (Aspire GA requires .NET 9)
* `dotnet workload install aspire`
* Azure SQL Database instance (or SQL Server local for dev)
* Azure CLI (for Bicep validation)
* SqlPackage CLI (for DACPAC deployment)
* NuGet packages: `Microsoft.AspNetCore.SignalR`, `Microsoft.EntityFrameworkCore.SqlServer`, `Microsoft.AspNetCore.Identity.EntityFrameworkCore`, `Microsoft.Azure.AppConfiguration.AspNetCore`, `Microsoft.ApplicationInsights.AspNetCore`

## Success Criteria

* `dotnet build checklist-hve-core.sln` succeeds with 0 errors — Traces to: All user requirements
* SignalR hub broadcasts `ReceiveActionUpdate` to all clients in `set-{setId}` group — Traces to: Real-time sync requirement
* ActiveList Blazor page subscribes to hub on load, updates without page refresh — Traces to: Mobile real-time UX requirement
* Bicep deploys App Service Plan (S1+), App Service, Azure SQL, Key Vault, App Insights, Log Analytics — Traces to: Azure deployment requirement
* CI/CD pipeline runs build + test + DACPAC + deploy for both ADO and GitHub Actions — Traces to: CI/CD requirement
* SQL DACPAC project deploys schema and RV sample data (Trip Prep, Hitching, Arrival) — Traces to: DACPAC requirement
* All Blazor pages render correctly on mobile viewport (375px+) — Traces to: Mobile-first requirement
