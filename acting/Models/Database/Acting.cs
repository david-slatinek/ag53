using System.ComponentModel.DataAnnotations.Schema;

namespace acting.Models.Database;

/// <summary>
/// Acting model for the database.
/// </summary>
[Table("actors_movies")]
public class Acting
{
    /// <summary>
    /// Id.
    /// </summary>
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// Actor id.
    /// </summary>
    [Column("fk_actor")]
    public Guid ActorId { get; set; }

    /// <summary>
    /// Movie id.
    /// </summary>
    [Column("fk_movie")]
    public Guid MovieId { get; set; }
}