namespace JTExpress.Api.Features.Shipments;

public interface IShipmentsService
{
    Task<ShipmentDto?> GetByTrackingNumberAsync(string trackingNumber);
    Task<IReadOnlyList<ShipmentDto>> GetAllAsync();
    Task<ShipmentDto> CreateAsync(CreateShipmentDto dto);
    Task<ShipmentDto> UpdateAsync(int id, UpdateShipmentDto dto);
    Task AddEventAsync(int shipmentId, AddTrackingEventDto dto);
    Task DeleteAsync(int id);
}
