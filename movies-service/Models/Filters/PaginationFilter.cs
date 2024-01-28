using System.ComponentModel.DataAnnotations;

namespace movies_service.Models.Filters;

/// <summary>
/// Pagination filter.
/// </summary>
public class PaginationFilter
{
    private const int MinPageNumber = 1;
    private const int MaxPageSize = 20;

    /// <summary>
    /// Page number.
    /// </summary>
    [Range(MinPageNumber, int.MaxValue, ErrorMessage = "Page number must be greater than 0.")]
    public int PageNumber { get; set; }

    /// <summary>
    /// Page size.
    /// </summary>
    [Range(1, MaxPageSize, ErrorMessage = "Page size must be between 1 and 20.")]
    public int PageSize { get; set; }

    /// <summary>
    /// Default constructor.
    /// </summary>
    public PaginationFilter()
    {
        PageNumber = MinPageNumber;
        PageSize = MaxPageSize;
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="pageNumber">Page number.</param>
    /// <param name="pageSize">Page size.</param>
    public PaginationFilter(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber < MinPageNumber ? MinPageNumber : pageNumber;
        PageSize = pageSize > MaxPageSize ? MaxPageSize : pageSize;
    }
}