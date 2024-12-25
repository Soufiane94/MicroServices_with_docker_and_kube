using Microsoft.EntityFrameworkCore;
using PlatFormService.Models;

namespace PlatFormService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(WebApplication app, bool IsProduction)
        {
            using var serviceScoped = app.Services.CreateScope();
            SeedData(serviceScoped.ServiceProvider.GetService<AppDbContext>(), IsProduction);
        }

        private static void SeedData(AppDbContext context, bool IsProduction)
        {
            if (IsProduction)
            {
                Console.WriteLine("--> Attempting to apply migrations...");
                try
                {
                    context.Database.Migrate();
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine($"Could not run migrations:{ex.Message}");
                }
            }
            if (!context.Platforms.Any())
            {
                Console.WriteLine("--> Seeding Data...");
                context.Platforms.AddRange(
                    new Platform()
                    {
                        Name = "Dot Net",
                        Publisher = "Microsoft",
                        Cost = "Free"
                    },
                       new Platform()
                       {
                           Name = "Sql Server",
                           Publisher = "Microsoft",
                           Cost = "Free"
                       },
                       new Platform()
                       {
                           Name = "Kubernetes",
                           Publisher = "Cloud Native Computing Foundation",
                           Cost = "Free"
                       }
                );
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("--> We already have data");
            }
        }
    }
}