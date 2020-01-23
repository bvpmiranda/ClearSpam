using ClearSpam.Application.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace ClearSpam.Web.Infrastructure
{
    public class ClearSpamConfigurations : IClearSpamConfigurations
    {
        public string ConnectionString { get; private set; }
        public long RequestSLA { get; private set; }
        public int PeriodInSeconds { get; private set; }

        public ClearSpamConfigurations(IHostingEnvironment env)
        {
            var appsettingsFileName = GetAppSettingsFileName(env);

            var settings = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(appsettingsFileName)
                .Build();

            ConnectionString = settings.GetConnectionString("ClearSpam");
            RequestSLA = long.Parse(settings["RequestSLA"]);
            PeriodInSeconds = int.Parse(settings["PeriodInSeconds"]);
        }

        private string GetAppSettingsFileName(IHostingEnvironment env)
        {
            var appsettingsFileName = "appsettings";
            
            if (!env.IsDevelopment())
                return $"{appsettingsFileName}.release.json";

            var basePath = Directory.GetCurrentDirectory();
            var machineName = System.Environment.MachineName.ToLowerInvariant();

            return File.Exists(Path.Combine(basePath, $"{appsettingsFileName}.{machineName}.json"))
                ? $"{appsettingsFileName}.{machineName}.json"
                : $"{appsettingsFileName}.json";
        }
    }
}
