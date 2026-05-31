using Rating.Domain.Entities;

namespace Rating.Application.Interfaces;

public interface IRatingRepository
{
    Task AddAsync(RatingEntity rating, CancellationToken ct);
    Task<List<RatingEntity>> GetByCourseIdAsync(int courseId, CancellationToken ct);
}