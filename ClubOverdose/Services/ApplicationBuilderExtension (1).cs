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
                await SeedDemoDataAsync(context);
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

        private static async Task SeedDemoDataAsync(ApplicationDbContext context)
        {
            var menuNames = await context.Menus.Select(m => m.Name).ToListAsync();
            var demoMenus = new[]
            {
                new Menu { Name = "Main Menu" },
                new Menu { Name = "VIP Menu" }
            };

            context.Menus.AddRange(demoMenus.Where(menu => !menuNames.Contains(menu.Name)));
            await context.SaveChangesAsync();

            var typeNames = await context.Types.Select(t => t.Name).ToListAsync();
            var demoTypes = new[]
            {
                new ClubOverdose.Data.Type { Name = "Cocktails" },
                new ClubOverdose.Data.Type { Name = "Soft Drinks" },
                new ClubOverdose.Data.Type { Name = "Alcohol" },
                new ClubOverdose.Data.Type { Name = "Shots" }
            };

            context.Types.AddRange(demoTypes.Where(type => !typeNames.Contains(type.Name)));
            await context.SaveChangesAsync();

            var eventNames = await context.Events.Select(e => e.Name).ToListAsync();
            var demoEvents = new[]
            {
                new Event
                {
                    Name = "Neon Friday",
                    ImageUrl = "https://images.unsplash.com/photo-1492684223066-81342ee5ff30?auto=format&fit=crop&w=1200&q=80",
                    Description = "Category: DJ Night. Entry: 15 lv. Promo: Free welcome shot before midnight. Resident DJs, neon lights and high-energy club sets.",
                    EventDateTime = DateTime.Today.AddDays(7).AddHours(22),
                    LastUpdatedDate = DateTime.Today
                },
                new Event
                {
                    Name = "VIP Retro Night",
                    ImageUrl = "https://images.unsplash.com/photo-1506157786151-b8491531f063?auto=format&fit=crop&w=1200&q=80",
                    Description = "Category: Retro Party. Entry: 20 lv. Promo: VIP tables include a complimentary bottle mixer. Classic hits and premium lounge energy.",
                    EventDateTime = DateTime.Today.AddDays(14).AddHours(22),
                    LastUpdatedDate = DateTime.Today
                },
                new Event
                {
                    Name = "Midnight Beats",
                    ImageUrl = "https://images.unsplash.com/photo-1516450360452-9312f5e86fc7?auto=format&fit=crop&w=1200&q=80",
                    Description = "Category: Guest Artist. Entry: 25 lv. Promo: Early reservations get priority entrance. Guest performers and midnight dance sets.",
                    EventDateTime = DateTime.Today.AddDays(21).AddHours(23),
                    LastUpdatedDate = DateTime.Today
                }
            };

            context.Events.AddRange(demoEvents.Where(demoEvent => !eventNames.Contains(demoEvent.Name)));
            await context.SaveChangesAsync();

            var mainMenu = await context.Menus.FirstAsync(m => m.Name == "Main Menu");
            var vipMenu = await context.Menus.FirstAsync(m => m.Name == "VIP Menu");
            var cocktails = await context.Types.FirstAsync(t => t.Name == "Cocktails");
            var softDrinks = await context.Types.FirstAsync(t => t.Name == "Soft Drinks");
            var alcohol = await context.Types.FirstAsync(t => t.Name == "Alcohol");
            var shots = await context.Types.FirstAsync(t => t.Name == "Shots");

            var existingDrinkNames = await context.Drinks.Select(d => d.Name).ToListAsync();
            var demoDrinks = new[]
            {
                new Drink
                {
                    Name = "Mojito",
                    MenuId = mainMenu.Id,
                    TypeId = cocktails.Id,
                    Price = 12,
                    Volume = 300,
                    DateAdded = DateTime.Today
                },
                new Drink
                {
                    Name = "Cola",
                    MenuId = mainMenu.Id,
                    TypeId = softDrinks.Id,
                    Price = 4,
                    Volume = 330,
                    DateAdded = DateTime.Today
                },
                new Drink
                {
                    Name = "Whiskey",
                    MenuId = vipMenu.Id,
                    TypeId = alcohol.Id,
                    Price = 18,
                    Volume = 50,
                    DateAdded = DateTime.Today
                },
                new Drink
                {
                    Name = "Tequila Shot",
                    MenuId = mainMenu.Id,
                    TypeId = shots.Id,
                    Price = 7,
                    Volume = 40,
                    DateAdded = DateTime.Today
                },
                new Drink
                {
                    Name = "Champagne Bottle",
                    MenuId = vipMenu.Id,
                    TypeId = alcohol.Id,
                    Price = 120,
                    Volume = 750,
                    DateAdded = DateTime.Today
                }
            };

            context.Drinks.AddRange(demoDrinks.Where(drink => !existingDrinkNames.Contains(drink.Name)));
            await context.SaveChangesAsync();
        }
    }
}
