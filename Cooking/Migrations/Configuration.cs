namespace Cooking.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Threading.Tasks;
    using Cooking.Data;
    using Microsoft.AspNet.Identity.EntityFramework;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected async override void Seed(ApplicationDbContext context)
        {
            await this.AddRoles(context);

            await this.AddAdminUser(context);
        }

        private async Task AddRoles(ApplicationDbContext context)
        {
            context.Roles.AddOrUpdate(
                r => r.Name,
                new IdentityRole(RoleConst.Administrator),
                new IdentityRole(RoleConst.User)
            );

            await context.SaveChangesAsync();
        }

        private async Task AddAdminUser(ApplicationDbContext context)
        {
            var adminUserExist = context.Users.Any(u => u.UserName == UserConst.AdminUsername);
            if (!adminUserExist)
            {
                var adminUser = new ApplicationUser()
                {
                    UserName = UserConst.AdminUsername,
                    Email = UserConst.AdminMail
                };

                var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
                await manager.CreateAsync(adminUser, UserConst.AdminPassword);

                await manager.AddToRoleAsync(adminUser.Id, RoleConst.Administrator);
                await manager.AddToRoleAsync(adminUser.Id, RoleConst.User);
            }
        }
    }
}
