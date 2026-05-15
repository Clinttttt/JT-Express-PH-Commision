namespace JTExpress.Api.Features.Rates;

public sealed class RatesService(IRatesRepository repository) : IRatesService
{
    public Task<IReadOnlyList<RateDto>> GetAllRatesAsync()
    {
        return repository.GetAllAsync();
    }

    public async Task<RateCalculationResultDto?> CalculateAsync(string zone, double weight)
    {
        var rate = await repository.GetByZoneAsync(zone);
        if (rate is null)
        {
            return null;
        }

        var extraKg = weight > 1 ? Math.Ceiling(weight - 1) : 0;
        var total = rate.FirstKg + ((decimal)extraKg * rate.SucceedingKg);

        return new RateCalculationResultDto(
            rate.Zone,
            weight,
            total,
            $"PHP {total:N0}");
    }

    public Task<RateDto> CreateRateAsync(CreateRateDto dto) => repository.CreateAsync(dto);

    public Task<RateDto> UpdateRateAsync(int id, UpdateRateDto dto) => repository.UpdateAsync(id, dto);

    public Task DeleteRateAsync(int id) => repository.DeleteAsync(id);
}
