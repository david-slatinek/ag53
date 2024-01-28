using Bogus;
using movies_service.Data;
using movies_service.Models.Database;

namespace movies_service.Seed;

/// <summary>
/// Seed class for Movies.
/// </summary>
/// <param name="context"></param>
public class Seed(DataContext context)
{
    /// <summary>
    /// Seed Movies.
    /// </summary>
    public async Task SeedAsync()
    {
        await context.Database.EnsureCreatedAsync();
        var movies = Enumerable.Range(1, 50).Select(_ => RandomMovie());
        await context.Movies.AddRangeAsync(movies);
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Get random Movie.
    /// </summary>
    /// <returns>Random Movie.</returns>
    private static Movie RandomMovie() => new Faker<Movie>()
        .RuleFor(m => m.Id, f => f.Random.Guid())
        .RuleFor(m => m.Title, f => f.Random.Words(3))
        .RuleFor(m => m.Description, f => f.Random.Words(10))
        .RuleFor(m => m.Release, f => DateOnly.FromDateTime(f.Date.Past()))
        .Generate();
}