namespace JTExpress.Api.Features.Services;

public interface IServicesService
{
    Task<IReadOnlyList<ServiceDto>> GetAllServicesAsync();
    Task<ServiceDto> CreateServiceAsync(CreateServiceDto dto);
    Task<ServiceDto?> UpdateServiceAsync(int id, UpdateServiceDto dto);
    Task<bool> DeleteServiceAsync(int id);
}
