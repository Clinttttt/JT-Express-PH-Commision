namespace JTExpress.Api.Features.Services;

public sealed class ServicesService(IServicesRepository repository) : IServicesService
{
    public Task<IReadOnlyList<ServiceDto>> GetAllServicesAsync()
    {
        return repository.GetAllAsync();
    }

    public Task<ServiceDto> CreateServiceAsync(CreateServiceDto dto)
    {
        return repository.CreateAsync(dto);
    }

    public Task<ServiceDto?> UpdateServiceAsync(int id, UpdateServiceDto dto)
    {
        return repository.UpdateAsync(id, dto);
    }

    public Task<bool> DeleteServiceAsync(int id)
    {
        return repository.DeleteAsync(id);
    }
}
