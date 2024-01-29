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

    /// <summary>
    /// Images.
    /// </summary>
    public DbSet<Image> Images { get; set; } = default!;

    /// <summary>
    /// Configure the data context.
    /// </summary>
    /// <param name="modelBuilder">Model builder.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Movie>()
            .HasMany(movie => movie.Images)
            .WithOne(image => image.Movie)
            .HasForeignKey(image => image.MovieId);
    }
}