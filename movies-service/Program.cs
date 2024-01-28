using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using movies_service.Data;
using movies_service.Interfaces;
using movies_service.Repositories;
using movies_service.Seed;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);
builder.Services.AddTransient<Seed>();
builder.Services.AddScoped<IMoviesRepository, MoviesRepository>();

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

app.UseHttpsRedirection();

app.MapControllers();

app.Run();