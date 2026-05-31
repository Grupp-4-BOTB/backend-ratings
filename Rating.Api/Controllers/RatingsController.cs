using Microsoft.AspNetCore.Mvc;
using Rating.Application.Dtos;
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
    public async Task<IActionResult> CreateRating(int courseId, CreateRatingDto dto, CancellationToken ct)
    {
        try
        {
            await _ratingService.CreateRating(courseId, dto, ct);
            return Created();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetRatingSummary(int courseId, CancellationToken ct)
    {
        var result = await _ratingService.GetRatingSummary(courseId, ct);
        return Ok(result);
    }
}