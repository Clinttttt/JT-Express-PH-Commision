using JTExpress.Api.Data;
using JTExpress.Api.Features.Auth;
using JTExpress.Api.Features.Branches;
using JTExpress.Api.Features.Rates;
using JTExpress.Api.Features.Services;
using JTExpress.Api.Features.Shipments;
using JTExpress.Api.Features.Tracking;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace JTExpress.Api.Common.Extensions;

public static class ServiceCollectionExtensions
{
    public const string CorsPolicyName = "ReactClient";

    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        // Add SSL mode for production (Render requires it)
        if (!connectionString!.Contains("SSL Mode", StringComparison.OrdinalIgnoreCase))
        {
            connectionString += ";SSL Mode=Require;Trust Server Certificate=true";
        }
        
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));

        var allowedOrigins = configuration
            .GetSection("Cors:AllowedOrigins")
            .Get<string[]>() ?? [];

        services.AddCors(options =>
        {
            options.AddPolicy(CorsPolicyName, policy =>
                policy.WithOrigins(allowedOrigins)
                    .AllowAnyHeader()
                    .AllowAnyMethod());
        });

        // JWT Authentication
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
                };
            });

        services.AddAuthorization();

        services.AddScoped<IAuthService, AuthService>();

        services.AddScoped<IServicesRepository, ServicesRepository>();
        services.AddScoped<IServicesService, ServicesService>();

        services.AddScoped<IRatesRepository, RatesRepository>();
        services.AddScoped<IRatesService, RatesService>();

        services.AddScoped<IShipmentsRepository, ShipmentsRepository>();
        services.AddScoped<IShipmentsService, ShipmentsService>();

        services.AddScoped<ITrackingRepository, TrackingRepository>();
        services.AddScoped<ITrackingService, TrackingService>();

        services.AddScoped<IBranchesRepository, BranchesRepository>();
        services.AddScoped<IBranchesService, BranchesService>();

        return services;
    }

    public static async Task ApplyDatabaseMigrationsAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await dbContext.Database.MigrateAsync();
    }
}
