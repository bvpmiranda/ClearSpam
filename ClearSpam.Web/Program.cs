using ClearSpam.SqlMigrations;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace ClearSpam.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (var context = new ContextFactory().CreateDbContext())
            {
                ClearSpamMigrations.ApplyMigrations(context);
            }

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                          .UseStartup<Startup>();
        }
    }
}
