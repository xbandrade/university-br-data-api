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
        Console.WriteLine("-> MySQLContext.OnConfiguring");
        string relativePath = @".\Env\ConnectionString.txt";
        string fullPath = Path.Combine(Environment.CurrentDirectory, relativePath);
        string connectionString = File.ReadAllText(fullPath);
        optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), null);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        Console.WriteLine("-> MySQLContext.OnModelCreating");
        modelBuilder.Entity<BrUniversity>().ToTable("University");
        modelBuilder.Entity<BrUniversity>().HasKey(u => u.Id);
        base.OnModelCreating(modelBuilder);
    }

}
