namespace JTExpress.Api.Features.Rates;

public sealed record RateDto(int Id, string Zone, decimal FirstKg, decimal SucceedingKg);

public sealed record CreateRateDto(string Zone, decimal FirstKg, decimal SucceedingKg);

public sealed record UpdateRateDto(string Zone, decimal FirstKg, decimal SucceedingKg);
