namespace JTExpress.Api.Features.Branches;

public interface IBranchesRepository
{
    Task<IReadOnlyList<BranchDto>> GetAllAsync();
    Task<IReadOnlyList<BranchDto>> GetByRegionAsync(string region);
    Task<BranchDto> CreateAsync(CreateBranchDto dto);
    Task<BranchDto> UpdateAsync(int id, UpdateBranchDto dto);
    Task DeleteAsync(int id);
}
