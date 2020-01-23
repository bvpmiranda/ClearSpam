using ClearSpam.SqlMigrations.Infrastructure;
using System;

namespace ClearSpam.SqlMigrations
{
    class Program
    {
        static void Main(string[] args)
        {
            var configurations = new Configurations();
            using (var context = new ContextFactory().CreateDbContext(configurations.ConnectionString))
            {
                Console.WriteLine("Applying ClearSpam Migrations");
                ClearSpamMigrations.ApplyMigrations(context);
                Console.WriteLine("ClearSpam Migrations Applied");
            }

            Console.ReadKey();
        }
    }
}