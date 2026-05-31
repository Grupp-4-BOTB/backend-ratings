using Microsoft.EntityFrameworkCore;
using Rating.Application.Interfaces;
using Rating.Domain.Entities;
using Rating.Infrastructure.Data;

namespace Rating.Infrastructure.Repositories;

public class RatingRepository : IRatingRepository
{
    private readonly RatingDbContext _dbContext;

    public RatingRepository(RatingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(RatingEntity rating, CancellationToken ct)
    {
        var existingRating = await _dbContext.Ratings
            .FirstOrDefaultAsync(
                r => r.CourseId == rating.CourseId &&
                     r.StudentId == rating.StudentId,
                ct);

        if (existingRating is null)
        {
            _dbContext.Ratings.Add(rating);
        }
        else
        {
            existingRating.Rating = rating.Rating;
        }

        await _dbContext.SaveChangesAsync(ct);
    }

    public async Task<List<RatingEntity>> GetByCourseIdAsync(int courseId, CancellationToken ct)
    {
        return await _dbContext.Ratings
            .Where(r => r.CourseId == courseId)
            .ToListAsync(ct);
    }
}
