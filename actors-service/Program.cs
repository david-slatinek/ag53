using System.Reflection;
using actors_service.Data;
using actors_service.Interfaces;
using actors_service.Middlewares;
using actors_service.Repositories;
using actors_service.Seed;
using actors_service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddResponseCaching();
builder.Services.AddControllers(options =>
{
    options.CacheProfiles.Add("Default",
        new CacheProfile
        {
            Duration = 60,
            Location = ResponseCacheLocation.Any
        }
    );
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);
builder.Services.AddTransient<Seed>();
builder.Services.AddScoped<IActorsRepository, ActorsRepository>();
builder.Services.AddScoped<IMoviesService, MoviesService>();

builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Actors API",
        Description = "Actors API.",
        Contact = new OpenApiContact
        {
            Name = "David Slatinek",
            Url = new Uri("https://github.com/david-slatinek")
        },
        License = new OpenApiLicense
        {
            Name = "MIT License",
            Url = new Uri("https://opensource.org/licenses/MIT")
        }
    });

    options.SupportNonNullableReferenceTypes();

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();

if (args.Length > 0)
{
    switch (args[0])
    {
        case "seed":
        {
            using var scope = app.Services.CreateScope();
            var seed = scope.ServiceProvider.GetRequiredService<Seed>();
            await seed.SeedAsync();
            break;
        }

        case "delete":
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<DataContext>();
            await context.Database.ExecuteSqlRawAsync("DELETE FROM actors");
            break;
        }
    }

    return;
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseResponseCaching();

app.UseHttpsRedirection();

app.MapControllers();

app.UseMiddleware<RequestCounter>();

app.Run();