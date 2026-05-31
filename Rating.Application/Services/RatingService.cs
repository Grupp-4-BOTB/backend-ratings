
using Rating.Application.Dtos;
using Rating.Application.Interfaces;
using Rating.Domain.Entities;

namespace Rating.Application.Services;

public class RatingService : IRatingService
{
    private readonly IRatingRepository _ratingRepository;

    public RatingService(IRatingRepository ratingRepository)
    {
        _ratingRepository = ratingRepository;
    }

    public async Task CreateRating(int courseId, CreateRatingDto dto, CancellationToken ct)
    {
        if (dto.Rating < 1 || dto.Rating > 5)
        {
            throw new ArgumentException("Rating must be between 1 and 5.");
        }

        var rating = new RatingEntity
        {
            CourseId = courseId,
            StudentId = dto.StudentId,
            Rating = dto.Rating
        };

        await _ratingRepository.AddAsync(rating, ct);
    }

    public async Task<RatingSummaryDto> GetRatingSummary(int courseId, CancellationToken ct)
    {
        var ratings = await _ratingRepository.GetByCourseIdAsync(courseId, ct);

        if (ratings.Count == 0)
        {
            return new RatingSummaryDto
            {
                AverageRating = 0,
                TotalReviews = 0,
                Ratings =
                [
                    new RatingRowDto { Stars = 5, Percentage = 0 },
                    new RatingRowDto { Stars = 4, Percentage = 0 },
                    new RatingRowDto { Stars = 3, Percentage = 0 },
                    new RatingRowDto { Stars = 2, Percentage = 0 },
                    new RatingRowDto { Stars = 1, Percentage = 0 }
                ]
            };
        }

        var totalReviews = ratings.Count;
        var averageRating = ratings.Average(r => r.Rating);

        return new RatingSummaryDto
        {
            AverageRating = Math.Round(averageRating, 1),
            TotalReviews = totalReviews,
            Ratings = new List<int> { 5, 4, 3, 2, 1 }
                .Select(stars => new RatingRowDto
                {
                    Stars = stars,
                    Percentage = (int)Math.Round(
                        ratings.Count(r => r.Rating == stars) * 100.0 / totalReviews)
                })
                .ToList()
        };
    }
}
