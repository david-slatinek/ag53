namespace acting.Middlewares;

/// <summary>
/// Middleware for counting requests.
/// </summary>
/// <param name="next">Next request delegate.</param>
public class RequestCounter(RequestDelegate next)
{
    /// <summary>
    /// Counts for each request.
    /// </summary>
    private readonly Dictionary<string, int> _counts = new();

    /// <summary>
    /// Increment request count for the path.
    /// </summary>
    /// <param name="context">HTTP context.</param>
    public async Task Invoke(HttpContext context)
    {
        var method = context.Request.Method;
        var path = context.Request.Path;

        var request = $"{method}--{path}";

        if (_counts.TryGetValue(request, out var value))
        {
            _counts[request] = ++value;
        }
        else
        {
            _counts.Add(request, 1);
        }

        Console.WriteLine($"Request count for {request}: {_counts[request]}");

        await next(context);
    }
}