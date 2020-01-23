using ClearSpam.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace ClearSpam.SqlMigrations.Infrastructure
{
    public class Configurations : IClearSpamConfigurations
    {
        public string ConnectionString { get; private set; }
        public long RequestSLA { get; private set; }
        public int PeriodInSeconds { get; private set; }

        public Configurations()
        {
            var appsettingsFileName = GetAppSettingsFileName();

            var settings = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(appsettingsFileName)
                .Build();

            ConnectionString = settings.GetConnectionString("ClearSpam");
        }

        private static string GetAppSettingsFileName()
        {
            var appsettingsFileName = "appsettings";
            
            var basePath = Directory.GetCurrentDirectory();
            var machineName = System.Environment.MachineName.ToLowerInvariant();

            return File.Exists(Path.Combine(basePath, $"{appsettingsFileName}.{machineName}.json"))
                ? $"{appsettingsFileName}.{machineName}.json"
                : $"{appsettingsFileName}.json";
        }
    }
}
