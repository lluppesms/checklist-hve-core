using CheckList.Api.Data;
using CheckList.Api.Data.Models;
using CheckList.Api.Endpoints;
using CheckList.Api.Hubs;
using CheckList.Api.Repositories.Implementations;
using CheckList.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();  // Aspire ServiceDefaults

// Authentication — Microsoft Entra ID
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

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS for Blazor Web App
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy =>
        policy.WithOrigins(builder.Configuration["WebAppUrl"] ?? "https://localhost:7001")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials()));  // required for SignalR

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Map endpoint groups
app.MapCheckSets();
app.MapCheckLists();
app.MapCheckActions();
app.MapTemplateSets();

// Map SignalR hub
app.MapHub<CheckListHub>("/hubs/checklist");

app.MapDefaultEndpoints();  // Aspire health checks

app.Run();
