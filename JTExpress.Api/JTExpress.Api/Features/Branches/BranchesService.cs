namespace JTExpress.Api.Features.Branches;

public sealed class BranchesService(IBranchesRepository repository) : IBranchesService
{
    public Task<IReadOnlyList<BranchDto>> GetBranchesAsync(string? region)
    {
        return string.IsNullOrWhiteSpace(region)
            ? repository.GetAllAsync()
            : repository.GetByRegionAsync(region);
    }

    public Task<BranchDto> CreateBranchAsync(CreateBranchDto dto) => repository.CreateAsync(dto);

    public Task<BranchDto> UpdateBranchAsync(int id, UpdateBranchDto dto) => repository.UpdateAsync(id, dto);

    public Task DeleteBranchAsync(int id) => repository.DeleteAsync(id);
}
