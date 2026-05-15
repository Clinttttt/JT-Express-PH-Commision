using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JTExpress.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace JTExpress.Api.Features.Auth;

public interface IAuthService
{
    Task<LoginResponse?> LoginAsync(string username, string password);
    Task<LoginResponse?> SignupAsync(string username, string password);
    Task<bool> HasAdminAsync();
}

public sealed class AuthService(AppDbContext dbContext, IConfiguration configuration) : IAuthService
{
    public async Task<LoginResponse?> LoginAsync(string username, string password)
    {
        var user = await dbContext.Users
            .FirstOrDefaultAsync(u => u.Username == username);

        if (user is null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            return null;

        var token = GenerateJwtToken(username);
        return new LoginResponse(token, username);
    }

    public async Task<LoginResponse?> SignupAsync(string username, string password)
    {
        var existingUser = await dbContext.Users
            .FirstOrDefaultAsync(u => u.Username == username);

        if (existingUser is not null)
            return null;

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
        var user = new UserEntity
        {
            Username = username,
            PasswordHash = hashedPassword
        };

        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();

        var token = GenerateJwtToken(username);
        return new LoginResponse(token, username);
    }

    public async Task<bool> HasAdminAsync()
    {
        return await dbContext.Users.AnyAsync();
    }

    private string GenerateJwtToken(string username)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, "Admin")
        };

        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
