using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Serialization;
using IntelART.Utilities;
using IntelART.Utilities.DocumentStore;
using IntelART.Utilities.PrintableFormGenerator;
using IntelART.Communication;
using IntelART.Ameria.Communication;
using IntelART.IdentityManagement;

namespace IntelART.Ameria.LoanApplicationRestApi
{
    public class Startup
    {
        private string uploadPath;

        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            this.uploadPath = System.IO.Path.Combine(env.ContentRootPath, "uploads");
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            IConfigurationSection corsConfig = Configuration.GetSection("Cors");

            if (corsConfig != null)
            {
                IEnumerable<string> origins = corsConfig.GetSection("Origins").AsEnumerable().Where(t => t.Value != null).Select(t => t.Value);
                services.AddCors(options =>
                {
                    options.AddPolicy("default", policy =>
                    {
                        policy.WithOrigins(origins.ToArray())
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
                });
            }

            services.AddMvcCore()
                .AddAuthorization()
                .AddJsonFormatters();

            services.AddSingleton(Configuration);

            IDocumentStore documentStore = new FileSystemDocumentStore(this.uploadPath);
            Dictionary<string, string> printableFormTemplates = new Dictionary<string, string>();
            printableFormTemplates["DOC_SCORING_REQUEST_AGREEMENT"] = "NorqAcraAgreement.html";
            printableFormTemplates["DOC_ARBITRAGE_AGREEMENT"] = "Arbitrage.html";
            printableFormTemplates["DOC_INDIVIDUAL_SHEET"] = "IndividualSheet.html";
            printableFormTemplates["DOC_CONSUMER_LOAN_CONTRACT"] = "ContractConsumerLoan.html";
            printableFormTemplates["DOC_OVERDRAFT_ANUITY_CONTRACT"] = "ContractOverdraftAnuity.html";
            printableFormTemplates["DOC_REVOLVING_OVERDRAFT_CONTRACT"] = "ContractRevolvingOverdraft.html";
            printableFormTemplates["DOC_REVOLVING_CREDIT_LINE_CARD_CONTRACT"] = "ContractRevolvingCreditLineCard.html";

            printableFormTemplates["DOC_CONSUMER_LOAN_INDIVIDUAL_SHEET"] = "IndividualSheetConsumerLoan.html";
            printableFormTemplates["DOC_OVERDRAFT_ANUITY_INDIVIDUAL_SHEET"] = "IndividualSheetOverdraftAnuity.html";
            printableFormTemplates["DOC_REVOLVING_OVERDRAFT_INDIVIDUAL_SHEET"] = "IndividualSheetOverdraft.html";
            printableFormTemplates["DOC_REVOLVING_CREDIT_LINE_CARD_INDIVIDUAL_SHEET"] = "IndividualSheetCreditLine.html";
            IPrintableFormGenerator printableFormGenerator = new SimplePrintableFormGenerator("templates", printableFormTemplates);

            ISmsSender smsSender = new AmeriaDbSmsSender(Configuration.GetSection("ConnectionStrings")["ScoringDB"]);

            services.AddTransient((sp) => smsSender);
            services.AddTransient((sp) => documentStore);
            services.AddTransient((sp) => printableFormGenerator);

            services.AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                });

            IMembershipProvider membershipProvider = new SqlMembershipProvider.SqlMembershipProvider(Configuration.GetSection("ConnectionStrings")["ScoringDB"]);

            services.AddTransient<IMembershipProvider>((sp) => membershipProvider);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("default");

            app.UseExceptionHandler(options =>
            {
                options.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    var ex = context.Features.Get<IExceptionHandlerFeature>();
                    if (ex != null)
                    {
                        ErrorInfo errorInfo = ErrorInfo.For(ex.Error);
                        string error = Newtonsoft.Json.JsonConvert.SerializeObject(errorInfo);
                        await context.Response.WriteAsync(error).ConfigureAwait(false);
                    }
                });
            });

            string authServiceUrl = Configuration.GetSection("AutenticationService")["Url"];
            app.UseIdentityServerAuthentication(new IdentityServerAuthenticationOptions
            {
                Authority = authServiceUrl,
                AllowedScopes = { "loanApplicationApi" },
                RequireHttpsMetadata = false,
                AutomaticChallenge = true,
                AutomaticAuthenticate = true
            });

            app.UseMvc();
        }
    }
}
