using JTExpress.Api.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JTExpress.Api.Features.Shipments;

[ApiController]
[Route("api/[controller]")]
public sealed class ShipmentsController(IShipmentsService service) : ControllerBase
{
    [HttpGet("{trackingNumber}")]
    [ProducesResponseType(typeof(ApiResponse<ShipmentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByTrackingNumber(string trackingNumber)
    {
        var result = await service.GetByTrackingNumberAsync(trackingNumber);
        if (result is null)
            return NotFound(ApiResponse<object>.Fail($"Shipment '{trackingNumber}' not found."));

        return Ok(ApiResponse<ShipmentDto>.Ok(result));
    }

    [Authorize]
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<ShipmentDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await service.GetAllAsync();
        return Ok(ApiResponse<IReadOnlyList<ShipmentDto>>.Ok(result));
    }

    [Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<ShipmentDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateShipmentDto dto)
    {
        var result = await service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetByTrackingNumber), new { trackingNumber = result.TrackingNumber }, ApiResponse<ShipmentDto>.Ok(result));
    }

    [Authorize]
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<ShipmentDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateShipmentDto dto)
    {
        var result = await service.UpdateAsync(id, dto);
        return Ok(ApiResponse<ShipmentDto>.Ok(result));
    }

    [Authorize]
    [HttpPost("{id}/events")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> AddEvent(int id, [FromBody] AddTrackingEventDto dto)
    {
        await service.AddEventAsync(id, dto);
        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(int id)
    {
        await service.DeleteAsync(id);
        return NoContent();
    }
}
