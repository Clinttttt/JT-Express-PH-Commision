namespace JTExpress.Api.Features.Branches;

public interface IBranchesService
{
    Task<IReadOnlyList<BranchDto>> GetBranchesAsync(string? region);
    Task<BranchDto> CreateBranchAsync(CreateBranchDto dto);
    Task<BranchDto> UpdateBranchAsync(int id, UpdateBranchDto dto);
    Task DeleteBranchAsync(int id);
}
