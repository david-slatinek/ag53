using System.ComponentModel.DataAnnotations.Schema;

namespace movies_service.Models.Database;

/// <summary>
/// Image model for the database.
/// </summary>
[Table("images")]
public class Image
{
    /// <summary>
    /// Image's unique identifier.
    /// </summary>
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// Movie ID, which this image belongs to.
    /// </summary>
    [Column("fk_movie")]
    public Guid MovieId { get; set; }

    /// <summary>
    /// Filename of the image, how it is stored in Minio.
    /// </summary>
    [Column("filename")]
    public string FileName { get; set; } = null!;

    /// <summary>
    /// Movie, which this image belongs to.
    /// </summary>
    public Movie Movie { get; set; } = null!;
}