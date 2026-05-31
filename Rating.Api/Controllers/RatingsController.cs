using Microsoft.AspNetCore.Mvc;
using Rating.Application.Interfaces;

namespace Rating.Api.Controllers;

[Route("api/courses/{courseId:int}/ratings")]
[ApiController]
public class RatingsController : ControllerBase
{
    private readonly IRatingService _ratingService;

    public RatingsController(IRatingService ratingService)
    {
        _ratingService = ratingService;
    }

    [HttpPost]
    public IActionResult CreateRating(int courseId)
    {
        return Ok($"Rating endpoint works for course {courseId}");
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetRatingSummary(
        int courseId,
        CancellationToken ct)
    {
        var result = await _ratingService.GetRatingSummary(courseId, ct);

        return Ok(result);
    }
}