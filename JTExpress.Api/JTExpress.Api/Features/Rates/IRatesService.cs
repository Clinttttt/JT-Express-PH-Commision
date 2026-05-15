namespace JTExpress.Api.Features.Rates;

public interface IRatesService
{
    Task<IReadOnlyList<RateDto>> GetAllRatesAsync();
    Task<RateCalculationResultDto?> CalculateAsync(string zone, double weight);
    Task<RateDto> CreateRateAsync(CreateRateDto dto);
    Task<RateDto> UpdateRateAsync(int id, UpdateRateDto dto);
    Task DeleteRateAsync(int id);
}
