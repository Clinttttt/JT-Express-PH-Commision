using JTExpress.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace JTExpress.Api.Features.Branches;

public sealed class BranchesRepository(AppDbContext dbContext) : IBranchesRepository
{
    public async Task<IReadOnlyList<BranchDto>> GetAllAsync()
    {
        return await GetBranches().ToListAsync();
    }

    public async Task<IReadOnlyList<BranchDto>> GetByRegionAsync(string region)
    {
        return await dbContext.Branches
            .AsNoTracking()
            .Where(branch => branch.Region.ToLower() == region.ToLower())
            .OrderBy(branch => branch.Id)
            .Select(branch => new BranchDto(
                branch.Id,
                branch.Name,
                branch.Address,
                branch.Region,
                branch.Phone,
                branch.Hours,
                branch.Latitude,
                branch.Longitude))
            .ToListAsync();
    }

    public async Task<BranchDto> CreateAsync(CreateBranchDto dto)
    {
        var entity = new BranchEntity
        {
            Name = dto.Name,
            Address = dto.Address,
            Region = dto.Region,
            Phone = dto.Phone,
            Hours = dto.Hours,
            Latitude = dto.Latitude,
            Longitude = dto.Longitude
        };

        dbContext.Branches.Add(entity);
        await dbContext.SaveChangesAsync();

        return new BranchDto(entity.Id, entity.Name, entity.Address, entity.Region, entity.Phone, entity.Hours, entity.Latitude, entity.Longitude);
    }

    public async Task<BranchDto> UpdateAsync(int id, UpdateBranchDto dto)
    {
        var entity = await dbContext.Branches.FindAsync(id) ?? throw new InvalidOperationException("Branch not found");

        entity.Name = dto.Name;
        entity.Address = dto.Address;
        entity.Region = dto.Region;
        entity.Phone = dto.Phone;
        entity.Hours = dto.Hours;
        entity.Latitude = dto.Latitude;
        entity.Longitude = dto.Longitude;

        await dbContext.SaveChangesAsync();

        return new BranchDto(entity.Id, entity.Name, entity.Address, entity.Region, entity.Phone, entity.Hours, entity.Latitude, entity.Longitude);
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await dbContext.Branches.FindAsync(id) ?? throw new InvalidOperationException("Branch not found");
        dbContext.Branches.Remove(entity);
        await dbContext.SaveChangesAsync();
    }

    private IQueryable<BranchDto> GetBranches()
    {
        return dbContext.Branches
            .AsNoTracking()
            .OrderBy(branch => branch.Id)
            .Select(branch => new BranchDto(
                branch.Id,
                branch.Name,
                branch.Address,
                branch.Region,
                branch.Phone,
                branch.Hours,
                branch.Latitude,
                branch.Longitude));
    }
}
