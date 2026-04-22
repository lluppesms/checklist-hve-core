<!-- markdownlint-disable-file -->
# Task Research: Shared Checklist App — New Project

A mobile-first Blazor app for collaborative real-time checklists, primarily designed for RV
owners managing arrival/departure procedures. Two users need to see each other's task completions
in real time while working in a campground with their phones out.

## Task Implementation Requests

* Build a new C# / Blazor / .NET Aspire app from scratch (do NOT copy old code)
* ASP.NET Core Web API backend with Entity Framework Core and Azure SQL Database
* SignalR for real-time task-completion updates shared between users
* Blazor front-end — mobile-responsive, simple, fast
* .NET Aspire orchestration host / AppHost project
* Azure deployment via App Service (Bicep IaC)
* Azure DevOps YAML pipelines (CI/CD)
* GitHub Actions YAML workflows (CI/CD)
* SQL DACPAC project to deploy schema + sample data (based on existing DB design)
* Use `https://github.com/lluppesms/dadabase.demo` as the golden-pattern reference

## Scope and Success Criteria

* Scope: New solution from scratch; old code is reference-only for DB model and sample data
* Assumptions:
  * Azure SQL Database is the target RDBMS
  * Authentication via Azure AD / Microsoft Entra ID (same pattern as old code)
  * Primary users are on mobile browsers (no native app needed)
  * Two+ users sharing a "set" of checklists must see updates in real time
  * App will be hosted in Azure App Service (not Container Apps for now)
* Success Criteria:
  * Solution builds cleanly with `dotnet build`
  * SignalR hub broadcasts task-complete events to all connected clients watching the same set
  * Blazor pages work on mobile screens (Bootstrap responsive)
  * Bicep deploys all required Azure resources (App Service, SQL, Key Vault, App Insights)
  * CI/CD pipelines are in place for both ADO and GitHub Actions
  * SQL DACPAC project deploys schema + RV sample data

## Outline

1. Solution structure and project layout
2. Data model (from existing code — well-designed, keep it)
3. API design (endpoints, SignalR hub)
4. Blazor UI design (pages, components)
5. .NET Aspire AppHost
6. Azure infrastructure (Bicep)
7. CI/CD pipelines

## Potential Next Research

* Review `dadabase.demo` repo structure for pipeline and Bicep patterns
  * Reasoning: README says to model CI/CD and Bicep on that repo
  * Reference: https://github.com/lluppesms/dadabase.demo
* Review latest .NET Aspire starter template patterns
  * Reasoning: Need to wire up Blazor + API + SignalR correctly in Aspire

## Research Executed

### File Analysis

* `README.md`
  * App concept: RV checklist, two users, real-time sync
  * Tech stack: C# Blazor, Aspire, ASP.NET Core Web API, SignalR, Azure SQL, Azure App Service
  * Reference repo: `https://github.com/lluppesms/dadabase.demo` for pipelines and Bicep
  * Old prototype in `old-source/` — DB structure is good, code is not production quality
* `old-source/CheckList.Core/Models/ProjectEntities.cs`
  * EF Core DbContext inheriting `IdentityDbContext<AppUser>`
  * Tables: CheckAction, TemplateSet, TemplateList, TemplateCategory, TemplateAction, CheckSet, CheckList, CheckCategory, Customer
* `old-source/CheckList.Core/Models/Tables/*.cs`
  * Full entity model reviewed (see Data Model section below)
* `old-source/CheckList.Core/Hub/ActionHub.cs` + `ITypedHubClient.cs`
  * Hub extends `Hub<ITypedHubClient>`
  * Interface: `Task BroadcastMessage(string type, string payload)`
  * Group-based routing (by "room"/set name)
* `old-source/Database/LylesCheckList.sql`
  * Rich RV sample data — 3 lists: Trip Prep, Hitching, Arrival
  * Confirmed DB structure: CheckSet → CheckList → CheckCategory → CheckAction hierarchy
* `old-source/CheckList.Core/API/*.cs`
  * Repository pattern with interfaces: `ICheckListRepository`, `ICheckActionRepository`, etc.
  * Controllers use `[Authorize(Policy = "ApiUser")]`

### Project Conventions

* File-scoped namespaces (C# 10+)
* PascalCase public / camelCase private
* XML doc comments on all public APIs
* Component-specific CSS in `.razor.css` files
* Bootstrap for responsive layout — no hard-coded colors (respect light/dark themes)
* Services in DI, interfaces in separate files
* Using directives: System → Microsoft → App; alphabetical within groups

## Key Discoveries

### Data Model

The existing database has two parallel hierarchies:

**Template hierarchy** (read-only "master" definitions):
```
TemplateSet (id, setName, setDscr, ownerName, activeInd, sortOrder, createDateTime)
  └── TemplateList (id, setId, listName, listDscr, activeInd, sortOrder, createDateTime)
        └── TemplateCategory (id, listId, categoryText, categoryDscr, activeInd, sortOrder, createDateTime)
              └── TemplateAction (id, categoryId, actionText, actionDscr, completeInd, sortOrder, createDateTime)
```

**Check (active instance) hierarchy** (one copy per user session):
```
CheckSet (id, templateSetId?, setName, setDscr, ownerName, activeInd, sortOrder, createDateTime)
  └── CheckList (id, setId, listName, listDscr, activeInd, sortOrder, createDateTime)
        └── CheckCategory (id, listId, categoryText, categoryDscr, activeInd, sortOrder, createDateTime)
              └── CheckAction (id, categoryId, listId, actionText, actionDscr, completeInd, sortOrder, createDateTime)
```

Key difference: `CheckAction` has both `categoryId` AND `listId` (denormalized for performance).
`completeInd` is a `char(1)` flag — "Y"/"N" or " "/" ".

**Identity**: `AppUser` extends `IdentityUser`. `Customer` entity links `AppUser` to profile data.

### SignalR Pattern

The old hub uses typed `Hub<ITypedHubClient>` with a single `BroadcastMessage(string type, string payload)` contract. For the new app, the hub should:

* Support joining a "group" named by `setId` (so only watchers of that set receive updates)
* Broadcast when a `CheckAction.completeInd` is toggled
* Payload should include `setId`, `listId`, `actionId`, `completeInd`, `completedBy`, `completedAt`

### Proposed Solution Structure

```
checklist-hve-core.sln
src/
  CheckList.AppHost/           ← .NET Aspire orchestration
    Program.cs
    CheckList.AppHost.csproj
  CheckList.ServiceDefaults/   ← Shared Aspire service defaults
    Extensions.cs
    CheckList.ServiceDefaults.csproj
  CheckList.Api/               ← ASP.NET Core Web API + SignalR hub
    CheckList.Api.csproj
    Program.cs
    Data/
      AppDbContext.cs
      Models/                  ← Entities
    Repositories/
      Interfaces/
      Implementations/
    Endpoints/                 ← Minimal API endpoint groups
    Hubs/
      CheckListHub.cs
      ICheckListHubClient.cs
  CheckList.Web/               ← Blazor Web App (interactive server-side)
    CheckList.Web.csproj
    Program.cs
    Components/
      Pages/
      Shared/
    wwwroot/
  CheckList.Shared/            ← Shared DTOs / contracts (optional)
    CheckList.Shared.csproj
    DTOs/
infra/
  bicep/
    main.bicep
    modules/
      appService.bicep
      appServicePlan.bicep
      sqlServer.bicep
      sqlDatabase.bicep
      keyVault.bicep
      appInsights.bicep
      logAnalytics.bicep
Database/
  CheckList.Database.sqlproj   ← SQL DACPAC project
  Schema/
    Tables/
  SeedData/
.github/
  workflows/
    build-and-test.yml
    deploy.yml
.azuredevops/
  pipelines/
    build-and-test.yml
    deploy.yml
Docs/
  README.md (architecture notes)
```

### API Design

Proposed minimal API endpoint groups (matching old controller routes):

| Route | Method | Description |
|---|---|---|
| `/api/sets` | GET | List CheckSets for current user |
| `/api/sets/{id}` | GET | Get one CheckSet |
| `/api/sets` | POST | Create CheckSet (from template or blank) |
| `/api/sets/{id}/lists` | GET | Get all CheckLists in a set |
| `/api/lists/{id}/actions` | GET | Get all CheckActions for a list |
| `/api/actions/{id}/complete` | PUT | Toggle complete on an action (triggers SignalR) |
| `/api/templates` | GET | List TemplateSets |
| `/api/templates/{setId}/lists` | GET | Lists in a template set |
| `/signalr/checklist` | WS | SignalR hub endpoint |

### Blazor UI Pages

| Page | Route | Purpose |
|---|---|---|
| Home | `/` | Welcome / select active set |
| Sets | `/sets` | Manage check sets |
| Set Detail | `/sets/{id}` | View lists in a set |
| Active List | `/sets/{id}/lists/{listId}` | Main checklist UI with checkboxes; SignalR-connected |
| Templates | `/templates` | Browse and copy template sets |
| Admin | `/admin` | Manage template definitions |

### Azure Infrastructure (Bicep)

Based on dadabase.demo patterns, resources needed:
* `Microsoft.Web/serverfarms` (App Service Plan — at least S1 for SignalR WebSockets)
* `Microsoft.Web/sites` (App Service — API + Blazor)
* `Microsoft.Sql/servers` + `Microsoft.Sql/servers/databases` (Azure SQL)
* `Microsoft.KeyVault/vaults` (secrets: DB connection string, JWT key)
* `Microsoft.Insights/components` (Application Insights)
* `Microsoft.OperationalInsights/workspaces` (Log Analytics workspace)
* Managed Identity on App Service + RBAC assignments

### CI/CD Pipeline Pattern (from dadabase.demo)

ADO pipelines (`/.azuredevops/pipelines/`):
* `build-and-test.yml` — restore, build, test, publish artifacts
* `deploy.yml` — deploy Bicep, then deploy app to App Service

GitHub Actions (`/.github/workflows/`):
* `build-and-test.yml` — same as ADO build
* `deploy.yml` — same as ADO deploy, using GitHub OIDC for Azure auth

## Technical Scenarios

### Scenario 1: Real-Time Task Completion via SignalR

**Description:** When User A checks off a task, User B (on the same "set") sees it update
immediately without refreshing.

**Requirements:**
* SignalR hub with group-based routing by `setId`
* Blazor component subscribes to hub on page load and unsubscribes on dispose
* API endpoint calls hub after persisting the update

**Preferred Approach:**

Use a typed SignalR hub with Blazor's `HubConnection` client. The API persists the change and
then calls `IHubContext<CheckListHub, ICheckListHubClient>` to push to the group.

Blazor component joins the group on `OnInitializedAsync` and handles `ReceiveActionUpdate`.

```csharp
// ICheckListHubClient.cs
public interface ICheckListHubClient
{
    Task ReceiveActionUpdate(ActionUpdateDto update);
}

// CheckListHub.cs
public class CheckListHub : Hub<ICheckListHubClient>
{
    public async Task JoinSet(string setId)
        => await Groups.AddToGroupAsync(Context.ConnectionId, $"set-{setId}");

    public async Task LeaveSet(string setId)
        => await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"set-{setId}");
}
```

```csharp
// In the API endpoint that toggles an action
await hubContext.Clients.Group($"set-{setId}").ReceiveActionUpdate(updateDto);
```

```razor
@* ActiveList.razor *@
@implements IAsyncDisposable

@code {
    private HubConnection? _hubConnection;

    protected override async Task OnInitializedAsync()
    {
        _hubConnection = new HubConnectionBuilder()
            .WithUrl(NavigationManager.ToAbsoluteUri("/hubs/checklist"))
            .WithAutomaticReconnect()
            .Build();

        _hubConnection.On<ActionUpdateDto>("ReceiveActionUpdate", update =>
        {
            // update local state and call StateHasChanged()
        });

        await _hubConnection.StartAsync();
        await _hubConnection.SendAsync("JoinSet", SetId.ToString());
    }

    public async ValueTask DisposeAsync()
    {
        if (_hubConnection is not null)
            await _hubConnection.DisposeAsync();
    }
}
```

#### Considered Alternatives

* **Server-sent Events (SSE)**: Simpler but unidirectional; rejected because two-way group
  messaging is needed.
* **Polling**: Simple but creates latency and server load in campground wi-fi conditions;
  rejected.

### Scenario 2: Aspire Orchestration

**Description:** .NET Aspire AppHost wires up the API and Blazor Web App as separate projects
with service discovery.

**Preferred Approach:**

```csharp
// CheckList.AppHost/Program.cs
var builder = DistributedApplication.CreateBuilder(args);

var sql = builder.AddSqlServer("sql")
    .AddDatabase("checklistdb");

var api = builder.AddProject<Projects.CheckList_Api>("api")
    .WithReference(sql);

builder.AddProject<Projects.CheckList_Web>("web")
    .WithReference(api);

builder.Build().Run();
```

Blazor Web App uses `HttpClient` configured by Aspire service discovery to call the API.
SignalR URL is the API project's base URL + `/hubs/checklist`.

### Scenario 3: Database Deployment (DACPAC)

**Description:** A SQL Server Database project deploys the schema and seeds sample RV data.

**Preferred Approach:**

Create `Database/CheckList.Database.sqlproj` using the `Microsoft.Build.Sql` SDK.  
Tables match the existing entity model exactly (two hierarchies: Template* and Check*).  
Seed data scripts (`PostDeployment.sql`) insert the RV checklist sample data from
`old-source/Database/LylesCheckList.sql` and `old-source/Database/ChangingLanesLists.sql`.

DACPAC is built in CI and deployed in CD using `SqlPackage.exe publish`.

## Implementation Next Steps

1. **Initialize solution** — `dotnet new aspire` then add projects per the structure above
2. **Create domain entities** — translate old EF entities to new C# record/class style with
   file-scoped namespaces and modern C# features
3. **Create DbContext** — `AppDbContext : IdentityDbContext<AppUser>` with all 9 entity DbSets
4. **Build API endpoints** — minimal API endpoint groups with repository pattern
5. **Build SignalR hub** — typed hub, group join/leave, broadcast on action toggle
6. **Build Blazor pages** — mobile-first using Bootstrap; ActiveList page with SignalR client
7. **Add Aspire wiring** — AppHost, ServiceDefaults, health checks, telemetry
8. **Create Bicep** — based on dadabase.demo patterns; all 6 resource types listed above
9. **Create CI/CD** — ADO pipelines + GitHub Actions (OIDC auth for Azure)
10. **Create DACPAC project** — schema + seed data, wired into CI/CD
