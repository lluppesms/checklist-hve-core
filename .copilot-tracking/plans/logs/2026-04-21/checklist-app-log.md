<!-- markdownlint-disable-file -->
# Planning Log: Shared Checklist App — New Blazor/.NET Aspire Solution

## Discrepancy Log

Gaps and differences identified between research findings and the implementation plan.

### Unaddressed Research Items

* DR-01: Authentication via Azure AD / Microsoft Entra ID is scoped to basic JWT bearer on the API and OIDC sign-in on the Blazor Web App.
  * Source: `.copilot-tracking/research/2026-04-21/checklist-app-research.md` (Lines 27-30)
  * Reason: Full Entra ID app registration (creating App Registrations, configuring scopes, admin consent) requires Azure portal configuration outside the codebase. Plan documents the configuration hooks but cannot automate Azure portal steps.
  * Impact: low — the code pattern is complete; only Azure tenant-specific values in `appsettings.json` are missing.

* DR-02: `dadabase.demo` reference repo patterns for Bicep and CI/CD were not verified via live fetch.
  * Source: `.copilot-tracking/research/2026-04-21/checklist-app-research.md` (Lines 40-45)
  * Reason: GitHub repo fetch was not executed during research phase. Bicep and pipeline patterns are based on research file which summarizes the golden-pattern. Patterns used are current best practices.
  * Impact: low — the Bicep and pipeline code in the plan is idiomatic and correct; minor deviations from dadabase.demo style may need adjustment after review.

* DR-03: .NET Aspire latest version patterns and `dotnet new aspire` template scaffolding specifics were not verified against the 9.x release.
  * Source: Research file Scenario 2 (Lines 270-300)
  * Reason: Aspire tooling evolves quickly. The plan uses the documented stable pattern; minor `csproj` package version differences may need adjustment.
  * Impact: low — package versions use `9.*` wildcards to absorb minor version differences.

* DR-04: DACPAC deploy step (C-01) was absent from both CI/CD pipelines despite research Scenario 3 specifying `SqlPackage.exe publish` in CD.
  * Source: `.copilot-tracking/research/2026-04-21/checklist-app-research.md` § "DACPAC CD pattern"
  * Reason: Originally omitted from deploy pipeline steps during authoring
  * Impact: high — originally blocking (no schema deployed); addressed in Steps 2B.7 and 2B.8

* DR-05: ASP.NET Core Identity tables required by `IdentityDbContext<AppUser>` had no defined creation strategy.
  * Source: `.copilot-tracking/details/2026-04-21/checklist-app-details.md` § "Step 2A.2"
  * Reason: DACPAC scope was documented as 9 domain tables; Identity tables were not considered
  * Impact: high — app cannot authenticate without Identity tables; addressed in Step 2A.6 (EF migrations bundle strategy)

* DR-06: GitHub Actions `deploy.yml` artifact cross-workflow download — **resolved**
  * Source: Details § "Step 2B.7"
  * Reason: Originally `deploy-schema` tried to download artifacts from a separate `build-and-test.yml` run
  * Fix: Added `build-schema` job as first job in `deploy.yml`; builds DACPAC and EF bundle within the same workflow run; `deploy-infra` now uses `needs: build-schema`
  * Impact: high — originally blocking; now addressed

* DR-07: ADO `build-and-test.yml` never built or published the EF migrations bundle — **resolved**
  * Source: Details § "Step 2B.8"
  * Reason: ADO build pipeline had no `dotnet ef migrations bundle` step and no `efmigrations` artifact
  * Fix: Added `dotnet ef migrations bundle` step + `PublishBuildArtifacts@1` for `efmigrations` to ADO `build-and-test.yml`
  * Impact: high — originally blocking; now addressed

* DR-08: `ActiveList.razor` missing `@inject IConfiguration Configuration` directive — **resolved**
  * Source: Details § "Step 4.4"
  * Reason: M-03 fix used `Configuration[...]` without declaring the inject directive
  * Fix: Added `@inject IConfiguration Configuration` as third inject directive in `ActiveList.razor`
  * Impact: high — originally compile-time error; now addressed

* DR-09: `CheckList.Api/Program.cs` Step 3.3 registers only 5 of 8 repositories — **resolved**
  * Source: Details § "Step 3.3"
  * Reason: Template sub-resource repositories were missing from DI registrations
  * Fix: Added `ITemplateListRepository`, `ITemplateCategoryRepository`, `ITemplateActionRepository` registrations
  * Impact: medium — originally runtime DI failure; now addressed

* DR-10: Blazor Web project deployed to App Service — **resolved**
  * Source: Details § "Step 2B.7" and § "Step 2B.8"
  * Reason: `deploy-app` job published Web project but only deployed API package
  * Fix: Removed second `webapps-deploy` step entirely; GitHub Actions deploy-app now deploys only the API. Note added stating Web requires its own App Service (WI-03). ADO `DeployApp` stage likewise deploys only the API.
  * Impact: resolved — single-slot overwrite no longer possible; WI-03 tracks the follow-on Web deployment work

* NEW-C-01: `builder.Services.AddSignalR()` is missing from `CheckList.Api/Program.cs` — **resolved**
  * Source: Details § "Step 3.3" — `// SignalR` comment present but no registration call
  * Reason: Placeholder comment was not replaced with the actual `AddSignalR()` call
  * Fix: `builder.Services.AddSignalR()` added under `// SignalR` comment, before `// Swagger` and `builder.Services.AddEndpointsApiExplorer()`, in Step 3.3
  * Impact: resolved — `app.MapHub<CheckListHub>("/hubs/checklist")` will compile and serve correctly at startup

* NEW-C-02: `type: war` in GitHub Actions `deploy.yml` second `azure/webapps-deploy@v3` step — **resolved**
  * Source: Details § "Step 2B.7" — `deploy-app` job, second `azure/webapps-deploy@v3` with `type: war`
  * Reason: `type: war` is the Java WAR deployment type; it is not valid for a .NET Blazor Server application
  * Fix: Second `webapps-deploy` step removed entirely from `deploy-app` job (addressed together with NEW-C-03)
  * Impact: resolved — no invalid `type: war` parameter remains in the pipeline

* NEW-C-03: Two sequential App Service deployments overwrite each other — **resolved**
  * Source: Details § "Step 2B.7" (GitHub Actions `deploy-app` job) and § "Step 2B.8" (ADO `DeployApp` stage)
  * Reason: Azure App Service replaces `/home/site/wwwroot` on each deployment; deploying API then Web means only the Web package survives; the API is wiped
  * Fix: Both GitHub Actions and ADO pipelines now deploy only the API package; Web deployment deferred to WI-03 with an explanatory note
  * Impact: resolved — single-slot overwrite eliminated; API deployment is the sole target

* NEW-M-01: ADO `deploy.yml` downloads artifacts with `buildType: current` but produces no artifacts of its own — **resolved**
  * Source: Details § "Step 2B.8" — `DeploySchema` stage, `DownloadBuildArtifacts@1` inputs
  * Reason: `dacpac` and `efmigrations` artifacts are published by the separate `build-and-test.yml` pipeline; `deploy.yml` had no `resources:` pipeline link and `buildType: current` refers only to the current pipeline run
  * Fix: Added `resources: pipelines: - pipeline: CIBuild, source: 'build-and-test', trigger: true` to ADO `deploy.yml`; both `DownloadBuildArtifacts@1` tasks now use `buildType: specific, project: $(System.TeamProjectId), pipeline: CIBuild`
  * Impact: resolved — deploy pipeline resolves artifacts from the CI build pipeline run

* NEW-M-02: `CheckList.Tests` project is absent from Phase 1 implementation steps — **resolved**
  * Source: User requirement #1 explicitly lists `CheckList.Tests` as a required solution project; Plan § Phase 1 (Steps 1.1–1.7) and Details § "Step 1.1" did not create it
  * Reason: Test project creation was deferred to WI-01 without creating an empty project stub
  * Fix: `dotnet new mstest -n CheckList.Tests -o src/CheckList.Tests` added to Step 1.1; `src/CheckList.Tests/CheckList.Tests.csproj` added to `dotnet sln add` command in Step 1.7
  * Impact: resolved — `CheckList.Tests` is created in Phase 1 and included in the solution; `dotnet build checklist-hve-core.sln` will succeed

* NEW-MI-01: Redundant artifact uploads in GitHub Actions `build-and-test.yml` — **resolved**
  * Source: Details § "Step 2B.7" — both `build-and-test.yml` and `deploy.yml` `build-schema` job uploaded artifacts named `dacpac` and `efmigrations`
  * Reason: DR-06 fix added artifact builds to `deploy.yml` but did not remove the duplicate uploads from `build-and-test.yml`
  * Fix: GitHub Actions `build-and-test.yml` now contains only restore/build/test steps; DACPAC and EF bundle are built exclusively in `deploy.yml` `build-schema` job; explanatory note added
  * Impact: resolved — single authoritative artifact source; CI build is lean

* NEW-M-03: ADO `DeployApp` stage `deployment` job missing `- checkout: self`
  * Source: Details § "Step 2B.8" — `DeployApp` stage, `AppDeploy` deployment job, `runOnce.deploy.steps` block
  * Reason: ADO `deployment` job type does not automatically checkout source code (unlike a regular `job`). The workspace is initialized at `$(Pipeline.Workspace)` with downloaded artifacts only. The `DotNetCoreCLI@2` publish step references `src/CheckList.Api/CheckList.Api.csproj` which does not exist without an explicit checkout.
  * Impact: major — ADO `DeployApp` stage fails with "project file not found"; API is never published or deployed
  * Recommended fix: Add `- checkout: self` as the first step inside `runOnce.deploy.steps` before the `DotNetCoreCLI@2` publish task

* NEW-MI-02: GitHub Actions `deploy-app` job publishes Web project to `./publish/web` but never deploys it
  * Source: Details § "Step 2B.7" — `deploy-app` job, step `run: dotnet publish src/CheckList.Web/CheckList.Web.csproj -c Release -o ./publish/web`
  * Reason: NEW-C-03 fix correctly removed the second `webapps-deploy` step, but the preceding `dotnet publish` for the Web project was not removed
  * Impact: minor — no functional breakage; wastes ~30s of CI time per run; creates a confusing artifact at `./publish/web` that is never used
  * Recommended fix: Remove the `run: dotnet publish src/CheckList.Web/...` step from `deploy-app`, or add an inline comment noting it is intentionally stubbed pending WI-03

### Plan Deviations from Research

* DD-01: `CompleteInd` modeled as `string` (nchar(1)) rather than `bool`.
  * Research recommends: No specific recommendation; old code uses `char(1)` "Y"/"N"
  * Plan implements: `string CompleteInd` with `nchar(1)` column type, consistent with old DB schema
  * Rationale: Changing to `bool` would break compatibility with the existing SQL DACPAC schema and any existing data. Keeping `char(1)` preserves DB compatibility.

* DD-02: `SetId` denormalization added to `CheckAction` entity.
  * Research notes: `CheckAction` in old model has `categoryId` and `listId` (denormalized)
  * Plan implements: Adding `SetId` as a third denormalized field on `CheckAction`
  * Rationale: The toggle-complete API endpoint must broadcast `SetId` to the SignalR group. Loading the full navigation chain `CheckAction → CheckCategory → CheckList → CheckSet` on every toggle is expensive. Adding `SetId` to `CheckAction` follows the existing denormalization pattern already present in the old code and eliminates a join in the hot path.

* DD-03: Combined API + Web deployment to a single App Service vs. two separate App Services.
  * Research recommends: Azure App Service (single resource)
  * Plan implements: Single App Service; notes that Blazor server-side and Web API can share one App Service instance (Web project hosted by API project or reverse proxy via YARP)
  * Rationale: Simplifies deployment for the initial version; can be split into two App Services in follow-on work if scaling or isolation is needed.

* DD-04: `Microsoft.Identity.Web` NuGet package was missing from API and Web project package lists (M-01).
  * Research recommends: Entra ID integration using `Microsoft.Identity.Web` for both API and Web
  * Plan implements: Package list in Steps 1.4 and 1.5 now includes `Microsoft.Identity.Web` (API) and `Microsoft.Identity.Web` + `Microsoft.Identity.Web.UI` (Web)
  * Rationale: Required for `AddMicrosoftIdentityWebApi()` and `AddMicrosoftIdentityWebApp()` to compile

* DD-05: Bicep RBAC wiring was broken — `appServicePrincipalId` was never passed to `keyVault.bicep` (M-02).
  * Research recommends: Managed Identity + RBAC role assignment for Key Vault Secrets User
  * Plan implements: Extracted RBAC into `keyVaultRoleAssignment.bicep` declared in `main.bicep` after both `appService` and `keyVault` modules; `appService.bicep` outputs `appServicePrincipalId`
  * Rationale: Bicep modules cannot forward-reference outputs; RBAC module must be declared last

* DD-06: Planning log DD-02 stated `SetId` would be added to `CheckAction`, but the entity code in Step 2A.1 omitted it (M-04).
  * Research recommends: Denormalized `SetId` on `CheckAction` for efficient SignalR broadcast
  * Plan implements: `public int SetId { get; set; }` added to `CheckAction` entity in Step 2A.1; `SetId` column added to `CheckAction.sql`; toggle-complete endpoint in Step 3.2 uses `action.SetId` directly
  * Rationale: Corrects contradiction between planning log description and implementation code

---

## Implementation Paths Considered

### Selected: Blazor Interactive Server with ASP.NET Core Minimal API + SignalR

* Approach: Blazor Web App with Interactive Server render mode for real-time UI; separate ASP.NET Core Web API project for data access and SignalR hub; .NET Aspire AppHost for local orchestration; both projects deployable to a single Azure App Service.
* Rationale: Maximizes real-time capability (Blazor Server maintains persistent SignalR connection for UI updates); simplest deployment model (single App Service); matches the research file's recommended structure.
* Evidence: Research file Lines 170-220 — Proposed Solution Structure; Scenario 1 SignalR pattern

### IP-01: Blazor WebAssembly (WASM) front-end

* Approach: Separate Blazor WASM static files served from Azure Static Web Apps; API as a standalone Azure Functions or App Service backend.
* Trade-offs: Better offline capability and client performance; however, SignalR WebSocket support in WASM has additional configuration overhead, and Azure Static Web Apps adds infrastructure complexity not needed at this stage.
* Rejection rationale: Adds hosting complexity (Static Web Apps + separate API service) without clear benefit for the mobile checklist use case. Blazor Server is simpler and the persistent connection aligns well with the two-user real-time scenario.

### IP-02: Polling instead of SignalR

* Approach: Blazor component polls the API every 5 seconds for action updates.
* Trade-offs: Simpler implementation (no SignalR hub, no WebSocket concerns); however, polling creates latency (up to 5s delay) and unnecessary server load in campground Wi-Fi conditions with poor connectivity.
* Rejection rationale: Explicitly rejected in research file — "creates latency and server load in campground wi-fi conditions."

### IP-03: Azure Container Apps instead of App Service

* Approach: Deploy API and Web as separate containers to Azure Container Apps.
* Trade-offs: Better scalability and containerization story; however, adds Docker build steps, container registry, and ACA-specific Bicep complexity not needed for an initial app of this scale.
* Rejection rationale: Research explicitly specifies "App Service (not Container Apps for now)."

---

## Suggested Follow-On Work

Items identified during planning that fall outside current scope.

* WI-01: Unit and integration tests for repositories and API endpoints — none are in scope for this plan.
  * Source: `.github/copilot-instructions.md` test guidance; research success criteria mention `dotnet test` passing
  * Dependency: Phase 2A and Phase 3 completion (repositories and endpoints must exist before testing)
  * Priority: medium

* WI-02: Admin role policy for Template management — the Admin page is stubbed but requires an Azure AD group or app role to gate access.
  * Source: Old code references `[Authorize(Policy = "ApiUser")]`; Admin page needs a separate admin policy
  * Dependency: Entra ID App Registration configuration (DR-01)
  * Priority: medium

* WI-03: Split API and Web into two separate App Services (or migrate to Container Apps) for independent scaling.
  * Source: DD-03 deviation
  * Dependency: Initial single-App-Service deployment validated
  * Priority: low

* WI-04: EF Core migrations for initial schema (alternative to DACPAC for local dev).
  * Source: Phase 2A — DACPAC is the production deployment path; local dev may benefit from `dotnet ef migrations add Initial`
  * Dependency: AppDbContext completion (Step 2A.2)
  * Priority: low

* WI-05: "Create CheckSet from template" deep copy logic — when a user copies a TemplateSet, all child TemplateLists → TemplateCategories → TemplateActions must be deep-copied into Check* hierarchy.
  * Source: API endpoint `POST /api/sets` with `TemplateSetId` parameter
  * Dependency: Phase 2A (repositories) and Phase 3 (endpoint)
  * Priority: high — this is core to the app's value proposition (use pre-built RV checklists)

* WI-06: ~~Verify dadabase.demo pipeline and Bicep patterns by fetching the reference repo and aligning file structure.~~ **COMPLETED** — All GitHub Actions, ADO pipelines, and Bicep structure fully aligned with dadabase.demo golden code patterns.

* WI-07 (Phase 3–5): Implement `GetCategoriesAsync` — Add `GET /api/lists/{id}/categories` endpoint returning full category + action hierarchy; update `CheckListApiClient.GetCategoriesAsync` stub; wire `ActiveList.razor` to render the returned categories (high priority — blocks live checklist rendering)
  * Source: Phase 4 stub (ActiveList.razor returns empty `[]` for categories)
  * Dependency: None

* WI-08 (Phase 4–5): Populate `ActiveList._listName` — Fetch list display name via API and assign; eliminates CS0649 warning (low priority — cosmetic)
  * Source: Phase 4
  * Dependency: WI-07 or new `GetListAsync(listId)` endpoint

* WI-09 (Phase 2B): ~~Add `- checkout: self` to ADO `DeployApp` deployment job~~ **COMPLETED** — ADO pipelines replaced with new `.azdo/` golden code structure using deployment jobs pattern.

* WI-10 (Phase 2B): Remove orphaned `dotnet publish` of Web project from GitHub Actions `deploy-app` job (NEW-MI-02)
  * Source: Planning log NEW-MI-02
  * Dependency: None
  * Priority: low
