<!-- markdownlint-disable-file -->
# Release Changes: Shared Checklist App — New Blazor/.NET Aspire Solution

**Related Plan**: checklist-app-plan.instructions.md
**Implementation Date**: 2026-04-21

## Summary

New C# / Blazor / .NET Aspire solution built from scratch for collaborative, mobile-first real-time checklists. Includes ASP.NET Core Web API backend with EF Core and Azure SQL, SignalR for real-time updates, Blazor front-end, .NET Aspire orchestration, Azure Bicep IaC, and GitHub Actions + Azure DevOps CI/CD pipelines.

## Changes

### Added

<!-- Phase 1 -->
* `src/CheckList.AppHost/` - Aspire AppHost project (orchestration host)
* `src/CheckList.AppHost/CheckList.AppHost.csproj` - AppHost project file with Aspire.Hosting.AppHost, SqlServer packages + project refs to Api and Web
* `src/CheckList.AppHost/Program.cs` - Aspire orchestration wiring: SQL, api, web with WaitFor ordering
* `src/CheckList.ServiceDefaults/` - Aspire ServiceDefaults shared defaults project
* `src/CheckList.ServiceDefaults/CheckList.ServiceDefaults.csproj` - ServiceDefaults project with OTel, resilience, service discovery packages
* `src/CheckList.Api/` - Web API project (scaffolded with minimal APIs)
* `src/CheckList.Api/CheckList.Api.csproj` - Api project file with EF Core, Identity, SignalR, Swagger, Microsoft.Identity.Web packages + project refs
* `src/CheckList.Web/` - Blazor Web App project (server-side interactive)
* `src/CheckList.Web/CheckList.Web.csproj` - Web project file with SignalR.Client, Microsoft.Identity.Web, Microsoft.Identity.Web.UI + project refs
* `src/CheckList.Shared/` - Shared class library for DTOs
* `src/CheckList.Shared/CheckList.Shared.csproj` - Shared project file (minimal, no dependencies)
* `src/CheckList.Tests/` - MSTest project for unit tests
* `src/CheckList.Tests/CheckList.Tests.csproj` - Tests project file
* `infra/bicep/modules/` - Directory for Bicep module files
* `Database/Schema/Tables/` - Directory for DACPAC table schema files
* `Database/SeedData/` - Directory for DACPAC seed data files
* `Docs/` - Documentation directory

### Modified

* `checklist-hve-core.sln` - Added all 6 new projects (AppHost, ServiceDefaults, Api, Web, Shared, Tests)

<!-- Phase 2A -->
* `src/CheckList.Api/Data/Models/AppUser.cs` - AppUser entity extending IdentityUser with FirstName, LastName
* `src/CheckList.Api/Data/Models/Customer.cs` - Customer entity linked to AppUser
* `src/CheckList.Api/Data/Models/TemplateSet.cs` - Template set with collection of TemplateLists
* `src/CheckList.Api/Data/Models/TemplateList.cs` - Template list entity
* `src/CheckList.Api/Data/Models/TemplateCategory.cs` - Template category entity
* `src/CheckList.Api/Data/Models/TemplateAction.cs` - Template action entity
* `src/CheckList.Api/Data/Models/CheckSet.cs` - Check set entity with optional TemplateSetId
* `src/CheckList.Api/Data/Models/CheckList.cs` - Check list entity
* `src/CheckList.Api/Data/Models/CheckCategory.cs` - Check category entity
* `src/CheckList.Api/Data/Models/CheckAction.cs` - Check action entity with denormalized SetId, CompleteInd, CompletedBy, CompletedAt
* `src/CheckList.Api/Data/AppDbContext.cs` - EF Core DbContext extending IdentityDbContext with all 9 entity DbSets
* `src/CheckList.Api/GlobalUsings.cs` - Global using alias for CheckListModel to resolve namespace ambiguity
* `src/CheckList.Api/Repositories/Interfaces/ICheckSetRepository.cs` - CRUD interface for CheckSets
* `src/CheckList.Api/Repositories/Interfaces/ICheckListRepository.cs` - CRUD interface for CheckLists
* `src/CheckList.Api/Repositories/Interfaces/ICheckCategoryRepository.cs` - CRUD interface for CheckCategories
* `src/CheckList.Api/Repositories/Interfaces/ICheckActionRepository.cs` - CRUD + ToggleCompleteAsync interface
* `src/CheckList.Api/Repositories/Interfaces/ITemplateSetRepository.cs` - Read-only interface for TemplateSets
* `src/CheckList.Api/Repositories/Interfaces/ITemplateListRepository.cs` - Read-only interface for TemplateLists
* `src/CheckList.Api/Repositories/Interfaces/ITemplateCategoryRepository.cs` - Read-only interface
* `src/CheckList.Api/Repositories/Interfaces/ITemplateActionRepository.cs` - Read-only interface
* `src/CheckList.Api/Repositories/Implementations/CheckSetRepository.cs` - CheckSet repository with owner-based query
* `src/CheckList.Api/Repositories/Implementations/CheckListRepository.cs` - CheckList repository
* `src/CheckList.Api/Repositories/Implementations/CheckCategoryRepository.cs` - CheckCategory repository
* `src/CheckList.Api/Repositories/Implementations/CheckActionRepository.cs` - CheckAction repository with ToggleCompleteAsync
* `src/CheckList.Api/Repositories/Implementations/TemplateSetRepository.cs` - Template set read-only repo with deep include
* `src/CheckList.Api/Repositories/Implementations/TemplateListRepository.cs` - Template list read-only repo
* `src/CheckList.Api/Repositories/Implementations/TemplateCategoryRepository.cs` - Template category read-only repo
* `src/CheckList.Api/Repositories/Implementations/TemplateActionRepository.cs` - Template action read-only repo
* `src/CheckList.Shared/DTOs/ActionUpdateDto.cs` - SignalR broadcast payload record
* `src/CheckList.Shared/DTOs/CheckSetDto.cs` - CheckSet response DTO record
* `src/CheckList.Shared/DTOs/CheckListDto.cs` - CheckList response DTO record
* `src/CheckList.Shared/DTOs/CheckCategoryDto.cs` - CheckCategory response DTO with nested actions
* `src/CheckList.Shared/DTOs/CheckActionDto.cs` - CheckAction response DTO record
* `src/CheckList.Shared/DTOs/TemplateSetDto.cs` - TemplateSet response DTO record
* `src/CheckList.Shared/DTOs/CreateCheckSetRequest.cs` - Request DTO for creating a CheckSet from template
* `Database/CheckList.Database.sqlproj` - SQL DACPAC project using Microsoft.Build.Sql SDK
* `Database/Schema/Tables/TemplateSet.sql` - TemplateSets table DDL
* `Database/Schema/Tables/TemplateList.sql` - TemplateLists table DDL
* `Database/Schema/Tables/TemplateCategory.sql` - TemplateCategories table DDL
* `Database/Schema/Tables/TemplateAction.sql` - TemplateActions table DDL
* `Database/Schema/Tables/CheckSet.sql` - CheckSets table DDL
* `Database/Schema/Tables/CheckList.sql` - CheckLists table DDL
* `Database/Schema/Tables/CheckCategory.sql` - CheckCategories table DDL
* `Database/Schema/Tables/CheckAction.sql` - CheckActions table DDL with SetId column
* `Database/Schema/Tables/Customer.sql` - Customers table DDL
* `Database/SeedData/PostDeployment.sql` - Post-deployment script referencing seed files
* `Database/SeedData/RVLists.sql` - RV Trip Prep, Hitching, Arrival seed data (65 actions)
* `Database/SeedData/ChangingLanes.sql` - Changing Lanes seed data (83 actions, 4 lists)

<!-- Phase 2B -->
* `infra/bicep/main.bicep` - Main Bicep template with all 8 modules (logAnalytics, appInsights, keyVault, sqlServer, sqlDatabase, appServicePlan, appService, keyVaultRoleAssignment)
* `infra/bicep/modules/appServicePlan.bicep` - App Service Plan module (S1 Standard, WebSocket-capable)
* `infra/bicep/modules/appService.bicep` - App Service module with SystemAssigned identity, webSocketsEnabled, alwaysOn, outputs appServicePrincipalId
* `infra/bicep/modules/sqlServer.bicep` - SQL Server module with Azure services firewall rule
* `infra/bicep/modules/sqlDatabase.bicep` - SQL Database module (S0 Standard tier)
* `infra/bicep/modules/keyVault.bicep` - Key Vault module with RBAC authorization and soft delete
* `infra/bicep/modules/keyVaultRoleAssignment.bicep` - Separate RBAC module granting Key Vault Secrets User to App Service identity
* `infra/bicep/modules/logAnalytics.bicep` - Log Analytics Workspace module (30-day retention)
* `infra/bicep/modules/appInsights.bicep` - Application Insights module linked to Log Analytics, outputs connection string
* `.github/workflows/build-and-test.yml` - GitHub Actions CI workflow: restore, build, test with Aspire workload
* `.github/workflows/deploy.yml` - GitHub Actions deploy workflow: OIDC auth, Bicep infra, EF migrations, API publish/deploy
* `.azuredevops/pipelines/build-and-test.yml` - Azure DevOps build pipeline: restore, build, test, EF bundle artifact
* `.azuredevops/pipelines/deploy.yml` - Azure DevOps deploy pipeline: DeployInfra, DeploySchema, DeployApp stages

<!-- Phase 3 -->
* `src/CheckList.Api/Hubs/ICheckListHubClient.cs` - Typed SignalR client interface with ReceiveActionUpdate method
* `src/CheckList.Api/Hubs/CheckListHub.cs` - [Authorize] SignalR hub with JoinSet/LeaveSet group management
* `src/CheckList.Api/Endpoints/CheckSetsEndpoints.cs` - CRUD endpoint group for check sets
* `src/CheckList.Api/Endpoints/CheckListsEndpoints.cs` - GET endpoint group for check lists by set
* `src/CheckList.Api/Endpoints/CheckActionsEndpoints.cs` - GET by category + toggle-complete endpoint with SignalR broadcast
* `src/CheckList.Api/Endpoints/TemplateSetsEndpoints.cs` - Read-only template sets endpoint group

<!-- Phase 4 -->
* `src/CheckList.Web/Services/CheckListApiClient.cs` - Typed HTTP client wrapping all API calls with service discovery
* `src/CheckList.Web/Components/Pages/Sets.razor` - Check sets list page with create-from-template modal
* `src/CheckList.Web/Components/Pages/SetDetail.razor` - Set detail page listing check lists
* `src/CheckList.Web/Components/Pages/ActiveList.razor` - Active list page with real-time SignalR updates (IAsyncDisposable)
* `src/CheckList.Web/Components/Pages/ActiveList.razor.css` - Scoped CSS for category sections using Bootstrap variables
* `src/CheckList.Web/Components/Pages/Templates.razor` - Templates listing page
* `src/CheckList.Web/Components/Pages/Admin.razor` - Admin placeholder page
* `src/CheckList.Web/Components/Shared/CheckActionItem.razor` - Checkbox component with completion strikethrough
* `src/CheckList.Web/Components/Shared/CheckActionItem.razor.css` - Scoped CSS for action rows
* `src/CheckList.Web/Components/Shared/SetCard.razor` - Bootstrap card component for check sets
* `src/CheckList.Web/Components/Shared/SetCard.razor.css` - Scoped CSS for set cards

<!-- Phase 5 -->
* `src/CheckList.Web/GlobalUsings.cs` - Global using for CheckList.ServiceDefaults namespace

<!-- Phase 6 -->
*(No file changes — solution built clean on first run, no fix-ups required)*

### Modified

* `src/CheckList.Api/Program.cs` - Replaced entirely: auth, EF Core + Identity, 8 repo registrations, SignalR, CORS (AllowCredentials), Swagger, endpoint group mapping, hub at /hubs/checklist, MapDefaultEndpoints
* `src/CheckList.Api/appsettings.json` - Added AzureAd, ConnectionStrings, and WebAppUrl sections
* `src/CheckList.Web/Program.cs` - Replaced: ServiceDefaults, Entra ID auth, HttpClient with service discovery, RazorComponents, MapControllers for Identity UI
* `src/CheckList.Web/appsettings.json` - Added AzureAd section
* `src/CheckList.Web/Components/_Imports.razor` - Added global usings for DTOs, services, shared components, auth
* `src/CheckList.Web/Components/App.razor` - Replaced: CDN Bootstrap 5.3, InteractiveServer render mode
* `src/CheckList.Web/Components/Routes.razor` - Replaced: AuthorizeRouteView with NotFound handler
* `src/CheckList.Web/Components/Layout/MainLayout.razor` - Replaced: flex layout with NavMenu header
* `src/CheckList.Web/Components/Layout/MainLayout.razor.css` - Replaced: Bootstrap variable-based styles
* `src/CheckList.Web/Components/Layout/NavMenu.razor` - Replaced: Bootstrap navbar with sign-in/sign-out
* `src/CheckList.Web/Components/Layout/NavMenu.razor.css` - Replaced: scoped minimal styles
* `src/CheckList.Web/Components/Pages/Home.razor` - Replaced: welcome page with call-to-action
* `src/CheckList.ServiceDefaults/Extensions.cs` - Replaced: full OTel, health checks, service discovery, resilience implementation
* `src/CheckList.AppHost/CheckList.AppHost.csproj` - Updated: migrated to Aspire.AppHost.Sdk 9.5.2 (deprecation fix)
* `src/CheckList.Api/GlobalUsings.cs` - Added global using for CheckList.ServiceDefaults namespace

## Additional or Deviating Changes

* `src/CheckList.Api/Program.cs` - Fixed pre-existing scaffold defect: replaced AddOpenApi/MapOpenApi (missing package) with Swashbuckle calls matching the csproj
  * Reason: Scaffolded template used .NET 9 built-in OpenAPI that was not in the plan's package list; fixed to match plan
* `src/CheckList.Api/GlobalUsings.cs` - Added global type alias `CheckListModel` to resolve namespace/class name ambiguity for `CheckList.Api.Data.Models.CheckList`
  * Reason: `CheckList` class name conflicts with `CheckList.Api` root namespace in fully-qualified references

## Release Summary

All 6 implementation phases completed successfully. Full solution builds 0 errors in Release configuration; 1 test passes; Bicep templates validate cleanly.

**Total files affected: 96**

| Category | Count |
|---|---|
| Files added | 79 |
| Files modified/replaced | 16 |
| Files deleted | 1 (AppHost.cs scaffold stub) |

**Projects created (6 new):**
- `src/CheckList.AppHost` — .NET Aspire orchestration host
- `src/CheckList.ServiceDefaults` — Shared OTel, health, resilience, service discovery defaults
- `src/CheckList.Api` — ASP.NET Core Web API with EF Core, SignalR, Identity, Swagger
- `src/CheckList.Web` — Blazor Server interactive web app with real-time SignalR client
- `src/CheckList.Shared` — Shared DTO record library
- `src/CheckList.Tests` — MSTest unit test project (placeholder)

**Infrastructure:**
- `infra/bicep/` — 9 Bicep files (main + 8 modules) targeting Azure App Service S1, Azure SQL S0, Key Vault, App Insights
- `.github/workflows/` — 2 GitHub Actions workflows (CI + Deploy, OIDC auth)
- `.azuredevops/pipelines/` — 2 Azure DevOps pipelines (CI + Deploy, 3-stage deploy)

**Database:**
- `Database/CheckList.Database.sqlproj` — DACPAC project, Microsoft.Build.Sql SDK
- `Database/Schema/Tables/` — 9 SQL table DDL files
- `Database/SeedData/` — 148 seed actions across 7 lists in 2 seed files

**Known follow-on work items:**
- WI-01: Implement `GetCategoriesAsync` full API endpoint (currently stub returning `[]` in `CheckListApiClient`)
- WI-02: Admin page role guard — wire `[Authorize(Roles = "Admin")]` + Entra ID role claim config
- WI-03: DACPAC CI/CD — add DACPAC deployment steps once `Microsoft.Build.Sql` available on ubuntu agents
- WI-04: Populate AzureAd config values in both `appsettings.json` files with real tenant/client IDs
- WI-05: Add meaningful unit tests to `CheckList.Tests` covering repository logic and endpoint handlers
- WI-06: Resolve `CS0649` on `ActiveList._listName` once list name retrieval is wired through API
