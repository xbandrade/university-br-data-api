using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace UniversityBRDataAPI;

public class UniversityDBContext : DbContext
{
    public DbSet<BrUniversity> Universities { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        Console.WriteLine("-> MySQLContext.OnConfiguring");
        string relativePath = @"..\Env\ConnectionString.txt";
        string fullPath = Path.Combine(Environment.CurrentDirectory, relativePath);
        string connectionString = File.ReadAllText(fullPath);
        optionsBuilder.UseMySQL(connectionString);
        // var serverVersion = new MySqlServerVersion("8.1.0");
        // optionsBuilder.UseMySql(connectionString, serverVersion);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        Console.WriteLine("-> MySQLContext.OnModelCreating");
        modelBuilder.Entity<BrUniversity>().ToTable("UniversityData");
        modelBuilder.Entity<BrUniversity>().HasKey(u => u.Id);
        base.OnModelCreating(modelBuilder);
    }
}
