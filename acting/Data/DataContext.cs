using acting.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace acting.Data;

/// <summary>
/// Data context.
/// </summary>
/// <param name="options">Database context options.</param>
public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    /// <summary>
    /// Acting.
    /// </summary>
    public DbSet<Acting> Acting { get; set; } = default!;
}