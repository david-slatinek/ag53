using System.ComponentModel.DataAnnotations.Schema;

namespace actors_service.Models.Database;

/// <summary>
/// Actor model for the database.
/// </summary>
[Table("actors")]
public class Actor
{
    /// <summary>
    /// Actor's unique identifier.
    /// </summary>
    [Column("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// First name of the actor.
    /// </summary>
    [Column("first_name")]
    public string FirstName { get; set; } = null!;

    /// <summary>
    /// Last name of the actor.
    /// </summary>
    [Column("last_name")]
    public string LastName { get; set; } = null!;

    /// <summary>
    /// Birth date of the actor.
    /// </summary>
    [Column("birthday")]
    public DateOnly BirthDate { get; set; }
}