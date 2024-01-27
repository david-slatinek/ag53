namespace actors_service.Models.Responses;

/// <summary>
/// Model for paged actors.
/// </summary>
public class PagedActors
{
    /// <summary>
    /// Actors.
    /// </summary>
    public List<ActorDto> Actors { get; init; } = [];

    /// <summary>
    /// Page number.
    /// </summary>
    public int PageNumber { get; set; }

    /// <summary>
    /// Page size.
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Total number of pages.
    /// </summary>
    public int TotalPages { get; set; }

    /// <summary>
    /// Total number of records.
    /// </summary>
    public int TotalRecords { get; set; }
}