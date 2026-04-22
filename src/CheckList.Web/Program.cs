using CheckList.Web.Components;
using CheckList.Web.Services;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// HttpClient with Aspire service discovery — "api" is the name registered in AppHost
builder.Services.AddHttpClient<CheckListApiClient>(client =>
    client.BaseAddress = new Uri("https+http://api"))
    .AddServiceDiscovery();

// Microsoft Entra ID authentication
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));
builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();

// Razor Pages (needed for Microsoft.Identity.Web.UI sign-in/sign-out redirect)
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
app.MapControllers();   // Microsoft Identity UI controllers
app.MapDefaultEndpoints();

app.Run();
