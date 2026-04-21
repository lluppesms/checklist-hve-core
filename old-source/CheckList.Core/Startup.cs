using AutoMapper;
using CheckList.Core.Extensions;
using CheckListApp.Auth;
using CheckListApp.Data;
using CheckListApp.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using System;
using System.Net;
using System.Text;

namespace Check2
{
    public class Startup
    {
        private const string SecretKey = "iNivDmHLpUA223sqsfhqGbMRdRj1PVkH"; // todo: get this from somewhere secure
        private readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            // ?  services.TryAddTransient<IHttpContextAccessor, HttpContextAccessor>();

            //// LLL: Register the IConfiguration instance which AppSettings binds against.
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            var settings = Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();
            services.AddSingleton(Configuration);

            ConfigureDatabase(services);
            //ConfigureAuthentication(services, settings);

            //services.AddAuthentication(sharedOptions =>
            //{
            //    sharedOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    sharedOptions.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            //})
            //.AddAzureAd(options => Configuration.Bind("AzureAd", options))
            //.AddCookie();

            //services.AddMvc(options =>
            //    {
            //        var policy = new AuthorizationPolicyBuilder()
            //            .RequireAuthenticatedUser()
            //            .Build();
            //        options.Filters.Add(new AuthorizeFilter(policy));
            //    })
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(opt =>
                {
                    opt.SerializerSettings.ContractResolver = new DefaultContractResolver()
                    {
                        NamingStrategy = null
                    };
                });

            // api user claim policy
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiUser", policy => 
                  policy.RequireClaim(Constants.Strings.JwtClaimIdentifiers.Rol, Constants.Strings.JwtClaims.ApiAccess)
                );
            });
            
            //services.AddMvc(options =>
            //{
            //    var policy = new AuthorizationPolicyBuilder()
            //        .RequireAuthenticatedUser()
            //        .Build();
            //    options.Filters.Add(new AuthorizeFilter(policy));
            //})
            //.AddRazorPagesOptions(options =>
            //{
            //    options.Conventions.AllowAnonymousToFolder("/Account");
            //}).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins", builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .AllowAnyMethod()
                        .AllowAnyOrigin();
                });
            });
            ////.WithOrigins();
            ////"https://localhost:44376/"
            ////"https://lylesrvchecklist.azurewebsites.net",
            ////"https://localhost:5001/",
            ////"http://localhost:5000/",
            ////"https://localhost:44381/",
            ////"http://localhost:53027"
            ////);

            //// var connectionString = Configuration.GetSection("Azure__SignalR_ConnectionString").Value;
            //// var connectionString = Configuration.GetSection("Azure__SignalR_ConnectionString").Value;
            services.AddSignalR().AddAzureSignalR(settings.SignalRHub);
            //services.AddSignalR();

            //In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            //if (settings != null)
            //{
            //    services.AddSignalR().AddAzureSignalR(settings.SignalRHub);
            //}

            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new Info { Version = "v1", Title = "TimeCrunch API", Description = "API's for the TimeCrunch App" });

            //    //// Set the comments path for the Swagger JSON and UI.
            //    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            //    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            //    c.IncludeXmlComments(xmlPath);
            //    c.IgnoreObsoleteActions();
            //    c.IgnoreObsoleteProperties();
            //    c.DescribeAllEnumsAsStrings();
            //});


            services.AddSingleton<IJwtFactory, JwtFactory>();

            // jwt wire up
            // Get options from app settings
            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));

            // Configure JwtIssuerOptions
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
            });

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],

                ValidateAudience = true,
                ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _signingKey,

                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(configureOptions =>
            {
                configureOptions.ClaimsIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                configureOptions.TokenValidationParameters = tokenValidationParameters;
                configureOptions.SaveToken = true;
            });

            // api user claim policy
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiUser", policy => policy.RequireClaim(Constants.Strings.JwtClaimIdentifiers.Rol, Constants.Strings.JwtClaims.ApiAccess));
            });

            // add identity
            var identityBuilder = services.AddIdentityCore<AppUser>(o =>
            {
                // configure identity options
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 6;
            });
            identityBuilder = new IdentityBuilder(identityBuilder.UserType, typeof(IdentityRole), identityBuilder.Services);
            identityBuilder.AddEntityFrameworkStores<ProjectEntities>().AddDefaultTokenProviders();

            services.AddAutoMapper();


        }
        public virtual void ConfigureDatabase(IServiceCollection services)
        {
            var connection = Configuration.GetConnectionString("ProjectEntities");
            services.AddDbContext<ProjectEntities>(options => options.UseSqlServer(connection));
        }
        ////public virtual void ConfigureAuthentication(IServiceCollection services, AppSettings settings)
        ////{
        ////    services.AddAuthentication(sharedOptions =>
        ////    {
        ////        sharedOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        ////        sharedOptions.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
        ////    })
        ////    .AddAzureAd(options => Configuration.Bind("AzureAd", options))
        ////    .AddCookie();

        ////    AzureAdLuppesAdminGroupPolicy.GroupId = (settings != null) ? settings.AdminGroupId : Guid.NewGuid().ToString();
        ////    services.AddAuthorization(options =>
        ////    {
        ////        options.AddPolicy(AzureAdLuppesAdminGroupPolicy.Name, AzureAdLuppesAdminGroupPolicy.Build);
        ////    });

        ////    services.Configure<CookiePolicyOptions>(options =>
        ////    {
        ////        // This lambda determines whether user consent for non-essential cookies is needed for a given request.
        ////        options.CheckConsentNeeded = context => true;
        ////        options.MinimumSameSitePolicy = SameSiteMode.None;
        ////    });
        ////}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(
                    builder =>
                    {
                        builder.Run(
                        async context =>
                                {
                                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                    context.Response.Headers.Add("Access-Control-Allow-Origin", "*");

                                    var error = context.Features.Get<IExceptionHandlerFeature>();
                                    if (error != null)
                                    {
                                        context.Response.AddApplicationError(error.Error.Message);
                                        await context.Response.WriteAsync(error.Error.Message).ConfigureAwait(false);
                                    }
                                });
                    });

                ////app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseAuthentication();

            //// global policy - assign here or on each controller
            app.UseCors("AllowAllOrigins");
            ////app.UseCors("AllowSpecificOrigins");

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501
                spa.Options.SourcePath = "ClientApp";
                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });

            //try
            //{
            //app.UseAzureSignalR(routes =>
            //{
            //    routes.MapHub<ActionHub>("/hub");
            //});
            //app.UseSignalR(routes =>
            //{
            //    routes.MapHub<ActionHub>("/hub");
            //});
            app.UseAzureSignalR(routes =>
            {
                routes.MapHub<CheckListApp.Chat>("/chat");
            });

            //app.UseSignalR(routes =>
            //    {
            //        routes.MapHub<CheckListApp.Chat>("/Chat");
            //    });

            //}
            //catch
            //{
            //}

            //app.UseSwagger();
            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TimeCrunch API V1");
            //});
        }
    }
}
