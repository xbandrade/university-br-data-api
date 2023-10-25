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
        services.AddDbContext<UniversityDBContext>();
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddAuthorization();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        Console.WriteLine("Configuring Startup");
        // string uniAPIUrl = "http://universities.hipolabs.com/search?name=&country=brazil";
        // string relativePath = @".\Env\ConnectionString.txt";
        // string fullPath = Path.Combine(Environment.CurrentDirectory, relativePath);
        // string connectionString = File.ReadAllText(fullPath);
        // var dataPopulator = new DataPopulator(uniAPIUrl, connectionString);
        // await dataPopulator.PopulateDatabase();
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
