using Microsoft.AspNetCore.Mvc;

namespace Rating.Api.Controllers;

[Route("api/courses/{courseId:int}/ratings")]
[ApiController]
public class RatingsController : ControllerBase
{
    [HttpPost]
    public IActionResult CreateRating(int courseId)
    {
        return Ok($"Rating endpoint works for course {courseId}");
    }
    [HttpGet("summary")]
    public IActionResult GetRatingSummary(int courseId)
    {
        return Ok(new
        {
            averageRating = 0,
            totalReviews = 0,
            ratings = new[]
            {
                    new { stars = 5, percentage = 0 },
                    new { stars = 4, percentage = 0 },
                    new { stars = 3, percentage = 0 },
                    new { stars = 2, percentage = 0 },
                    new { stars = 1, percentage = 0 }
            }
        });
    }
}
