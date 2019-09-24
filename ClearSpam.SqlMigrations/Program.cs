using ClearSpam.Persistence;
using System;

namespace ClearSpam.SqlMigrations
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new ContextFactory().CreateDbContext())
            {
                Console.WriteLine("Applying ClearSpam Migrations");
                ClearSpamMigrations.ApplyMigrations(context);
                Console.WriteLine("ClearSpam Migrations Applied");
            }

            Console.ReadKey();
        }
    }
}