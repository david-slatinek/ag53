namespace movies_service.Models.Responses;

/// <summary>
/// Model for an error response.
/// </summary>
public class Error
{
    /// <summary>
    /// Error message.
    /// </summary>
    public string Message { get; set; } = null!;
}