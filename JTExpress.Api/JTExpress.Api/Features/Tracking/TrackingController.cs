using JTExpress.Api.Common;
using Microsoft.AspNetCore.Mvc;

namespace JTExpress.Api.Features.Tracking;

[ApiController]
[Route("api/[controller]")]
public sealed class TrackingController(ITrackingService service) : ControllerBase
{
    [HttpGet("{trackingNumber}")]
    [ProducesResponseType(typeof(ApiResponse<TrackingResultDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Track(string trackingNumber)
    {
        if (string.IsNullOrWhiteSpace(trackingNumber))
        {
            return BadRequest(ApiResponse<object>.Fail("Tracking number is required."));
        }

        var result = await service.TrackAsync(trackingNumber);
        if (result is null)
        {
            return NotFound(ApiResponse<object>.Fail($"No parcel found for tracking number '{trackingNumber}'."));
        }

        return Ok(ApiResponse<TrackingResultDto>.Ok(result));
    }
}
