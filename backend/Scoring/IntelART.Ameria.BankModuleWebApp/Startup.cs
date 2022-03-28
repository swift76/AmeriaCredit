using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using IntelART.WebApiRequestProxy;

namespace IntelART.Ameria.BankModuleWebApp
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore()
                .AddAuthorization()
                .AddJsonFormatters();

            services.AddSingleton(Configuration);

            services.AddMvc()
               .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
               .AddJsonOptions(options =>
               {
                   options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
               });

            string authority = Configuration.GetSection("Authentication")["Authority"];

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
                .AddCookie("Cookies", 
                options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
                    options.SlidingExpiration = true;
                })
                .AddOpenIdConnect("oidc", options => new OpenIdConnectOptions
                {
                    SignInScheme = "Cookies",
                    Authority = authority,
                    ClientId = "bankApplication",
                    ClientSecret = "secret",
                    ResponseType = "code id_token",
                    RequireHttpsMetadata = false, // TODO: Change to 'true', leave as 'false' only in development mode
                    GetClaimsFromUserInfoEndpoint = true,
                    SaveTokens = true,
                    Scope = {
                    "bankInternalApi",
                    "offline_access",
                },
                    TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                    {
                        NameClaimType = "name",
                        RoleClaimType = "role",
                    }
                });

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));

            app.UseAuthentication();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            IConfigurationSection requestProxyPolicies = Configuration.GetSection("RequestProxy").GetSection("Policies");
            IConfigurationSection requestProxyPolicy = requestProxyPolicies.GetSection("0");

            PathString bankApiPathPrefix = new PathString(requestProxyPolicy["LocalPath"]);
            string bankApiForwardUrlBase = requestProxyPolicy["RemoteUrlBase"];

            app.UseWebApiRequestProxy()
                .AddRule(
                    new SimleWebApiRequestProxyRule(
                        bankApiPathPrefix,
                        bankApiForwardUrlBase,
                        async (r, c) =>
                        {
                            AuthenticateResult info = await r.HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                            string accessToken;
                            if (info != null
                                && info.Properties != null
                                && info.Properties.Items != null
                                && info.Properties.Items.TryGetValue(".Token.access_token", out accessToken))
                            {
                                c.SetBearerToken(accessToken);
                            }
                        })
                );

            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsEnvironment("DevelopmentLocal") || env.IsEnvironment("Development"))
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
