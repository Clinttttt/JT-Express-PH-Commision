namespace JTExpress.Api.Features.Rates;

public sealed record RateCalculationResultDto(
    string Zone,
    double Weight,
    decimal EstimatedRate,
    string FormattedRate);
