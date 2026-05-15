using JTExpress.Api.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JTExpress.Api.Features.Rates;

[ApiController]
[Route("api/[controller]")]
public sealed class RatesController(IRatesService service) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<RateDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await service.GetAllRatesAsync();
        return Ok(ApiResponse<IReadOnlyList<RateDto>>.Ok(result));
    }

    [HttpGet("calculate")]
    [ProducesResponseType(typeof(ApiResponse<RateCalculationResultDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Calculate([FromQuery] string? zone, [FromQuery] double weight)
    {
        if (string.IsNullOrWhiteSpace(zone) || weight <= 0)
        {
            return BadRequest(ApiResponse<object>.Fail("Zone and a positive weight are required."));
        }

        var result = await service.CalculateAsync(zone, weight);
        if (result is null)
        {
            return BadRequest(ApiResponse<object>.Fail($"Zone '{zone}' was not found."));
        }

        return Ok(ApiResponse<RateCalculationResultDto>.Ok(result));
    }

    [Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<RateDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateRateDto dto)
    {
        var result = await service.CreateRateAsync(dto);
        return CreatedAtAction(nameof(GetAll), new { }, ApiResponse<RateDto>.Ok(result));
    }

    [Authorize]
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<RateDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateRateDto dto)
    {
        var result = await service.UpdateRateAsync(id, dto);
        return Ok(ApiResponse<RateDto>.Ok(result));
    }

    [Authorize]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(int id)
    {
        await service.DeleteRateAsync(id);
        return NoContent();
    }
}
