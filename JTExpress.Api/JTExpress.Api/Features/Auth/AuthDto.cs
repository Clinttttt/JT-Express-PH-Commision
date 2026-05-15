namespace JTExpress.Api.Features.Auth;

public sealed record LoginRequest(string Username, string Password);

public sealed record SignupRequest(string Username, string Password);

public sealed record LoginResponse(string Token, string Username);

public sealed record SetupStatusResponse(bool HasAdmin);
