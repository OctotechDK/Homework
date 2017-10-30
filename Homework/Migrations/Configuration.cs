namespace Homework.Migrations
{
    using Models;
    using Microsoft.AspNet.Identity;
    using System;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "Homework.Models.ApplicationDbContext";
        }

        protected override void Seed(ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.
            var passwordHasher = new PasswordHasher();
            var hashedPassword = passwordHasher.HashPassword("UnicornsRule!");
            context.Users.AddOrUpdate(u => u.UserName,
                new ApplicationUser
                {
                    UserName = "nh@umbraco.com",
                    Email = "nh@umbraco.com",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    PasswordHash = hashedPassword
                });
            context.Users.AddOrUpdate(u => u.UserName,
                new ApplicationUser
                {
                    UserName = "sneum@umbraco.com",
                    Email = "sneum@umbraco.com",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    PasswordHash = hashedPassword
                });

            SeedProductSerialNumbers(context);
        }

        private static void SeedProductSerialNumbers(ApplicationDbContext context)
        {
            for (var i = 1; i <= 100; i++)
            {
                if (Guid.TryParse(i.ToString().PadLeft(32, '0'), out Guid guid))
                {
                    context.DrawSerialNumbers.AddOrUpdate(id => id.Id, new DrawSerialNumberModel
                    {
                        Id = guid
                    });
                }
            }
        }
    }
}
