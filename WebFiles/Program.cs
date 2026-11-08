using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using WebFiles.Data;
using WebFiles.Domain.Entities;
using WebFiles.Services;
using WebFiles.Services.Abstract;

namespace WebFiles;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllersWithViews();

        builder.Services.AddDbContext<ApplicationDbContext>();

        builder.Services.AddIdentity<User, IdentityRole>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        builder.Services.AddAuthentication();
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminAccess", policy => { policy.RequireClaim("Administrator"); });
        });

        builder.Services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 1;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredUniqueChars = 0;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
        });

        builder.Services.Configure<FormOptions>(x =>
        {
            x.MultipartBodyLengthLimit = long.MaxValue;
        });

        builder.Services.Configure<KestrelServerOptions>(x =>
        {
            x.Limits.MaxRequestBodySize = null;     //Allow any size of file, size'll be customized in controllers
            x.Limits.KeepAliveTimeout = TimeSpan.MaxValue;
        });

        builder.Services.AddScoped<IFileService, FileService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<INoteService, NoteService>();
        builder.Services.AddScoped<IPageService, PageService>();
        builder.Services.AddSingleton<IStorageService, StorageService>();

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                await IdentityInitializer.InitializeAsync(userManager, roleManager, config);
                Console.WriteLine("Identity initialized succesfully");
            }
            catch (Exception)
            {
                Console.WriteLine("An error occuried during Identity initialization.");
            }
        }

        app.UseStatusCodePagesWithReExecute("/error", "?code={0}");

        app.UseStaticFiles();

        app.MapControllerRoute(
            "Admin",
            "{area:exists}/{controller=Home}/{action=Index}/{id?}");

        app.MapControllerRoute(
            "default",
            "{controller=Home}/{action=Index}/{id?}");

        app.UseAuthentication();
        app.UseAuthorization();

        app.Run();
    }
}