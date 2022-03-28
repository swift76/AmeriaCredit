using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using IdentityServer4.Services;
using IntelART.IdentityManagement;
using IntelART.Ameria.SqlMembershipProvider;
using IntelART.Communication;
using IntelART.Ameria.Communication;

namespace IntelART.IdentityServer
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

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddIdentityServer(Config.SetupIdentityServer)
                .AddTemporarySigningCredential()  // Replace with .AddSigningCredential()
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryClients(Config.GetClients(Configuration.GetSection("ClientApplications")))
                .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
                ;

            IMembershipProvider membershipProvider = new SqlMembershipProvider(Configuration.GetSection("ConnectionStrings")["MembershipDB"]);

            services.AddTransient<IUserStore>((sp) => membershipProvider);
            services.AddTransient<IMembershipProvider>((sp) => membershipProvider);

            services.AddTransient<IProfileService, ProfileService>();

            IEmailSender emailSender;
            if (Configuration["EmailService"] == "Ameria")
            {
                emailSender = new AmeriaDbEmailSender(Configuration.GetSection("ConnectionStrings")["MembershipDB"]);
            }
            else
            {
                emailSender = new SmtpEmailSender("smtp.sendgrid.net", 465, "apikey", "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", "support@inchvorban.com", true);
            }

            services.AddTransient<IEmailSender>((sp) => emailSender);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseIdentityServer();

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}
