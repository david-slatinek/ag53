namespace actors_service.Models.Responses;

/// <summary>
/// Actor response model.
/// </summary>
public class ActorDto
{
    /// <summary>
    /// Actor's unique identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// First name of the actor.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Last name of the actor.
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Birth date of the actor.
    /// </summary>
    public string BirthDate { get; set; } = string.Empty;
}