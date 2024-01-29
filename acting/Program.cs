using System.Reflection;
using acting.Data;
using acting.Interfaces;
using acting.Middlewares;
using acting.Repositories;
using acting.Services;
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
builder.Services.AddScoped<IActingRepository, ActingRepository>();
builder.Services.AddScoped<IActingService, ActingService>();

builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Acting API",
        Description = "Acting API.",
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

app.UseSwagger();
app.UseSwaggerUI();

app.UseResponseCaching();

app.UseHttpsRedirection();

app.MapControllers();

app.UseMiddleware<RequestCounter>();

app.Run();