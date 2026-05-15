using JTExpress.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace JTExpress.Api.Features.Rates;

public sealed class RatesRepository(AppDbContext dbContext) : IRatesRepository
{
    public async Task<IReadOnlyList<RateDto>> GetAllAsync()
    {
        return await dbContext.Rates
            .AsNoTracking()
            .OrderBy(rate => rate.Id)
            .Select(rate => new RateDto(rate.Id, rate.Zone, rate.FirstKg, rate.SucceedingKg))
            .ToListAsync();
    }

    public async Task<RateDto?> GetByZoneAsync(string zone)
    {
        return await dbContext.Rates
            .AsNoTracking()
            .Where(rate => rate.Zone.ToLower() == zone.ToLower())
            .Select(rate => new RateDto(rate.Id, rate.Zone, rate.FirstKg, rate.SucceedingKg))
            .FirstOrDefaultAsync();
    }

    public async Task<RateDto> CreateAsync(CreateRateDto dto)
    {
        var entity = new RateEntity
        {
            Zone = dto.Zone,
            FirstKg = dto.FirstKg,
            SucceedingKg = dto.SucceedingKg
        };

        dbContext.Rates.Add(entity);
        await dbContext.SaveChangesAsync();

        return new RateDto(entity.Id, entity.Zone, entity.FirstKg, entity.SucceedingKg);
    }

    public async Task<RateDto> UpdateAsync(int id, UpdateRateDto dto)
    {
        var entity = await dbContext.Rates.FindAsync(id) ?? throw new InvalidOperationException("Rate not found");

        entity.Zone = dto.Zone;
        entity.FirstKg = dto.FirstKg;
        entity.SucceedingKg = dto.SucceedingKg;

        await dbContext.SaveChangesAsync();

        return new RateDto(entity.Id, entity.Zone, entity.FirstKg, entity.SucceedingKg);
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await dbContext.Rates.FindAsync(id) ?? throw new InvalidOperationException("Rate not found");
        dbContext.Rates.Remove(entity);
        await dbContext.SaveChangesAsync();
    }
}
