using Microsoft.EntityFrameworkCore;
using movies_service.Models.Database;

namespace movies_service.Data;

/// <summary>
/// Data context.
/// </summary>
/// <param name="options">Database context options.</param>
public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    /// <summary>
    /// Movies.
    /// </summary>
    public DbSet<Movie> Movies { get; set; } = default!;
}