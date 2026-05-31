using Rating.Application.Interfaces;
using Rating.Domain.Entities;

namespace Rating.Infrastructure.Repositories;

public class RatingRepository : IRatingRepository
{
    public Task AddRatingAsync(RatingEntity ratingEntity, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<RatingEntity> GetRatingByCourseIdAsync(int studentId, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
