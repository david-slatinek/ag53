using actors_service.Data;
using actors_service.Models;
using actors_service.Models.Database;
using Bogus;

namespace actors_service.Seed;

public class Seed(DataContext context)
{
    public async Task SeedAsync()
    {
        await context.Database.EnsureCreatedAsync();
        var actors = Enumerable.Range(1, 50).Select(_ => RandomActor());
        await context.Actors.AddRangeAsync(actors);
        await context.SaveChangesAsync();
    }

    private static Actor RandomActor() => new Faker<Actor>()
        .RuleFor(a => a.Id, f => f.Random.Guid())
        .RuleFor(a => a.FirstName, f => f.Person.FirstName)
        .RuleFor(a => a.LastName, f => f.Person.LastName)
        .RuleFor(a => a.BirthDate, f => DateOnly.FromDateTime(f.Person.DateOfBirth))
        .Generate();
}