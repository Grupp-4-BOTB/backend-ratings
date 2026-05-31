using Rating.Application.Dtos;

namespace Rating.Application.Interfaces;

public interface IRatingService
{
    Task CreateRating(int courseId, CreateRatingDto dto, CancellationToken ct);

    Task<RatingSummaryDto> GetRatingSummary(int courseId, CancellationToken ct);
}