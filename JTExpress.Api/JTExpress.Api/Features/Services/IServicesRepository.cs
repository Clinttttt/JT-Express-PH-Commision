namespace JTExpress.Api.Features.Services;

public interface IServicesRepository
{
    Task<IReadOnlyList<ServiceDto>> GetAllAsync();
    Task<ServiceDto> CreateAsync(CreateServiceDto dto);
    Task<ServiceDto?> UpdateAsync(int id, UpdateServiceDto dto);
    Task<bool> DeleteAsync(int id);
}
