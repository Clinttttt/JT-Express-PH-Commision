using JTExpress.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace JTExpress.Api.Features.Services;

public sealed class ServicesRepository(AppDbContext dbContext) : IServicesRepository
{
    public async Task<IReadOnlyList<ServiceDto>> GetAllAsync()
    {
        return await dbContext.Services
            .AsNoTracking()
            .OrderBy(service => service.Id)
            .Select(service => new ServiceDto(
                service.Id,
                service.Name,
                service.Description,
                service.PriceLabel))
            .ToListAsync();
    }

    public async Task<ServiceDto> CreateAsync(CreateServiceDto dto)
    {
        var entity = new ServiceEntity
        {
            Name = dto.Name,
            Description = dto.Description,
            PriceLabel = dto.PriceLabel
        };

        dbContext.Services.Add(entity);
        await dbContext.SaveChangesAsync();

        return new ServiceDto(entity.Id, entity.Name, entity.Description, entity.PriceLabel);
    }

    public async Task<ServiceDto?> UpdateAsync(int id, UpdateServiceDto dto)
    {
        var entity = await dbContext.Services.FindAsync(id);
        if (entity is null) return null;

        dbContext.Entry(entity).CurrentValues.SetValues(new
        {
            Id = id,
            Name = dto.Name,
            Description = dto.Description,
            PriceLabel = dto.PriceLabel
        });

        await dbContext.SaveChangesAsync();

        return new ServiceDto(entity.Id, entity.Name, entity.Description, entity.PriceLabel);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await dbContext.Services.FindAsync(id);
        if (entity is null) return false;

        dbContext.Services.Remove(entity);
        await dbContext.SaveChangesAsync();

        return true;
    }
}
