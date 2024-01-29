using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Minio;
using movies_service.Data;
using movies_service.Interfaces;
using movies_service.Middlewares;
using movies_service.Repositories;
using movies_service.Seed;
using movies_service.Services;

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
builder.Services.AddScoped<IMoviesRepository, MoviesRepository>();
builder.Services.AddScoped<IImagesService, ImagesService>();
builder.Services.AddScoped<IImagesRepository, ImagesRepository>();

builder.Services.AddMinio(configureClient => configureClient
    .WithEndpoint(builder.Configuration["MinIOEndpoint"])
    .WithCredentials(builder.Configuration["MinIOAccessKey"], builder.Configuration["MinIOSecretKey"])
    .WithSSL(false)
);

builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Movies API",
        Description = "Movies API.",
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
            await context.Database.ExecuteSqlRawAsync("DELETE FROM movies");
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