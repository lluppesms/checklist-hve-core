using CheckList.Web.Components;
using CheckList.Web.Data;
using CheckList.Web.Data.Models;
using CheckList.Web.Hubs;
using CheckList.Web.Repositories.Implementations;
using CheckList.Web.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Entity Framework + SQL Server (Aspire-wired connection string)
builder.AddSqlServerDbContext<AppDbContext>("checklistdb");
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

// Microsoft Entra ID authentication (OpenID Connect for interactive Blazor login)
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));
builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();

// Razor Pages (needed for Microsoft.Identity.Web.UI sign-in/sign-out redirects)
builder.Services.AddControllersWithViews()
    .AddMicrosoftIdentityUI();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.MapControllers();
app.MapHub<CheckListHub>("/hubs/checklist");
app.MapDefaultEndpoints();

app.Run();

