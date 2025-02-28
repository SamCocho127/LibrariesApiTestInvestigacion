using LearningService.WebAPI.Data;
using LearningService.WebAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace LearningService.WebAPI;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        // Add support for Dependency Injection for internal services (StudentsService and LearningService)
        services.AddScoped<IActivitiesService, ActivitiesService>();
        services.AddScoped<IStudentsService,  StudentsService>();

        services.AddCors(option =>
        {
            option.AddPolicy("AllowLocalHost", builder => {
                builder.WithOrigins(new string[] { "http://localhost:5173", "https://localhost:5173" })
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });

        });

        // Add connection string from appsettings.json
        services.AddDbContext<StudentsContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

        services.AddControllers();

        //Add Swagger generation
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "LearningService API",
                Version = "v1",
                Description = "A simple example ASP.NET Core Web API for LearningService"
            });
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui, specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "LearningService API v1");
            });

            // Enable CORS for localhost during development
            app.UseCors("AllowLocalHost");
        }

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
