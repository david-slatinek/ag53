using actors_service.Data;
using actors_service.Models.Database;
using Bogus;

namespace actors_service.Seed;

/// <summary>
/// Seed class for Actors.
/// </summary>
/// <param name="context"></param>
public class Seed(DataContext context)
{
    /// <summary>
    /// Seed Actors.
    /// </summary>
    public async Task SeedAsync()
    {
        await context.Database.EnsureCreatedAsync();
        var actors = Enumerable.Range(1, 50).Select(_ => RandomActor());
        await context.Actors.AddRangeAsync(actors);
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Get random Actor.
    /// </summary>
    /// <returns>Random Actor.</returns>
    private static Actor RandomActor() => new Faker<Actor>()
        .RuleFor(a => a.Id, f => f.Random.Guid())
        .RuleFor(a => a.FirstName, f => f.Person.FirstName)
        .RuleFor(a => a.LastName, f => f.Person.LastName)
        .RuleFor(a => a.BirthDate, f => DateOnly.FromDateTime(f.Person.DateOfBirth))
        .Generate();
}