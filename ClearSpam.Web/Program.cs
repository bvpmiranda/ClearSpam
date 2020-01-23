using ClearSpam.Application.Interfaces;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace ClearSpam.Web
{
    public class Program
    {
        public static IClearSpamService ClearSpamService;

        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
