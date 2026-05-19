namespace JTExpress.Api.Features.Services;

public sealed record ServiceDto(
    int Id,
    string Name,
    string Description,
    string PriceLabel);

public sealed record CreateServiceDto(
    string Name,
    string Description,
    string PriceLabel);

public sealed record UpdateServiceDto(
    string Name,
    string Description,
    string PriceLabel);
