namespace movies_service.Models.Responses;

/// <summary>
/// Model for paged movies.
/// </summary>
public class PagedMovies
{
    /// <summary>
    /// Movies.
    /// </summary>
    public List<MovieDto> Movies { get; init; } = [];

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