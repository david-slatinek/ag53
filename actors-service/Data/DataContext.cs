using actors_service.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace actors_service.Data;

/// <summary>
/// Data context.
/// </summary>
/// <param name="options">Database context options.</param>
public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    /// <summary>
    /// Actors.
    /// </summary>
    public DbSet<Actor> Actors { get; set; } = default!;
}