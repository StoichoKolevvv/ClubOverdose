using ClubOverdose;
using ClubOverdose.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace ClubOverdose.Services
{
    public static class ApplicationBuilderExtension
    {
        private const string SuperAdminRoleName = "SuperAdmin";
        private const string AdminRoleName = "Admin";
        private const string ClientRoleName = "Client";
        private const string GuestRoleName = "Guest";

        private const string SuperAdminUserName = "superadmin";
        private const string SuperAdminEmail = "superadmin@gmail.com";
        private const string SuperAdminPassword = "123!@#Qwe";
        private const string SuperAdminFirstName = "Cgn";
        private const string SuperAdminLastName = "Cgn";
        private const string SuperAdminPhoneNumber = "0899999999";

        public static async Task<IApplicationBuilder> PrepareDataBase(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();

            var services = scope.ServiceProvider;

            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            try
            {
                var context = services.GetRequiredService<ApplicationDbContext>();
                var userManager = services.GetRequiredService<UserManager<Client>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                context.Database.Migrate();
                //Sazdavane na roles
                await SeedRolesAsync(roleManager);
                //sazdavane na SUPER ADMIN s vsi4kite mu roli
                await SeedSuperAdminAsync(userManager);
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                try
                {
                    logger.LogError(ex, "An error occurred migrating or seeding the DB.");
                }
                catch
                {
                    // Preserve the original database startup failure if a logging provider is unavailable.
                }

                throw;
            }

            return app;
        }
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            var roles = new[] { SuperAdminRoleName, AdminRoleName, ClientRoleName, GuestRoleName };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        public static async Task SeedSuperAdminAsync(UserManager<Client> userManager)
        {
            //Seed Default User
            var defaultUser = new Client
            {
                UserName = SuperAdminUserName,
                Email = SuperAdminEmail,
                FirstName = SuperAdminFirstName,
                LastName = SuperAdminLastName,
                PhoneNumber = SuperAdminPhoneNumber,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            var user = await userManager.FindByEmailAsync(defaultUser.Email);
            if (user == null)
            {
                var result = await userManager.CreateAsync(defaultUser, SuperAdminPassword);
                if (result.Succeeded)
                {
                    user = defaultUser;
                }
            }

            if (user != null)
            {
                var superAdminRoles = new[] { SuperAdminRoleName, AdminRoleName };

                foreach (var role in superAdminRoles)
                {
                    if (!await userManager.IsInRoleAsync(user, role))
                    {
                        await userManager.AddToRoleAsync(user, role);
                    }
                }
            }
        }
    }
}
