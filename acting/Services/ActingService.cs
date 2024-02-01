using System.Net;
using System.Net.Http.Headers;
using acting.Interfaces;
using acting.Models.Database;
using acting.Models.Requests;
using acting.Models.Responses;

namespace acting.Services;

/// <summary>
/// Actor service.
/// </summary>
/// <param name="actingRepository">Acting repository.</param>
/// <param name="configuration">Configuration.</param>
public class ActingService(IActingRepository actingRepository, IConfiguration configuration) : IActingService
{
    /// <summary>
    /// Acting repository.
    /// </summary>
    private IActingRepository ActingRepository { get; } = actingRepository;

    /// <summary>
    /// Configuration.
    /// </summary>
    private IConfiguration Configuration { get; } = configuration;

    /// <inheritdoc />
    public ActingDto CreateActing(CreateActing createActing)
    {
        var alreadyExists = ActingRepository.AlreadyExists(createActing);
        if (alreadyExists)
        {
            throw new BadHttpRequestException(
                $"Acting with actor id = {createActing.ActorId} and movie id = {createActing.MovieId} already exists.");
        }

        var actor = GetActorById(createActing.ActorId);
        if (actor == null)
        {
            throw new BadHttpRequestException($"Actor with id = {createActing.ActorId} does not exist.");
        }

        var movie = GetMovieById(createActing.MovieId);
        if (movie == null)
        {
            throw new BadHttpRequestException($"Movie with id = {createActing.MovieId} does not exist.");
        }

        return ActingRepository.CreateActing(new CreateActing
        {
            ActorId = actor.Id,
            MovieId = movie.Id
        });
    }

    /// <summary>
    /// Get actor from actors service.
    /// </summary>
    /// <param name="id">Actor ID.</param>
    /// <returns>Actor if it exists, null otherwise.</returns>
    private ActorDto? GetActorById(Guid id)
    {
        var url = Configuration["ActorsServiceUrl"];
        if (string.IsNullOrEmpty(url))
        {
            throw new Exception("Actors service URL not configured.");
        }

        var client = new HttpClient();
        client.BaseAddress = new Uri(url);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var response = client.GetAsync("/api/actors/" + id).Result;

        return response.StatusCode == HttpStatusCode.BadRequest
            ? null
            : response.Content.ReadFromJsonAsync<ActorDto>().Result;
    }

    /// <summary>
    /// Get movie from movies service.
    /// </summary>
    /// <param name="id">Movie ID.</param>
    /// <returns>Movie if it exists, null otherwise.</returns>
    private MovieDto? GetMovieById(Guid id)
    {
        var url = Configuration["MoviesServiceUrl"];
        if (string.IsNullOrEmpty(url))
        {
            throw new Exception("Movies service URL not configured.");
        }

        var client = new HttpClient();
        client.BaseAddress = new Uri(url);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var response = client.GetAsync("/api/movies/" + id).Result;

        return response.StatusCode == HttpStatusCode.BadRequest
            ? null
            : response.Content.ReadFromJsonAsync<MovieDto>().Result;
    }
}