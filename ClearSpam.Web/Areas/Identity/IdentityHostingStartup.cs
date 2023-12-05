using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(ClearSpam.Web.Areas.Identity.IdentityHostingStartup))]
namespace ClearSpam.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}