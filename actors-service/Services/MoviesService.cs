using System.Net;
using System.Net.Http.Headers;
using System.Text;
using actors_service.Interfaces;
using actors_service.Models.Responses;
using Newtonsoft.Json;

namespace actors_service.Services;

/// <summary>
/// Movies service.
/// </summary>
/// <param name="configuration">Configuration.</param>
public class MoviesService(IConfiguration configuration) : IMoviesService
{
    /// <summary>
    /// Configuration.
    /// </summary>
    private IConfiguration Configuration { get; } = configuration;

    /// <inheritdoc />
    public List<MovieDto> GetMoviesForActor(Guid actorId)
    {
        var url = Configuration["ActingServiceUrl"];
        if (string.IsNullOrEmpty(url))
        {
            throw new Exception("Acting service URL is not configured.");
        }

        var actings = GetActingsForActor(actorId);
        if (actings.Count == 0)
        {
            return [];
        }

        var movieIds = actings.Select(a => a.Id);
        var movies = GetMovies(movieIds);

        return movies;
    }

    /// <summary>
    /// Get all actors for an actor.
    /// </summary>
    /// <param name="actorId">Actor id.</param>
    /// <returns>List of movies for the actor.</returns>
    private List<MovieDto> GetActingsForActor(Guid actorId)
    {
        var url = Configuration["ActingServiceUrl"];
        if (string.IsNullOrEmpty(url))
        {
            throw new Exception("Acting service URL is not configured.");
        }

        var client = new HttpClient();
        client.BaseAddress = new Uri(url);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var response = client.GetAsync($"/api/acting/actors/{actorId}").Result;

        return response.StatusCode switch
        {
            HttpStatusCode.OK => response.Content.ReadFromJsonAsync<List<MovieDto>>().Result ?? [],
            HttpStatusCode.NoContent => [],
            _ => throw new Exception($"Acting service returned status code {response.StatusCode}.")
        };
    }

    /// <summary>
    /// Get movies by ids.
    /// </summary>
    /// <param name="movieIds">Movies ids.</param>
    /// <returns>A list of movies.</returns>
    private List<MovieDto> GetMovies(IEnumerable<Guid> movieIds)
    {
        var url = Configuration["MoviesServiceUrl"];
        if (string.IsNullOrEmpty(url))
        {
            throw new Exception("Movies service URL is not configured.");
        }

        var client = new HttpClient();
        client.BaseAddress = new Uri(url);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var response = client.PostAsync("/api/movies/ids",
            new StringContent(JsonConvert.SerializeObject(movieIds), Encoding.UTF8, "application/json")).Result;

        return response.StatusCode switch
        {
            HttpStatusCode.OK => response.Content.ReadFromJsonAsync<List<MovieDto>>().Result ?? [],
            HttpStatusCode.NoContent => [],
            _ => throw new Exception($"Movies service returned status code {response.StatusCode}.")
        };
    }
}