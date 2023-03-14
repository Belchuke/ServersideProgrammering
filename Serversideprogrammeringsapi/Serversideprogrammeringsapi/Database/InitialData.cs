using Microsoft.AspNetCore.Identity;
using Serversideprogrammeringsapi.Database.Models;
using Serversideprogrammeringsapi.Env;
using Serversideprogrammeringsapi.Identity.Properties;

namespace Serversideprogrammeringsapi.Database
{
    public static class InitialData
    {
        public static async Task PopulateTestData(WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            ApiDbContext context = scope.ServiceProvider.GetRequiredService<ApiDbContext>();

            ToDoDbContext todoContext = scope.ServiceProvider.GetRequiredService<ToDoDbContext>();

            UserManager<ApiUser> userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApiUser>>();

            RoleManager<ApiRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApiRole>>();
           

            if (!context.Users.Any())
            {
                Func<ApiUser, Task> AddAdminAccount = async (ApiUser user) =>
                {
                    await userMgr.CreateAsync(user, EnvHandler.GetAdminPas());

                    await userMgr.AddToRolesAsync(user, new string[] { AuthRoles.Admin});
                };

                Func<ApiUser, Task> AddNormalUser = async (ApiUser user) =>
                {
                    await userMgr.CreateAsync(user, "312PassExample");
                };

                if (!await roleManager.RoleExistsAsync(AuthRoles.Admin))
                {
                    await roleManager.CreateAsync(new ApiRole() { Name = AuthRoles.Admin });
                }

                await AddAdminAccount(new ApiUser()
                {
                    UserName = "jcb.a.belchuke@gmail.com",
                    Email = "jcb.a.belchuke@gmail.com",
                    EmailConfirmed = true,
                    TwoFactorEnabled = true,
                    IsEnabled = true,
                });

                await AddNormalUser(new ApiUser()
                {
                    UserName = "spaggetiecodepastamaker@gmail.com",
                    Email = "spaggetiecodepastamaker@gmail.com",
                    EmailConfirmed = true,
                    TwoFactorEnabled = true,
                });
            }
        }
    }
}
