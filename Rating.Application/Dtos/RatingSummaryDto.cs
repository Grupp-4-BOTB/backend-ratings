namespace Rating.Application.Dtos;

public class RatingSummaryDto
{
    public double AverageRating { get; set; }
    public int TotalReviews { get; set; }
    public List<RatingRowDto> Ratings { get; set; } = [];
}