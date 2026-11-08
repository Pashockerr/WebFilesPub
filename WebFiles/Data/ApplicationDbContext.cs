using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebFiles.Domain.Entities;

namespace WebFiles.Data;

public class ApplicationDbContext : IdentityDbContext<User>
{
    private readonly IConfiguration _configuration;

    public ApplicationDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public DbSet<StoredFile> Files { get; set; }
    public DbSet<Note> Notes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies()
            .UseNpgsql(_configuration.GetConnectionString("DefaultConnection"));

        base.OnConfiguring(optionsBuilder);
    }
}