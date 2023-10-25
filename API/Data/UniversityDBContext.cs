using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace UniversityBRDataAPI;

public class UniversityDBContext : DbContext
{
    public DbSet<BrUniversity> Universities { get; set; }

    public UniversityDBContext()
    {
        Universities = Set<BrUniversity>();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string connectionString = "Server=db4free.net;Port=3306;Database=bruniapi;User=bruniapi;Password=P@ssw0rd;";
        // string connectionString = "server=db;port=3306;uid=bruniapi;pwd=P@ssw0rd;database=bruniapi";
        optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), null);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BrUniversity>().ToTable("University");
        modelBuilder.Entity<BrUniversity>().HasKey(u => u.Id);
        base.OnModelCreating(modelBuilder);
    }
}
