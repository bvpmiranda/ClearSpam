using ClearSpam.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace ClearSpam.Web.Infrastructure
{
    public class ClearSpamConfigurations : IClearSpamConfigurations
    {
        public long RequestSLA { get; private set; }
        public int PeriodInSeconds { get; private set; }

        public ClearSpamConfigurations()
        {
            var settings = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            RequestSLA = long.Parse(settings["RequestSLA"]);
            PeriodInSeconds = int.Parse(settings["PeriodInSeconds"]);
        }
    }
}
