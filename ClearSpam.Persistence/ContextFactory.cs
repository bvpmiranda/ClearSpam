//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Design;
//using Microsoft.Extensions.Configuration;
//using System.IO;

//namespace ClearSpam.Persistence
//{
//    public class ContextFactory : IDesignTimeDbContextFactory<ClearSpamContext>
//    {
//        public ClearSpamContext CreateDbContext()
//        {
//            return CreateDbContext(new[] { "" });
//        }

//        public ClearSpamContext CreateDbContext(string[] args)
//        {
//            var options = GetOptions();
//            var context = new ClearSpamContext(options);

//            return context;
//        }

//        public static DbContextOptions<ClearSpamContext> GetOptions()
//        {
//            var connectionString = GetConnectionString();
//            var contextBuilder = new DbContextOptionsBuilder<ClearSpamContext>()
//                .UseSqlServer(connectionString);

//            return contextBuilder.Options;
//        }

//        public static string GetConnectionString()
//        {
//            var basePath = Directory.GetCurrentDirectory();
//            var machineName = System.Environment.MachineName;
//            var appsettingsFileName = "appsettings";

//            appsettingsFileName = File.Exists(Path.Combine(basePath, $"{appsettingsFileName}.{machineName}.json"))
//                ? $"{appsettingsFileName}.{machineName}.json"
//                : $"{appsettingsFileName}.json";

//            var configurationBuilder = new ConfigurationBuilder()
//                .SetBasePath(basePath)
//                .AddJsonFile(appsettingsFileName);
//            var configuration = configurationBuilder.Build();
//            return configuration.GetConnectionString("ClearSpam");
//        }
//    }
//}
