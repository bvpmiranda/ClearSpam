using ClearSpam.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ClearSpam
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("appsettings.json", true);
            configurationBuilder.AddJsonFile("appsettings.Development.json", true);
            configurationBuilder.AddJsonFile("appsettings.Release.json", true);
            configurationBuilder.AddEnvironmentVariables();
            IConfigurationRoot configuration = configurationBuilder.Build();

            // Register the configuration as a service
            builder.Services.AddSingleton<IConfiguration>(configuration);

            // Add services to the container.
            builder.Services.AddScoped<IClearSpamService, ClearSpamService>();
            builder.Services.AddScoped<ICryptography, Cryptography>();
            builder.Services.AddScoped<IImapService, ImapService>();

            builder.Services.AddLogging(c => c.AddConsole());

            var connectionString = configuration.GetConnectionString("ClearSpam");
            builder.Services.AddDbContext<ClearSpamContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ClearSpamContext>();
            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}