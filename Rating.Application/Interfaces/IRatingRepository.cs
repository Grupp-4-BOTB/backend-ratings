using Rating.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rating.Application.Interfaces
{
    public interface IRatingRepository
    {
        Task AddRatingAsync(RatingEntity ratingEntity, CancellationToken ct);
        Task<RatingEntity> GetRatingByCourseIdAsync(int studentId, CancellationToken ct);

    }
}
