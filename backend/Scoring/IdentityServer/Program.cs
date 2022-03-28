using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace IntelART.IdentityServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseEnvironment(GetEnvironment())
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseApplicationInsights()
                .Build();

            host.Run();
        }

        private static string GetEnvironment()
        {
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (string.IsNullOrEmpty(environment))
            {
#if LOCALDEBUG
                environment = "DevelopmentLocal";
#elif STAGING
                environment = EnvironmentName.Staging;
#elif DEBUG
                environment = EnvironmentName.Development;
#else
                environment = EnvironmentName.Production;
#endif
            }

            return environment;
        }
    }
}
