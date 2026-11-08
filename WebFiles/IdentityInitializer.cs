using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using WebFiles.Domain.Entities;

namespace WebFiles;

public class IdentityInitializer
{
    public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager,
        IConfiguration config)
    {
        var adminPassword = config.GetValue<string>("AdminPassword");
        var res = await userManager.FindByNameAsync("Admin");
        var claim = new Claim("Administrator", "1");
        if (res == null)
        {
            var adminUser = new User { UserName = "Admin", AvailableSpace = config.GetValue<long>("DefaultSpace")};

            await userManager.CreateAsync(adminUser, adminPassword);
            await userManager.AddClaimAsync(adminUser, claim);
        }
        else if (!(await userManager.GetClaimsAsync(res)).Any(item => item.Type.Equals("Administrator")))
        {
            await userManager.AddClaimAsync(res, claim);
        }
    }
}