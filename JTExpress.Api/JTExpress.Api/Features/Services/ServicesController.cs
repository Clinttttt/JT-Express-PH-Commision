using JTExpress.Api.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JTExpress.Api.Features.Services;

[ApiController]
[Route("api/[controller]")]
public sealed class ServicesController(IServicesService service) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<ServiceDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await service.GetAllServicesAsync();
        return Ok(ApiResponse<IReadOnlyList<ServiceDto>>.Ok(result));
    }

    [Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<ServiceDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateServiceDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.Description))
            return BadRequest(ApiResponse<object>.Fail("Name and Description are required."));

        var result = await service.CreateServiceAsync(dto);
        return CreatedAtAction(nameof(GetAll), ApiResponse<ServiceDto>.Ok(result));
    }

    [Authorize]
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<ServiceDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateServiceDto dto)
    {
        var result = await service.UpdateServiceAsync(id, dto);
        if (result is null)
            return NotFound(ApiResponse<object>.Fail($"Service with ID {id} not found."));

        return Ok(ApiResponse<ServiceDto>.Ok(result));
    }

    [Authorize]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await service.DeleteServiceAsync(id);
        if (!success)
            return NotFound(ApiResponse<object>.Fail($"Service with ID {id} not found."));

        return NoContent();
    }
}
