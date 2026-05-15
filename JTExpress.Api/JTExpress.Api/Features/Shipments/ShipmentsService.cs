namespace JTExpress.Api.Features.Shipments;

public sealed class ShipmentsService(IShipmentsRepository repository) : IShipmentsService
{
    public Task<ShipmentDto?> GetByTrackingNumberAsync(string trackingNumber) =>
        repository.GetByTrackingNumberAsync(trackingNumber);

    public Task<IReadOnlyList<ShipmentDto>> GetAllAsync() =>
        repository.GetAllAsync();

    public Task<ShipmentDto> CreateAsync(CreateShipmentDto dto) =>
        repository.CreateAsync(dto);

    public Task<ShipmentDto> UpdateAsync(int id, UpdateShipmentDto dto) =>
        repository.UpdateAsync(id, dto);

    public Task AddEventAsync(int shipmentId, AddTrackingEventDto dto) =>
        repository.AddEventAsync(shipmentId, dto);

    public Task DeleteAsync(int id) =>
        repository.DeleteAsync(id);
}
