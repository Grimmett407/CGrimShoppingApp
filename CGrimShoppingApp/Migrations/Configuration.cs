namespace CGrimShoppingApp.Migrations
{
    using CGrimShoppingApp.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CGrimShoppingApp.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }
                var userManager = new UserManager<ApplicationUser>(
                    new UserStore<ApplicationUser>(context));
                if (!context.Users.Any(u => u.Email == "cgrimmett407@gmail.com"))
                {
                    userManager.Create(new ApplicationUser         //Creating new user for the application using required fields
                    {
                        UserName = "Christian Grimmett",
                        Email = "cgrimmett407@gmail.com",
                        FirstName = "Christian",
                        LastName = "Grimmett",
                    }, "Chris407!!");
                }
                var userId = userManager.FindByEmail("cgrimmett407@gmail.com").Id;
                userManager.AddToRole(userId, "Admin");
        }
    }
}

