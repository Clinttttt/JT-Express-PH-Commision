using JTExpress.Api.Common;
using Microsoft.AspNetCore.Mvc;

namespace JTExpress.Api.Features.Auth;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController(IAuthService authService) : ControllerBase
{
    [HttpGet("setup-status")]
    [ProducesResponseType(typeof(ApiResponse<SetupStatusResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSetupStatus()
    {
        var hasAdmin = await authService.HasAdminAsync();
        return Ok(ApiResponse<SetupStatusResponse>.Ok(new SetupStatusResponse(hasAdmin)));
    }

    [HttpPost("signup")]
    [ProducesResponseType(typeof(ApiResponse<LoginResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Signup([FromBody] SignupRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            return BadRequest(ApiResponse<object>.Fail("Username and password are required."));

        if (request.Password.Length < 6)
            return BadRequest(ApiResponse<object>.Fail("Password must be at least 6 characters."));

        var result = await authService.SignupAsync(request.Username, request.Password);
        
        if (result is null)
            return BadRequest(ApiResponse<object>.Fail("Username already exists."));

        return CreatedAtAction(nameof(GetSetupStatus), ApiResponse<LoginResponse>.Ok(result));
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<LoginResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await authService.LoginAsync(request.Username, request.Password);
        
        if (result is null)
            return Unauthorized(ApiResponse<object>.Fail("Invalid username or password."));

        return Ok(ApiResponse<LoginResponse>.Ok(result));
    }

    [HttpPost("reset-password")]
    [ProducesResponseType(typeof(ApiResponse<ResetPasswordResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || 
            string.IsNullOrWhiteSpace(request.RestorationKey) || 
            string.IsNullOrWhiteSpace(request.NewPassword))
            return BadRequest(ApiResponse<object>.Fail("All fields are required."));

        if (request.NewPassword.Length < 6)
            return BadRequest(ApiResponse<object>.Fail("Password must be at least 6 characters."));

        var result = await authService.ResetPasswordAsync(request.Username, request.RestorationKey, request.NewPassword);
        
        if (result is null)
            return BadRequest(ApiResponse<object>.Fail("Invalid username or restoration key."));

        return Ok(ApiResponse<ResetPasswordResponse>.Ok(result));
    }
}
