using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MySql.Data.EntityFrameworkCore.Extensions;

namespace UniversityBRDataAPI;
public class Startup
{
    public IConfiguration Configuration { get; }
    // private MySQLContext _mySQLContext;

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
        // _mySQLContext = mySQLContext;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        Console.WriteLine("Configuring Services");
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddAuthorization();
    }

    public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        Console.WriteLine("Configuring Startup");
        string uniAPIUrl = "http://universities.hipolabs.com/search?name=&country=brazil";
        string relativePath = @".\Env\ConnectionString.txt";
        string fullPath = Path.Combine(Environment.CurrentDirectory, relativePath);
        string connectionString = File.ReadAllText(fullPath);
        var dataPopulator = new DataPopulator(uniAPIUrl, connectionString);
        await dataPopulator.PopulateDatabase();

        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
