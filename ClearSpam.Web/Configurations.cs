using AutoMapper;
using ClearSpam.Application.Accounts.Queries;
using ClearSpam.Application.Infrastructure;
using ClearSpam.Application.Interfaces;
using ClearSpam.Application.Models.Mappings;
using ClearSpam.Application.Services;
using ClearSpam.Domain.Interfaces;
using ClearSpam.Infrastructure;
using ClearSpam.Persistence;
using MediatR;
using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using System.Reflection;

namespace ClearSpam.Web
{
    public static class Configurations
    {
        public static void ConfigureDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureMediatr();
            services.ConfigureDatabaseConnection(configuration);

            services.AddAutoMapper(typeof(AccountMappings).Assembly);

            services.AddSingleton<IClearSpamConfigurations, Infrastructure.ClearSpamConfigurations>();

            services.AddScoped<IRepository, Repository>();
            services.AddSingleton<ICryptography, Cryptography>();
            services.AddSingleton<IImapService, ImapService>();

            services.AddScoped<Application.Interfaces.ILogger, ClearSpam.Infrastructure.Logger>();

            services.AddScoped<NLog.ILogger>(x => LogManager.GetCurrentClassLogger());

            services.AddSingleton<IClearSpamService, ClearSpamService>();
        }

        private static void ConfigureMediatr(this IServiceCollection services)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestLogger<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
            services.AddMediatR(typeof(GetAccountQueryHandler).GetTypeInfo().Assembly);
        }

        private static void ConfigureDatabaseConnection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ClearSpamContext>(options => options.UseSqlServer(ClearSpamContext.GetConnectionString()));
        }
    }
}
