using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using IntelART.WebApiRequestProxy;
using System.Net.Http;
using Microsoft.AspNetCore.Http.Authentication;

namespace IntelART.Ameria.CustomerModuleWebApp
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

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddAuthentication("Cookies")
                .AddCookie(o =>
                {
                    o.ExpireTimeSpan = TimeSpan.FromMinutes(10);
                    o.SlidingExpiration = true;
                    o.Events = new CookieAuthenticationEvents()
                    {
                        OnRedirectToLogin = ctx =>
                        {
                            if (ctx.Request.Path.StartsWithSegments(new PathString("/api")))
                            {
                                ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            }
                            else
                            {
                                ctx.Response.Redirect(ctx.RedirectUri);
                            }
                            return Task.FromResult<object>(null);
                        }
                    };
                });

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });
        }

        [Obsolete]
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseAuthentication();

            IConfigurationSection requestProxyPolicies = Configuration.GetSection("RequestProxy").GetSection("Policies");
            IConfigurationSection requestProxyPolicy1 = requestProxyPolicies.GetSection("0");
            IConfigurationSection requestProxyPolicy2 = requestProxyPolicies.GetSection("1");

            PathString customerApiPathPrefix = new PathString(requestProxyPolicy1["LocalPath"]);
            string customerApiForwardUrlBase = requestProxyPolicy1["RemoteUrlBase"];

            PathString loanApiPathPrefix = new PathString(requestProxyPolicy2["LocalPath"]);
            string loanApiForwardUrlBase = requestProxyPolicy2["RemoteUrlBase"];

            app.UseWebApiRequestProxy()
                .AddRule(
                    new SimleWebApiRequestProxyRule(
                        customerApiPathPrefix,
                        customerApiForwardUrlBase,
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
                )
                .AddRule(
                    new SimleWebApiRequestProxyRule(
                        loanApiPathPrefix,
                        loanApiForwardUrlBase,
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
