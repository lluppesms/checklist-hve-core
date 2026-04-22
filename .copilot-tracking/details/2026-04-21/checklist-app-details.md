<!-- markdownlint-disable-file -->
# Implementation Details: Shared Checklist App — New Blazor/.NET Aspire Solution

## Context Reference

Sources: `.copilot-tracking/research/2026-04-21/checklist-app-research.md`, `old-source/CheckList.Core/Models/`, `old-source/CheckList.Core/Hub/`, `.github/copilot-instructions.md`

---

## Implementation Phase 1: Solution Structure Initialization

<!-- parallelizable: false -->

### Step 1.1: Create `src/` directory and initialize Aspire solution projects

Run the following commands from the workspace root (`c:\ghcp\agents\checklist-hve-core`):

```powershell
# Create top-level directories
New-Item -ItemType Directory -Path src, infra/bicep/modules, Database/Schema/Tables, Database/SeedData, Docs -Force

# Create the Aspire AppHost project
dotnet new aspire-apphost -n CheckList.AppHost -o src/CheckList.AppHost

# Create the Aspire ServiceDefaults project
dotnet new aspire-servicedefaults -n CheckList.ServiceDefaults -o src/CheckList.ServiceDefaults

# Create the Web API project
dotnet new webapi -n CheckList.Api -o src/CheckList.Api --use-minimal-apis

# Create the Blazor Web App project
dotnet new blazor -n CheckList.Web -o src/CheckList.Web --interactivity Server

# Create the shared class library
dotnet new classlib -n CheckList.Shared -o src/CheckList.Shared

# Create the test project
dotnet new mstest -n CheckList.Tests -o src/CheckList.Tests
```

Files created: all project files under `src/`

Dependencies: .NET 9 SDK, `dotnet workload install aspire`

Success criteria:
* Each project directory exists with a `.csproj` file
* `dotnet build` succeeds for each individual project before adding references

---

### Step 1.2: Create `CheckList.AppHost` project

File: `src/CheckList.AppHost/CheckList.AppHost.csproj`

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net9.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    <IsAspireHost>true</IsAspireHost>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="Aspire.Hosting.AppHost" Version="9.*" />
    <PackageReference Include="Aspire.Hosting.SqlServer" Version="9.*" />
  </ItemGroup>
  <ItemGroup>
    <ProjectReference Include="..\CheckList.Api\CheckList.Api.csproj" />
    <ProjectReference Include="..\CheckList.Web\CheckList.Web.csproj" />
  </ItemGroup>
</Project>
```

File: `src/CheckList.AppHost/Program.cs`

```csharp
var builder = DistributedApplication.CreateBuilder(args);

var sql = builder.AddSqlServer("sql")
    .AddDatabase("checklistdb");

var api = builder.AddProject<Projects.CheckList_Api>("api")
    .WithReference(sql)
    .WaitFor(sql);

builder.AddProject<Projects.CheckList_Web>("web")
    .WithReference(api)
    .WaitFor(api);

builder.Build().Run();
```

Success criteria:
* AppHost references both CheckList.Api and CheckList.Web
* SQL Server dependency is declared with `WaitFor` ordering

---

### Step 1.3: Create `CheckList.ServiceDefaults` project

File: `src/CheckList.ServiceDefaults/CheckList.ServiceDefaults.csproj`

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    <IsAspireSharedProject>true</IsAspireSharedProject>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="Microsoft.Extensions.Http.Resilience" Version="9.*" />
    <PackageReference Include="Microsoft.Extensions.ServiceDiscovery" Version="9.*" />
    <PackageReference Include="OpenTelemetry.Exporter.OpenTelemetryProtocol" Version="1.*" />
    <PackageReference Include="OpenTelemetry.Extensions.Hosting" Version="1.*" />
    <PackageReference Include="OpenTelemetry.Instrumentation.AspNetCore" Version="1.*" />
    <PackageReference Include="OpenTelemetry.Instrumentation.Http" Version="1.*" />
    <PackageReference Include="OpenTelemetry.Instrumentation.Runtime" Version="1.*" />
  </ItemGroup>
</Project>
```

File: `src/CheckList.ServiceDefaults/Extensions.cs`

Configure health checks, OpenTelemetry, service discovery, and resilience via `AddServiceDefaults` extension method (standard Aspire pattern).

Success criteria:
* `AddServiceDefaults()` extension method is defined and callable by API and Web projects

---

### Step 1.4: Create `CheckList.Api` project

File: `src/CheckList.Api/CheckList.Api.csproj`

Key NuGet packages:
* `Aspire.Microsoft.EntityFrameworkCore.SqlServer`
* `Microsoft.AspNetCore.Authentication.JwtBearer`
* `Microsoft.AspNetCore.Identity.EntityFrameworkCore`
* `Microsoft.AspNetCore.SignalR`
* `Microsoft.EntityFrameworkCore.Design`
* `Microsoft.Identity.Web` — required for `AddMicrosoftIdentityWebApi()` extension method (Entra ID auth)
* `Swashbuckle.AspNetCore`

Project references:
* `CheckList.ServiceDefaults`
* `CheckList.Shared`

Success criteria:
* Project references ServiceDefaults and Shared
* All listed NuGet packages restore successfully
* `Microsoft.Identity.Web` package enables `AddMicrosoftIdentityWebApi()` to compile

---

### Step 1.5: Create `CheckList.Web` project

File: `src/CheckList.Web/CheckList.Web.csproj`

Key NuGet packages:
* `Microsoft.AspNetCore.SignalR.Client`
* `Microsoft.Extensions.Http.Resilience` (via ServiceDefaults)
* `Microsoft.Identity.Web` — required for `AddMicrosoftIdentityWebApp()` extension method
* `Microsoft.Identity.Web.UI` — required for OIDC login/logout UI helpers

Project references:
* `CheckList.ServiceDefaults`
* `CheckList.Shared`

Success criteria:
* Project references ServiceDefaults and Shared
* `Microsoft.AspNetCore.SignalR.Client` package is available for hub connection in Blazor
* `Microsoft.Identity.Web` enables `AddMicrosoftIdentityWebApp()` and `OpenIdConnectDefaults` to compile

---

### Step 1.6: Create `CheckList.Shared` project

File: `src/CheckList.Shared/CheckList.Shared.csproj`

No special NuGet packages needed (plain class library for DTOs).

Success criteria:
* Library is referenceable from both Api and Web projects
* Contains only POCOs and interfaces — no infrastructure dependencies

---

### Step 1.7: Update `checklist-hve-core.sln`

Add all six new projects to the existing solution file:

```powershell
dotnet sln checklist-hve-core.sln add `
  src/CheckList.AppHost/CheckList.AppHost.csproj `
  src/CheckList.ServiceDefaults/CheckList.ServiceDefaults.csproj `
  src/CheckList.Api/CheckList.Api.csproj `
  src/CheckList.Web/CheckList.Web.csproj `
  src/CheckList.Shared/CheckList.Shared.csproj `
  src/CheckList.Tests/CheckList.Tests.csproj
```

Note: The SQL DACPAC project (`Database/CheckList.Database.sqlproj`) uses a different SDK and is added separately or managed via a database solution folder.

Success criteria:
* `dotnet sln list` shows all 6 new projects
* `dotnet build checklist-hve-core.sln` produces no missing-project errors

---

## Implementation Phase 2A: Data Layer

<!-- parallelizable: true -->

### Step 2A.1: Create domain entities in `CheckList.Api/Data/Models/`

Reference: `old-source/CheckList.Core/Models/Tables/` — translate to modern C# with file-scoped namespaces, nullable reference types, record-style or class-style entities.

Files to create:

**`src/CheckList.Api/Data/Models/AppUser.cs`**
```csharp
namespace CheckList.Api.Data.Models;

using Microsoft.AspNetCore.Identity;

public class AppUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public Customer? Customer { get; set; }
}
```

**`src/CheckList.Api/Data/Models/Customer.cs`**
```csharp
namespace CheckList.Api.Data.Models;

public class Customer
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string? CustomerName { get; set; }
    public AppUser? User { get; set; }
}
```

**`src/CheckList.Api/Data/Models/TemplateSet.cs`**
```csharp
namespace CheckList.Api.Data.Models;

public class TemplateSet
{
    public int Id { get; set; }
    public string SetName { get; set; } = string.Empty;
    public string? SetDscr { get; set; }
    public string? OwnerName { get; set; }
    public string ActiveInd { get; set; } = "Y";
    public int SortOrder { get; set; }
    public DateTime CreateDateTime { get; set; } = DateTime.UtcNow;
    public ICollection<TemplateList> TemplateLists { get; set; } = [];
}
```

**`src/CheckList.Api/Data/Models/TemplateList.cs`**
```csharp
namespace CheckList.Api.Data.Models;

public class TemplateList
{
    public int Id { get; set; }
    public int SetId { get; set; }
    public string ListName { get; set; } = string.Empty;
    public string? ListDscr { get; set; }
    public string ActiveInd { get; set; } = "Y";
    public int SortOrder { get; set; }
    public DateTime CreateDateTime { get; set; } = DateTime.UtcNow;
    public TemplateSet? Set { get; set; }
    public ICollection<TemplateCategory> TemplateCategories { get; set; } = [];
}
```

**`src/CheckList.Api/Data/Models/TemplateCategory.cs`** and **`TemplateAction.cs`** follow the same pattern with `CategoryText`/`ActionText` properties and parent navigation properties.

**`src/CheckList.Api/Data/Models/CheckSet.cs`**
```csharp
namespace CheckList.Api.Data.Models;

public class CheckSet
{
    public int Id { get; set; }
    public int? TemplateSetId { get; set; }
    public string SetName { get; set; } = string.Empty;
    public string? SetDscr { get; set; }
    public string? OwnerName { get; set; }
    public string ActiveInd { get; set; } = "Y";
    public int SortOrder { get; set; }
    public DateTime CreateDateTime { get; set; } = DateTime.UtcNow;
    public TemplateSet? TemplateSet { get; set; }
    public ICollection<CheckList> CheckLists { get; set; } = [];
}
```

**`src/CheckList.Api/Data/Models/CheckList.cs`**, **`CheckCategory.cs`** follow the same pattern as their Template counterparts.

**`src/CheckList.Api/Data/Models/CheckAction.cs`**
```csharp
namespace CheckList.Api.Data.Models;

public class CheckAction
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public int ListId { get; set; }          // denormalized for performance
    public int SetId { get; set; }           // denormalized for SignalR group broadcast (DD-02)
    public string ActionText { get; set; } = string.Empty;
    public string? ActionDscr { get; set; }
    public string CompleteInd { get; set; } = " ";   // "Y" or " "
    public string? CompletedBy { get; set; }
    public DateTime? CompletedAt { get; set; }
    public int SortOrder { get; set; }
    public DateTime CreateDateTime { get; set; } = DateTime.UtcNow;
    public CheckCategory? Category { get; set; }
}
```

Note: `SetId` must also be added to `Database/Schema/Tables/CheckAction.sql` as an `INT NOT NULL` column.

Context references:
* `old-source/CheckList.Core/Models/Tables/` — original entity definitions
* Research file Lines 60-100 — data model hierarchy diagram

Discrepancy references:
* Addresses DD-01: `CompleteInd` kept as `char(1)` string to match DB schema; `CompletedBy` and `CompletedAt` added as new fields for audit trail (not in old model but needed for real-time broadcast DTO)

Success criteria:
* All 10 entity classes created with correct navigation properties
* Collection initializers use C# 12 `[]` syntax
* All properties use nullable reference types appropriately

Dependencies:
* Step 1.4 (CheckList.Api project must exist)

---

### Step 2A.2: Create `AppDbContext` in `CheckList.Api/Data/`

File: `src/CheckList.Api/Data/AppDbContext.cs`

```csharp
namespace CheckList.Api.Data;

using CheckList.Api.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class AppDbContext(DbContextOptions<AppDbContext> options) 
    : IdentityDbContext<AppUser>(options)
{
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<TemplateSet> TemplateSets => Set<TemplateSet>();
    public DbSet<TemplateList> TemplateLists => Set<TemplateList>();
    public DbSet<TemplateCategory> TemplateCategories => Set<TemplateCategory>();
    public DbSet<TemplateAction> TemplateActions => Set<TemplateAction>();
    public DbSet<CheckSet> CheckSets => Set<CheckSet>();
    public DbSet<CheckList> CheckLists => Set<CheckList>();
    public DbSet<CheckCategory> CheckCategories => Set<CheckCategory>();
    public DbSet<CheckAction> CheckActions => Set<CheckAction>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Configure table names, indexes, and column types here
        // CheckAction.CompleteInd: nchar(1), default " "
        builder.Entity<CheckAction>()
            .Property(a => a.CompleteInd)
            .HasColumnType("nchar(1)")
            .HasDefaultValue(" ");
    }
}
```

Success criteria:
* All 9 entity DbSets are declared
* `OnModelCreating` configures `CompleteInd` column type correctly
* Primary constructor syntax used (C# 12)

Dependencies:
* Step 2A.1 (all entity classes must exist)

---

### Step 2A.3: Create repository interfaces in `CheckList.Api/Repositories/Interfaces/`

Files to create:

**`ICheckSetRepository.cs`** — CRUD for CheckSets + list by owner
**`ICheckListRepository.cs`** — CRUD for CheckLists + list by setId
**`ICheckCategoryRepository.cs`** — CRUD for CheckCategories + list by listId
**`ICheckActionRepository.cs`** — CRUD + `ToggleCompleteAsync(int id, string userId)`
**`ITemplateSetRepository.cs`** — Read-only: list all, get by id with children
**`ITemplateListRepository.cs`**, **`ITemplateCategoryRepository.cs`**, **`ITemplateActionRepository.cs`** — Read-only

All interfaces follow the pattern:
```csharp
namespace CheckList.Api.Repositories.Interfaces;

public interface ICheckSetRepository
{
    Task<IEnumerable<CheckSet>> GetByOwnerAsync(string ownerName);
    Task<CheckSet?> GetByIdAsync(int id);
    Task<CheckSet> CreateAsync(CheckSet entity);
    Task<CheckSet?> UpdateAsync(CheckSet entity);
    Task<bool> DeleteAsync(int id);
}
```

Success criteria:
* 8 interface files created, one per repository
* `ICheckActionRepository` includes `ToggleCompleteAsync` returning the updated `CheckAction`

Dependencies:
* Step 2A.1 (entity types referenced in interface signatures)

---

### Step 2A.4: Create repository implementations in `CheckList.Api/Repositories/Implementations/`

Each implementation injects `AppDbContext` and implements its interface.

Key implementation: `CheckActionRepository.ToggleCompleteAsync`

```csharp
public async Task<CheckAction?> ToggleCompleteAsync(int id, string userId)
{
    var action = await _db.CheckActions.FindAsync(id);
    if (action is null) return null;

    action.CompleteInd = action.CompleteInd.Trim() == "Y" ? " " : "Y";
    action.CompletedBy = action.CompleteInd.Trim() == "Y" ? userId : null;
    action.CompletedAt = action.CompleteInd.Trim() == "Y" ? DateTime.UtcNow : null;
    await _db.SaveChangesAsync();
    return action;
}
```

Register all repositories in `Program.cs` DI container:
```csharp
builder.Services.AddScoped<ICheckSetRepository, CheckSetRepository>();
// ... etc.
```

Success criteria:
* 8 repository implementation files created
* All use `AppDbContext` via constructor injection
* `ToggleCompleteAsync` correctly toggles `CompleteInd` and sets audit fields

Dependencies:
* Steps 2A.2 (AppDbContext) and 2A.3 (interfaces)

---

### Step 2A.5: Create DTOs in `CheckList.Shared/DTOs/`

Files to create:

**`src/CheckList.Shared/DTOs/ActionUpdateDto.cs`** — payload broadcast over SignalR
```csharp
namespace CheckList.Shared.DTOs;

public record ActionUpdateDto(
    int SetId,
    int ListId,
    int ActionId,
    string CompleteInd,
    string? CompletedBy,
    DateTime? CompletedAt
);
```

**`src/CheckList.Shared/DTOs/CheckSetDto.cs`**, **`CheckListDto.cs`**, **`CheckCategoryDto.cs`**, **`CheckActionDto.cs`** — read/response DTOs

**`src/CheckList.Shared/DTOs/CreateCheckSetRequest.cs`** — includes optional `TemplateSetId` for creating from template

Success criteria:
* All DTO files use `record` syntax
* `ActionUpdateDto` matches the SignalR hub broadcast contract
* No infrastructure dependencies in `CheckList.Shared`

Dependencies:
* Step 1.6 (CheckList.Shared project must exist)

---

### Step 2A.6: Create SQL DACPAC project

File: `Database/CheckList.Database.sqlproj`

```xml
<Project DefaultTargets="Build">
  <Sdk Name="Microsoft.Build.Sql" Version="0.1.*" />
  <PropertyGroup>
    <Name>CheckList.Database</Name>
    <DSP>Microsoft.Data.Tools.Schema.Sql.SqlAzureV12DatabaseSchemaProvider</DSP>
    <ModelCollation>1033, CI</ModelCollation>
  </PropertyGroup>
</Project>
```

Schema files under `Database/Schema/Tables/` — one `.sql` file per table:
* `TemplateSet.sql`, `TemplateList.sql`, `TemplateCategory.sql`, `TemplateAction.sql`
* `CheckSet.sql`, `CheckList.sql`, `CheckCategory.sql`, `CheckAction.sql` (include `SetId INT NOT NULL`)
* `Customer.sql`

Reference: `old-source/Database/GenerateDatabase.sql` — extract CREATE TABLE statements

**Identity table strategy (M-05):** ASP.NET Core Identity tables (`AspNetUsers`, `AspNetRoles`, `AspNetUserRoles`, `AspNetUserClaims`, `AspNetRoleClaims`, `AspNetUserLogins`, `AspNetUserTokens`) are NOT managed by the DACPAC. They are created via EF Core migrations.

Add an EF migrations step to the deploy pipeline:
```powershell
# Run once after DACPAC deploy to create Identity tables
dotnet ef database update --project src/CheckList.Api --connection "<azure-sql-connection-string>"
```

In CI/CD pipelines, add `dotnet ef migrations bundle` in the build stage and run the bundle in the deploy stage after the DACPAC deploy. This keeps domain tables in DACPAC and Identity tables in EF migrations (two clear ownership boundaries).

File: `Database/SeedData/PostDeployment.sql`
```sql
:r .\RVLists.sql
:r .\ChangingLanes.sql
```

File: `Database/SeedData/RVLists.sql` — extracted from `old-source/Database/LylesCheckList.sql`
File: `Database/SeedData/ChangingLanes.sql` — extracted from `old-source/Database/ChangingLanesLists.sql`

Success criteria:
* `Database/CheckList.Database.sqlproj` builds with `dotnet build` (requires `Microsoft.Build.Sql` SDK)
* All 9 domain tables defined in Schema/Tables/
* PostDeployment.sql references both seed data files

Dependencies:
* Step 2A.1 (entity model defines the schema)

---

## Implementation Phase 2B: Azure Infrastructure and CI/CD

<!-- parallelizable: true -->

### Step 2B.1: Create `infra/bicep/main.bicep` and module stubs

File: `infra/bicep/main.bicep`

```bicep
targetScope = 'resourceGroup'

@description('Environment name (dev, staging, prod)')
param environmentName string = 'dev'

@description('Azure region for all resources')
param location string = resourceGroup().location

@description('Application name prefix')
param appName string = 'checklist'

var resourcePrefix = '${appName}-${environmentName}'

module logAnalytics 'modules/logAnalytics.bicep' = {
  name: 'logAnalytics'
  params: { location: location, workspaceName: '${resourcePrefix}-law' }
}

module appInsights 'modules/appInsights.bicep' = {
  name: 'appInsights'
  params: { location: location, appInsightsName: '${resourcePrefix}-ai', logAnalyticsWorkspaceId: logAnalytics.outputs.workspaceId }
}

module keyVault 'modules/keyVault.bicep' = {
  name: 'keyVault'
  params: { location: location, keyVaultName: '${resourcePrefix}-kv' }
}

module sqlServer 'modules/sqlServer.bicep' = {
  name: 'sqlServer'
  params: { location: location, serverName: '${resourcePrefix}-sql', administratorLogin: 'sqladmin' }
}

module sqlDatabase 'modules/sqlDatabase.bicep' = {
  name: 'sqlDatabase'
  params: { serverName: sqlServer.outputs.serverName, databaseName: '${resourcePrefix}-db' }
}

module appServicePlan 'modules/appServicePlan.bicep' = {
  name: 'appServicePlan'
  params: { location: location, planName: '${resourcePrefix}-asp', sku: 'S1' }
}

module appService 'modules/appService.bicep' = {
  name: 'appService'
  params: {
    location: location
    appName: '${resourcePrefix}-app'
    planId: appServicePlan.outputs.planId
    appInsightsConnectionString: appInsights.outputs.connectionString
    keyVaultName: keyVault.outputs.keyVaultName
  }
}

// RBAC: grant App Service managed identity access to Key Vault — declared after both modules
module keyVaultRoleAssignment 'modules/keyVaultRoleAssignment.bicep' = {
  name: 'keyVaultRoleAssignment'
  params: {
    keyVaultName: keyVault.outputs.keyVaultName
    principalId: appService.outputs.appServicePrincipalId
  }
}
```

Note on M-02 (RBAC wiring fix): The RBAC role assignment must be a separate module declared *after* both `keyVault` and `appService` modules, because it depends on outputs from both. The `keyVault.bicep` module itself should NOT contain the role assignment — it cannot reference `appService.outputs.principalId` before the App Service is declared.

Create `infra/bicep/modules/keyVaultRoleAssignment.bicep`:
```bicep
param keyVaultName string
param principalId string

resource kv 'Microsoft.KeyVault/vaults@2023-07-01' existing = {
  name: keyVaultName
}

resource kvRoleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  scope: kv
  name: guid(kv.id, principalId, 'Key Vault Secrets User')
  properties: {
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', '4633458b-17de-408a-b874-0445c86b69e6')
    principalId: principalId
    principalType: 'ServicePrincipal'
  }
}
```

Success criteria:
* `main.bicep` references all 8 module files (7 resource modules + 1 RBAC module)
* Parameters use `environmentName`, `location`, `appName` for reusability
* Module outputs are wired (e.g., planId, workspaceId)

---

### Step 2B.2: Create `infra/bicep/modules/appServicePlan.bicep`

```bicep
param location string
param planName string
param sku string = 'S1'   // S1 minimum for WebSocket/SignalR

resource plan 'Microsoft.Web/serverfarms@2023-12-01' = {
  name: planName
  location: location
  sku: { name: sku, tier: 'Standard' }
  properties: { reserved: false }
}

output planId string = plan.id
```

Note: S1 (Standard tier) is the minimum required for WebSocket support needed by SignalR. Free/Shared/Basic tiers do not support WebSockets.

---

### Step 2B.3: Create `infra/bicep/modules/appService.bicep`

Key settings to configure:
* `webSocketsEnabled: true` — required for SignalR
* `alwaysOn: true` — prevent cold start breaking persistent connections
* Managed Identity (`identity: { type: 'SystemAssigned' }`)
* App Settings: `APPLICATIONINSIGHTS_CONNECTION_STRING`, `ASPNETCORE_ENVIRONMENT`
* `httpsOnly: true`
* `minTlsVersion: '1.2'`

Output: `appServicePrincipalId` — **must be declared** so `main.bicep` can pass it to `keyVaultRoleAssignment`:
```bicep
output appServicePrincipalId string = app.identity.principalId
```

---

### Step 2B.4: Create `infra/bicep/modules/sqlServer.bicep` and `sqlDatabase.bicep`

`sqlServer.bicep`:
* `Microsoft.Sql/servers@2023-08-01-preview`
* Configure firewall rule to allow Azure services
* Output: `serverName`, `serverFqdn`

`sqlDatabase.bicep`:
* `Microsoft.Sql/servers/databases@2023-08-01-preview`
* SKU: `{ name: 'S0', tier: 'Standard' }` (sufficient for dev/staging)
* Output: `databaseName`

---

### Step 2B.5: Create `infra/bicep/modules/keyVault.bicep`

**M-02 fix:** `keyVault.bicep` defines only the vault resource. The RBAC role assignment is in `keyVaultRoleAssignment.bicep` (called from `main.bicep` after both `keyVault` and `appService` modules are declared).

```bicep
param location string
param keyVaultName string

resource kv 'Microsoft.KeyVault/vaults@2023-07-01' = {
  name: keyVaultName
  location: location
  properties: {
    sku: { family: 'A', name: 'standard' }
    tenantId: subscription().tenantId
    enableRbacAuthorization: true
    enableSoftDelete: true
    softDeleteRetentionInDays: 90
  }
}

output keyVaultName string = kv.name
output keyVaultUri string = kv.properties.vaultUri
```

`keyVaultRoleAssignment.bicep` is declared separately in `main.bicep` after `appService` is available (see Step 2B.1). Its definition:
```bicep
param keyVaultName string
param principalId string

resource kv 'Microsoft.KeyVault/vaults@2023-07-01' existing = {
  name: keyVaultName
}

resource kvRoleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  scope: kv
  name: guid(kv.id, principalId, 'Key Vault Secrets User')
  properties: {
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', '4633458b-17de-408a-b874-0445c86b69e6')
    principalId: principalId
    principalType: 'ServicePrincipal'
  }
}
```

---

### Step 2B.6: Create `infra/bicep/modules/appInsights.bicep` and `logAnalytics.bicep`

`logAnalytics.bicep`:
* `Microsoft.OperationalInsights/workspaces@2023-09-01`
* Retention: 30 days
* Output: `workspaceId`

`appInsights.bicep`:
* `Microsoft.Insights/components@2020-02-02`
* `Application_Type: 'web'`
* Linked to Log Analytics workspace
* Output: `connectionString` (use connection string, not instrumentation key — modern pattern)

---

### Step 2B.7: Create GitHub Actions workflows in `.github/workflows/`

**`.github/workflows/build-and-test.yml`**:
```yaml
name: Build and Test

on:
  push:
    branches: [main]
  pull_request:
    branches: [main]

jobs:
  build:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '9.0.x'
      - name: Restore dependencies
        run: dotnet restore checklist-hve-core.sln
      - name: Build
        run: dotnet build checklist-hve-core.sln --no-restore --configuration Release
      - name: Test
        run: dotnet test checklist-hve-core.sln --no-build --configuration Release
```

Note: DACPAC and EF migrations bundle artifacts are built by the `build-schema` job in `deploy.yml`, not in `build-and-test.yml`. This keeps the CI build fast and ensures artifact builds run in the same workflow run as deployment.
```

**`.github/workflows/deploy.yml`**:
```yaml
name: Deploy to Azure

on:
  workflow_dispatch:
  push:
    branches: [main]

permissions:
  id-token: write   # OIDC auth
  contents: read

jobs:
  build-schema:
    runs-on: ubuntu-latest
    outputs:
      dacpac-artifact: dacpac
      efmigrations-artifact: efmigrations
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-dotnet@v4
        with: { dotnet-version: '9.0.x' }
      - name: Restore workloads
        run: dotnet workload restore
      - name: Build DACPAC
        run: dotnet build Database/CheckList.Database.sqlproj -c Release
      - name: Build EF migrations bundle
        run: dotnet ef migrations bundle --project src/CheckList.Api --output ./efmigrations/efbundle --self-contained
      - uses: actions/upload-artifact@v4
        with: { name: dacpac, path: Database/bin/Release/*.dacpac }
      - uses: actions/upload-artifact@v4
        with: { name: efmigrations, path: ./efmigrations/efbundle }
  deploy-infra:
    needs: build-schema
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: azure/login@v2
        with:
          client-id: ${{ secrets.AZURE_CLIENT_ID }}
          tenant-id: ${{ secrets.AZURE_TENANT_ID }}
          subscription-id: ${{ secrets.AZURE_SUBSCRIPTION_ID }}
      - name: Deploy Bicep
        uses: azure/arm-deploy@v2
        with:
          resourceGroupName: ${{ vars.AZURE_RESOURCE_GROUP }}
          template: ./infra/bicep/main.bicep
          parameters: environmentName=${{ vars.ENVIRONMENT_NAME }}
  deploy-schema:
    needs: deploy-infra
    runs-on: ubuntu-latest
    steps:
      - uses: actions/download-artifact@v4
        with: { name: dacpac, path: ./dacpac }
      - uses: actions/download-artifact@v4
        with: { name: efmigrations, path: ./efmigrations }
      - name: Deploy DACPAC (domain tables + seed data)
        run: |
          sqlpackage /Action:Publish \
            /SourceFile:./dacpac/CheckList.Database.dacpac \
            /TargetConnectionString:"${{ secrets.AZURE_SQL_CONNECTION_STRING }}"
      - name: Run EF migrations (Identity tables)
        run: |
          chmod +x ./efmigrations/efbundle
          ./efmigrations/efbundle --connection "${{ secrets.AZURE_SQL_CONNECTION_STRING }}"
  deploy-app:
    needs: deploy-schema
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-dotnet@v4
        with: { dotnet-version: '9.0.x' }
      - run: dotnet publish src/CheckList.Api/CheckList.Api.csproj -c Release -o ./publish/api
      # Note: CheckList.Web requires its own App Service (see WI-03); Web publish is deferred
      - uses: azure/login@v2
        with:
          client-id: ${{ secrets.AZURE_CLIENT_ID }}
          tenant-id: ${{ secrets.AZURE_TENANT_ID }}
          subscription-id: ${{ secrets.AZURE_SUBSCRIPTION_ID }}
      - uses: azure/webapps-deploy@v3
        with:
          app-name: ${{ vars.AZURE_APP_NAME }}
          package: './publish/api'
```

Note: Blazor Server cannot be served as static files from the API process. `CheckList.Web` requires its own App Service (see WI-03 in the Planning Log). Deploy the API only from this pipeline; provision a second App Service for the Web project as follow-on work.

Success criteria:
* OIDC-based auth (no stored service principal secret)
* Separate jobs for infra and app deployment with `needs:` dependency
* DACPAC artifact uploaded in build workflow

---

### Step 2B.8: Create Azure DevOps pipelines in `.azuredevops/pipelines/`

**`.azuredevops/pipelines/build-and-test.yml`**:
```yaml
trigger:
  branches:
    include:
      - main
      - feature/*

pool:
  vmImage: 'ubuntu-latest'

steps:
  - task: UseDotNet@2
    inputs:
      version: '9.0.x'
  - script: dotnet restore checklist-hve-core.sln
    displayName: Restore
  - script: dotnet build checklist-hve-core.sln --configuration Release --no-restore
    displayName: Build
  - script: dotnet test checklist-hve-core.sln --configuration Release --no-build --logger trx
    displayName: Test
  - script: dotnet build Database/CheckList.Database.sqlproj --configuration Release
    displayName: Build DACPAC
  - script: dotnet ef migrations bundle --project src/CheckList.Api --output $(Build.ArtifactStagingDirectory)/efmigrations/efbundle --self-contained
    displayName: Build EF migrations bundle
  - task: PublishBuildArtifacts@1
    inputs:
      pathToPublish: 'Database/bin/Release'
      artifactName: 'dacpac'
  - task: PublishBuildArtifacts@1
    inputs:
      pathToPublish: '$(Build.ArtifactStagingDirectory)/efmigrations'
      artifactName: 'efmigrations'
```

**`.azuredevops/pipelines/deploy.yml`**:
```yaml
trigger: none

resources:
  pipelines:
    - pipeline: CIBuild
      source: 'build-and-test'
      trigger: true

parameters:
  - name: environment
    type: string
    default: dev

stages:
  - stage: DeployInfra
    displayName: Deploy Infrastructure
    jobs:
      - job: BicepDeploy
        steps:
          - task: AzureCLI@2
            inputs:
              azureSubscription: $(AZURE_SERVICE_CONNECTION)
              scriptType: bash
              scriptLocation: inlineScript
              inlineScript: |
                az deployment group create \
                  --resource-group $(AZURE_RESOURCE_GROUP) \
                  --template-file infra/bicep/main.bicep \
                  --parameters environmentName=${{ parameters.environment }}

  - stage: DeploySchema
    dependsOn: DeployInfra
    jobs:
      - job: DeployDacpac
        steps:
          - task: DownloadBuildArtifacts@1
            inputs: { buildType: specific, project: $(System.TeamProjectId), pipeline: CIBuild, artifactName: dacpac, downloadPath: $(System.ArtifactsDirectory) }
          - task: SqlAzureDacpacDeployment@1
            displayName: Deploy DACPAC (domain tables + seed data)
            inputs:
              azureSubscription: $(AZURE_SERVICE_CONNECTION)
              AuthenticationType: connectionString
              ConnectionString: $(AZURE_SQL_CONNECTION_STRING)
              deployType: DacpacTask
              DeploymentAction: Publish
              DacpacFile: '$(System.ArtifactsDirectory)/dacpac/*.dacpac'
          - task: DownloadBuildArtifacts@1
            inputs: { buildType: specific, project: $(System.TeamProjectId), pipeline: CIBuild, artifactName: efmigrations, downloadPath: $(System.ArtifactsDirectory) }
          - script: |
              chmod +x $(System.ArtifactsDirectory)/efmigrations/efbundle
              $(System.ArtifactsDirectory)/efmigrations/efbundle --connection "$(AZURE_SQL_CONNECTION_STRING)"
            displayName: Run EF migrations (Identity tables)

  - stage: DeployApp
    dependsOn: DeploySchema
    jobs:
      - deployment: AppDeploy
        environment: ${{ parameters.environment }}
        strategy:
          runOnce:
            deploy:
              steps:
                - checkout: self   # required: deployment job type does not auto-checkout
                - task: DotNetCoreCLI@2
                  inputs:
                    command: publish
                    publishWebProjects: false
                    projects: 'src/CheckList.Api/CheckList.Api.csproj'
                    arguments: '-c Release -o $(Build.ArtifactStagingDirectory)/api'
                - task: AzureWebApp@1
                  inputs:
                    azureSubscription: $(AZURE_SERVICE_CONNECTION)
                    appName: $(AZURE_APP_NAME)
                    package: '$(Build.ArtifactStagingDirectory)/api'
```

Note: Blazor Server cannot be served as static files from the API process. `CheckList.Web` requires its own App Service (see WI-03 in the Planning Log). This stage deploys the API only. Provisioning a second App Service for the Web project is deferred to WI-03.
```

Success criteria:
* ADO pipeline uses `AzureCLI@2` task for Bicep deployment
* Deployment pipeline uses an `environment` parameter for multi-environment support
* Build pipeline publishes test results and DACPAC artifact

---

## Implementation Phase 3: API Layer

<!-- parallelizable: false -->
<!-- Depends on: Phase 2A -->

### Step 3.1: Create SignalR hub and interface in `CheckList.Api/Hubs/`

File: `src/CheckList.Api/Hubs/ICheckListHubClient.cs`
```csharp
namespace CheckList.Api.Hubs;

using CheckList.Shared.DTOs;

public interface ICheckListHubClient
{
    /// <summary>Sent to all clients watching the same set when an action is toggled.</summary>
    Task ReceiveActionUpdate(ActionUpdateDto update);
}
```

File: `src/CheckList.Api/Hubs/CheckListHub.cs`
```csharp
namespace CheckList.Api.Hubs;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

[Authorize]
public class CheckListHub : Hub<ICheckListHubClient>
{
    /// <summary>Join the group for a specific check set to receive real-time updates.</summary>
    public async Task JoinSet(string setId)
        => await Groups.AddToGroupAsync(Context.ConnectionId, $"set-{setId}");

    /// <summary>Leave the group for a specific check set.</summary>
    public async Task LeaveSet(string setId)
        => await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"set-{setId}");
}
```

Context references:
* `old-source/CheckList.Core/Hub/ActionHub.cs` — original typed hub pattern
* Research file Scenario 1 (Lines 195-265) — SignalR implementation pattern

Success criteria:
* Hub uses typed generic `Hub<ICheckListHubClient>`
* Hub is decorated with `[Authorize]`
* Group name format: `set-{setId}`

---

### Step 3.2: Create minimal API endpoint groups in `CheckList.Api/Endpoints/`

Files to create (one file per endpoint group):

**`src/CheckList.Api/Endpoints/CheckSetsEndpoints.cs`**
```csharp
namespace CheckList.Api.Endpoints;

using CheckList.Api.Repositories.Interfaces;
using CheckList.Shared.DTOs;

public static class CheckSetsEndpoints
{
    public static IEndpointRouteBuilder MapCheckSets(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/sets").RequireAuthorization();

        group.MapGet("/", async (ICheckSetRepository repo, HttpContext ctx) =>
        {
            var owner = ctx.User.Identity?.Name ?? string.Empty;
            var sets = await repo.GetByOwnerAsync(owner);
            return Results.Ok(sets);
        });

        group.MapGet("/{id:int}", async (int id, ICheckSetRepository repo) =>
        {
            var set = await repo.GetByIdAsync(id);
            return set is null ? Results.NotFound() : Results.Ok(set);
        });

        group.MapPost("/", async (CreateCheckSetRequest request, ICheckSetRepository repo, HttpContext ctx) =>
        {
            // Map request to CheckSet entity, create, return 201
        });

        return app;
    }
}
```

Additional endpoint files:
* `CheckListsEndpoints.cs` — `/api/sets/{setId}/lists`
* `CheckActionsEndpoints.cs` — `/api/lists/{listId}/actions`, `/api/actions/{id}/complete`
* `TemplateSetsEndpoints.cs` — `/api/templates`, `/api/templates/{setId}/lists`

**Critical endpoint** — toggle complete (triggers SignalR):
```csharp
group.MapPut("/{id:int}/complete", async (
    int id,
    ICheckActionRepository repo,
    IHubContext<CheckListHub, ICheckListHubClient> hubContext,
    HttpContext ctx) =>
{
    var userId = ctx.User.Identity?.Name ?? string.Empty;
    var action = await repo.ToggleCompleteAsync(id, userId);
    if (action is null) return Results.NotFound();

    var update = new ActionUpdateDto(
        SetId: action.SetId,   // denormalized field — see DD-02 and Step 2A.1
        ListId: action.ListId,
        ActionId: action.Id,
        CompleteInd: action.CompleteInd,
        CompletedBy: action.CompletedBy,
        CompletedAt: action.CompletedAt
    );

    await hubContext.Clients.Group($"set-{update.SetId}").ReceiveActionUpdate(update);
    return Results.Ok(update);
});
```

Note: The `SetId` resolution requires either storing `SetId` on `CheckAction` (denormalized further) or loading it from the repository with an include. Adding `SetId` as a denormalized field on `CheckAction` is recommended to avoid joins in the hot path.

Discrepancy references:
* Addresses DD-02: SetId denormalization on CheckAction recommended for toggle endpoint performance

Success criteria:
* All 9 API routes mapped per the research file API Design table
* Toggle-complete endpoint calls `hubContext.Clients.Group()` after persisting
* All endpoint groups require authorization

---

### Step 3.3: Configure `CheckList.Api/Program.cs`

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();  // Aspire ServiceDefaults

// Authentication and authorization
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
builder.Services.AddAuthorization();

// Entity Framework + Identity
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("checklistdb")));
builder.Services.AddIdentityCore<AppUser>()
    .AddEntityFrameworkStores<AppDbContext>();

// Repository registrations
builder.Services.AddScoped<ICheckSetRepository, CheckSetRepository>();
builder.Services.AddScoped<ICheckListRepository, CheckListRepository>();
builder.Services.AddScoped<ICheckCategoryRepository, CheckCategoryRepository>();
builder.Services.AddScoped<ICheckActionRepository, CheckActionRepository>();
builder.Services.AddScoped<ITemplateSetRepository, TemplateSetRepository>();
builder.Services.AddScoped<ITemplateListRepository, TemplateListRepository>();
builder.Services.AddScoped<ITemplateCategoryRepository, TemplateCategoryRepository>();
builder.Services.AddScoped<ITemplateActionRepository, TemplateActionRepository>();

// SignalR
builder.Services.AddSignalR();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS for Blazor Web App
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy =>
        policy.WithOrigins(builder.Configuration["WebAppUrl"] ?? "https://localhost:7001")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials())); // required for SignalR

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseRouting();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment()) { app.UseSwagger(); app.UseSwaggerUI(); }

// Map endpoint groups
app.MapCheckSets();
app.MapCheckLists();
app.MapCheckActions();
app.MapTemplateSets();

// Map SignalR hub
app.MapHub<CheckListHub>("/hubs/checklist");

app.MapDefaultEndpoints();  // Aspire health checks
app.Run();
```

Success criteria:
* CORS is configured with `AllowCredentials()` (required for SignalR)
* All endpoint groups are mapped
* Hub is mapped at `/hubs/checklist`
* `AddServiceDefaults()` is called before `Build()`

---

## Implementation Phase 4: Blazor Web App

<!-- parallelizable: false -->
<!-- Depends on: Phase 3 -->

### Step 4.1: Configure `CheckList.Web/Program.cs`

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// HttpClient with service discovery for API calls
builder.Services.AddHttpClient<CheckListApiClient>(client =>
    client.BaseAddress = new Uri("https+http://api"))
    .AddServiceDiscovery();

// Authentication (Microsoft Entra ID)
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));
builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.MapDefaultEndpoints();
app.Run();
```

Create `CheckList.Web/Services/CheckListApiClient.cs` — typed HTTP client wrapping all API calls.

---

### Step 4.2: Create `App.razor`, `Routes.razor`, `MainLayout.razor`

File: `src/CheckList.Web/Components/App.razor`
```razor
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>CheckList</title>
    <base href="/" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" />
    <link rel="stylesheet" href="app.css" />
    <HeadOutlet />
</head>
<body>
    <Routes />
    <script src="_framework/blazor.server.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
```

File: `src/CheckList.Web/Components/Layout/MainLayout.razor`
```razor
@inherits LayoutComponentBase

<div class="page">
    <NavMenu />
    <main class="container-fluid py-3">
        @Body
    </main>
</div>
```

---

### Step 4.3: Create Home, Sets, SetDetail pages

**`src/CheckList.Web/Components/Pages/Home.razor`** — Welcome message + button to go to Sets
**`src/CheckList.Web/Components/Pages/Sets.razor`** — List all CheckSets for current user; create new set from template
**`src/CheckList.Web/Components/Pages/SetDetail.razor`** — `@page "/sets/{Id:int}"` — list CheckLists in the set; navigate to ActiveList

Each page is decorated with `@attribute [Authorize]` and includes a corresponding `.razor.css` file.

---

### Step 4.4: Create `ActiveList.razor` — main checklist UI with SignalR

File: `src/CheckList.Web/Components/Pages/ActiveList.razor`

```razor
@page "/sets/{SetId:int}/lists/{ListId:int}"
@attribute [Authorize]
@implements IAsyncDisposable
@inject NavigationManager NavigationManager
@inject CheckListApiClient ApiClient
@inject IConfiguration Configuration

<h2>@_list?.ListName</h2>

@if (_categories is null)
{
    <p>Loading...</p>
}
else
{
    foreach (var cat in _categories)
    {
        <div class="category-section mb-3">
            <h5 class="category-header">@cat.CategoryText</h5>
            @foreach (var action in cat.Actions)
            {
                <CheckActionItem Action="action" OnToggle="ToggleAction" />
            }
        </div>
    }
}

@code {
    [Parameter] public int SetId { get; set; }
    [Parameter] public int ListId { get; set; }

    private CheckListDto? _list;
    private List<CheckCategoryDto>? _categories;
    private HubConnection? _hubConnection;

    protected override async Task OnInitializedAsync()
    {
        _list = await ApiClient.GetListAsync(ListId);
        _categories = await ApiClient.GetCategoriesAsync(ListId);

        // M-03 fix: Hub is hosted in CheckList.Api, not CheckList.Web.
        // In Aspire dev, service discovery resolves "https+http://api" to the API project.
        // In production (combined App Service), the base URL is the same origin.
        // Inject IConfiguration and read the API base URL; fall back to the navigation manager
        // origin only when both projects are hosted under the same URL (production single-App-Service).
        var apiBaseUrl = Configuration["services:api:https:0"] 
                      ?? Configuration["services:api:http:0"]
                      ?? NavigationManager.BaseUri.TrimEnd('/');

        _hubConnection = new HubConnectionBuilder()
            .WithUrl($"{apiBaseUrl}/hubs/checklist")
            .WithAutomaticReconnect()
            .Build();

        _hubConnection.On<ActionUpdateDto>("ReceiveActionUpdate", update =>
        {
            var action = _categories?
                .SelectMany(c => c.Actions)
                .FirstOrDefault(a => a.Id == update.ActionId);

            if (action is not null)
            {
                action.CompleteInd = update.CompleteInd;
                action.CompletedBy = update.CompletedBy;
                action.CompletedAt = update.CompletedAt;
                InvokeAsync(StateHasChanged);
            }
        });

        await _hubConnection.StartAsync();
        await _hubConnection.SendAsync("JoinSet", SetId.ToString());
    }

    private async Task ToggleAction(int actionId)
        => await ApiClient.ToggleActionAsync(actionId);

    public async ValueTask DisposeAsync()
    {
        if (_hubConnection is not null)
        {
            await _hubConnection.SendAsync("LeaveSet", SetId.ToString());
            await _hubConnection.DisposeAsync();
        }
    }
}
```

File: `src/CheckList.Web/Components/Pages/ActiveList.razor.css`
```css
.category-header {
    background-color: var(--bs-secondary-bg);
    padding: 0.5rem 1rem;
    border-radius: 0.375rem;
    margin-bottom: 0.5rem;
}

.category-section {
    border: 1px solid var(--bs-border-color);
    border-radius: 0.5rem;
    padding: 0.75rem;
}
```

Success criteria:
* Hub connection is started in `OnInitializedAsync` and disposed in `DisposeAsync`
* `ReceiveActionUpdate` updates local state and calls `InvokeAsync(StateHasChanged)`
* `JoinSet` and `LeaveSet` are called at connection start and dispose
* No hard-coded colors — all use Bootstrap CSS variables

---

### Step 4.5: Create Templates and Admin pages

**`src/CheckList.Web/Components/Pages/Templates.razor`** — `@page "/templates"` — List TemplateSets; button to "Use This Template" (creates a CheckSet from the template)
**`src/CheckList.Web/Components/Pages/Admin.razor`** — `@page "/admin"` — CRUD for template definitions (TemplateSets, TemplateLists, TemplateCategories, TemplateActions)

Both pages require authorization. Admin page may require an admin policy (future work — see planning log WI-03).

---

### Step 4.6: Create shared Blazor components

**`src/CheckList.Web/Components/Shared/NavMenu.razor`**
```razor
<nav class="navbar navbar-expand-lg bg-body-tertiary">
    <div class="container-fluid">
        <a class="navbar-brand" href="/">CheckList</a>
        <ul class="navbar-nav">
            <li class="nav-item"><NavLink class="nav-link" href="sets">My Sets</NavLink></li>
            <li class="nav-item"><NavLink class="nav-link" href="templates">Templates</NavLink></li>
        </ul>
    </div>
</nav>
```

**`src/CheckList.Web/Components/Shared/CheckActionItem.razor`**
```razor
@* Single check action row with checkbox, label, and completion metadata *@
<div class="d-flex align-items-center gap-2 py-1">
    <input type="checkbox"
           class="form-check-input"
           checked="@(Action.CompleteInd?.Trim() == "Y")"
           @onchange="() => OnToggle.InvokeAsync(Action.Id)" />
    <span class="@(Action.CompleteInd?.Trim() == "Y" ? "text-decoration-line-through text-muted" : "")">
        @Action.ActionText
    </span>
    @if (Action.CompletedBy is not null)
    {
        <small class="text-muted ms-auto">@Action.CompletedBy</small>
    }
</div>

@code {
    [Parameter, EditorRequired] public CheckActionDto Action { get; set; } = default!;
    [Parameter, EditorRequired] public EventCallback<int> OnToggle { get; set; }
}
```

**`src/CheckList.Web/Components/Shared/SetCard.razor`** — Bootstrap card displaying a CheckSet with a link to SetDetail.

---

### Step 4.7: Create component-scoped CSS files

Every interactive component (NavMenu, CheckActionItem, SetCard, ActiveList) gets a matching `.razor.css` file. All colors reference Bootstrap CSS variables (`var(--bs-*)`) — no hard-coded hex or RGB values.

---

## Implementation Phase 5: Aspire Wiring

<!-- parallelizable: false -->
<!-- Depends on: Phases 3 and 4 -->

### Step 5.1: Configure `CheckList.AppHost/Program.cs` — complete version

(Full content was shown in Step 1.2; this step confirms all project names match actual project names and the connection string name `checklistdb` is consistent across AppHost and API `appsettings`.)

Final version:
```csharp
var builder = DistributedApplication.CreateBuilder(args);

var sql = builder.AddSqlServer("sql")
    .AddDatabase("checklistdb");

var api = builder.AddProject<Projects.CheckList_Api>("api")
    .WithReference(sql)
    .WaitFor(sql);

builder.AddProject<Projects.CheckList_Web>("web")
    .WithReference(api)
    .WaitFor(api);

builder.Build().Run();
```

Note: The project reference tokens (`Projects.CheckList_Api`, `Projects.CheckList_Web`) are generated by the Aspire tooling from the project names. They must match exactly.

---

### Step 5.2: Configure `CheckList.ServiceDefaults/Extensions.cs`

Standard Aspire ServiceDefaults implementation:

```csharp
namespace CheckList.ServiceDefaults;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

public static class Extensions
{
    public static IHostApplicationBuilder AddServiceDefaults(this IHostApplicationBuilder builder)
    {
        builder.ConfigureOpenTelemetry();
        builder.AddDefaultHealthChecks();
        builder.Services.AddServiceDiscovery();
        builder.Services.ConfigureHttpClientDefaults(http =>
        {
            http.AddStandardResilienceHandler();
            http.AddServiceDiscovery();
        });
        return builder;
    }

    public static IHostApplicationBuilder ConfigureOpenTelemetry(this IHostApplicationBuilder builder)
    {
        builder.Logging.AddOpenTelemetry(logging =>
        {
            logging.IncludeFormattedMessage = true;
            logging.IncludeScopes = true;
        });

        builder.Services.AddOpenTelemetry()
            .WithMetrics(metrics =>
                metrics.AddAspNetCoreInstrumentation()
                       .AddHttpClientInstrumentation()
                       .AddRuntimeInstrumentation())
            .WithTracing(tracing =>
                tracing.AddAspNetCoreInstrumentation()
                       .AddHttpClientInstrumentation());

        builder.AddOpenTelemetryExporters();
        return builder;
    }

    private static IHostApplicationBuilder AddOpenTelemetryExporters(this IHostApplicationBuilder builder)
    {
        var useOtlpExporter = !string.IsNullOrWhiteSpace(builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]);
        if (useOtlpExporter)
            builder.Services.AddOpenTelemetry().UseOtlpExporter();
        return builder;
    }

    public static IHostApplicationBuilder AddDefaultHealthChecks(this IHostApplicationBuilder builder)
    {
        builder.Services.AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy(), ["live"]);
        return builder;
    }

    public static WebApplication MapDefaultEndpoints(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapHealthChecks("/health");
            app.MapHealthChecks("/alive", new HealthCheckOptions
            {
                Predicate = r => r.Tags.Contains("live")
            });
        }
        return app;
    }
}
```

Success criteria:
* `AddServiceDefaults()` configures OpenTelemetry, service discovery, and resilience
* `MapDefaultEndpoints()` maps health check routes
* Both API and Web project `Program.cs` call `builder.AddServiceDefaults()`

---

## Implementation Phase 6: Validation

<!-- parallelizable: false -->

### Step 6.1: Run full solution build

```powershell
dotnet build checklist-hve-core.sln --configuration Release
```

Expected: 0 errors across all projects.

### Step 6.2: Run any unit tests

```powershell
dotnet test checklist-hve-core.sln --configuration Release
```

Note: Unit test project is not in scope for this implementation. This step will pass vacuously unless tests are added.

### Step 6.3: Validate Bicep templates

```powershell
az bicep build --file infra/bicep/main.bicep
```

Validates that all Bicep templates compile without errors. Does not deploy.

### Step 6.4: Fix minor validation issues inline

Iterate on build errors, lint warnings, and namespace mismatches. Apply fixes directly.

### Step 6.5: Report blocking issues

Document any issues requiring additional research (e.g., Entra ID app registration specifics, Azure SQL firewall configuration for local dev).

---

## Dependencies

* .NET 9 SDK
* `dotnet workload install aspire` (for Aspire tooling)
* `Microsoft.Build.Sql` NuGet SDK (for DACPAC project)
* Azure CLI (for Bicep validation, `az bicep build`)
* SqlPackage CLI (for DACPAC deployment in CI/CD)

## Success Criteria

* `dotnet build checklist-hve-core.sln` completes with 0 errors
* SignalR hub `CheckListHub` broadcasts `ReceiveActionUpdate` to correct group
* `ActiveList.razor` subscribes and unsubscribes correctly using `IAsyncDisposable`
* All Bicep modules compile with `az bicep build`
* CI/CD workflows reference correct project paths and use OIDC auth
* SQL DACPAC project builds and includes RV sample data
