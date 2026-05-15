using JTExpress.Api.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JTExpress.Api.Features.Branches;

[ApiController]
[Route("api/[controller]")]
public sealed class BranchesController(IBranchesService service) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<BranchDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] string? region = null)
    {
        var result = await service.GetBranchesAsync(region);
        return Ok(ApiResponse<IReadOnlyList<BranchDto>>.Ok(result));
    }

    [Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<BranchDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateBranchDto dto)
    {
        var result = await service.CreateBranchAsync(dto);
        return CreatedAtAction(nameof(GetAll), new { }, ApiResponse<BranchDto>.Ok(result));
    }

    [Authorize]
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<BranchDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateBranchDto dto)
    {
        var result = await service.UpdateBranchAsync(id, dto);
        return Ok(ApiResponse<BranchDto>.Ok(result));
    }

    [Authorize]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(int id)
    {
        await service.DeleteBranchAsync(id);
        return NoContent();
    }
}
