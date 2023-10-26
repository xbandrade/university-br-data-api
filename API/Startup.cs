using Microsoft.OpenApi.Models;

namespace UniversityBRDataAPI;
public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "BrUni API", Version = "v1" });
            c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            c.EnableAnnotations();
        });
        services.AddDbContext<UniversityDBContext>();
        services.AddControllers();
        services.AddEndpointsApiExplorer();   
        services.AddAuthorization();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "/swagger/{documentName}/swagger.json";
            });
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BrUni API v1"));
        }
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();
        app.UseDeveloperExceptionPage();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
