using acting.Interfaces;
using acting.Models.Requests;
using acting.Models.Responses;

namespace acting.Mocking;

/// <summary>
/// Service used for unit testing.
/// </summary>
public class ActingServiceFake(IActingRepository actingRepository) : IActingService
{
    /// <inheritdoc />
    public ActingDto CreateActing(CreateActing createActing)
    {
        return actingRepository.CreateActing(createActing);
    }
}