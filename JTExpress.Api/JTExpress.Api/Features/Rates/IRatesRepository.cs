namespace JTExpress.Api.Features.Rates;

public interface IRatesRepository
{
    Task<IReadOnlyList<RateDto>> GetAllAsync();
    Task<RateDto?> GetByZoneAsync(string zone);
    Task<RateDto> CreateAsync(CreateRateDto dto);
    Task<RateDto> UpdateAsync(int id, UpdateRateDto dto);
    Task DeleteAsync(int id);
}
