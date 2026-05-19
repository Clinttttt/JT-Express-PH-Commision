namespace JTExpress.Api.Features.Auth;

public sealed record LoginRequest(string Username, string Password);

public sealed record SignupRequest(string Username, string Password);

public sealed record LoginResponse(string Token, string Username, string? RestorationKey = null);

public sealed record SetupStatusResponse(bool HasAdmin);

public sealed record ResetPasswordRequest(string Username, string RestorationKey, string NewPassword);

public sealed record ResetPasswordResponse(string RestorationKey);
