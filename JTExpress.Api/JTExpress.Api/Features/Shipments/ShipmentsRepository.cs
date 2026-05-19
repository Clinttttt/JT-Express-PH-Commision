using JTExpress.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace JTExpress.Api.Features.Shipments;

public sealed class ShipmentsRepository(AppDbContext dbContext) : IShipmentsRepository
{
    public async Task<ShipmentDto?> GetByTrackingNumberAsync(string trackingNumber)
    {
        var entity = await dbContext.TrackingResults
            .AsNoTracking()
            .Include(r => r.Timeline)
            .FirstOrDefaultAsync(r => r.TrackingNumber.ToLower() == trackingNumber.ToLower());

        return entity is null ? null : MapToDto(entity);
    }

    public async Task<IReadOnlyList<ShipmentDto>> GetAllAsync()
    {
        var entities = await dbContext.TrackingResults
            .AsNoTracking()
            .Include(r => r.Timeline)
            .OrderByDescending(r => r.Id)
            .ToListAsync();

        return entities.Select(MapToDto).ToList();
    }

    public async Task<ShipmentDto> CreateAsync(CreateShipmentDto dto)
    {
        var entity = new TrackingResultEntity
        {
            TrackingNumber = dto.TrackingNumber,
            Status = "Parcel Picked Up",
            Sender = dto.Sender,
            Recipient = dto.Recipient,
            EstimatedDelivery = dto.EstimatedDelivery,
            CurrentLocation = "Processing"
        };

        dbContext.TrackingResults.Add(entity);
        await dbContext.SaveChangesAsync();

        return MapToDto(entity);
    }

    public async Task<ShipmentDto> UpdateAsync(int id, UpdateShipmentDto dto)
    {
        var entity = await dbContext.TrackingResults.FindAsync(id) 
            ?? throw new InvalidOperationException("Shipment not found");

        entity.Status = dto.Status;
        entity.CurrentLocation = dto.CurrentLocation;

        await dbContext.SaveChangesAsync();

        await dbContext.Entry(entity).Collection(r => r.Timeline).LoadAsync();
        return MapToDto(entity);
    }

    public async Task AddEventAsync(int shipmentId, AddTrackingEventDto dto)
    {
        var entity = await dbContext.TrackingResults.FindAsync(shipmentId)
            ?? throw new InvalidOperationException("Shipment not found");

        var trackingEvent = new TrackingEventEntity
        {
            TrackingResultEntityId = shipmentId,
            Date = dto.Date,
            Status = dto.Status,
            Location = dto.Location
        };

        dbContext.TrackingEvents.Add(trackingEvent);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await dbContext.TrackingResults.FindAsync(id)
            ?? throw new InvalidOperationException("Shipment not found");

        dbContext.TrackingResults.Remove(entity);
        await dbContext.SaveChangesAsync();
    }

    private static ShipmentDto MapToDto(TrackingResultEntity entity)
    {
        var timeline = entity.Timeline
            .OrderByDescending(e => e.Id)
            .Select(e => new ShipmentEventDto(e.Id, e.Date, e.Status, e.Location))
            .ToList();

        return new ShipmentDto(
            entity.Id,
            entity.TrackingNumber,
            entity.Status,
            entity.Sender,
            entity.Recipient,
            entity.EstimatedDelivery,
            entity.CurrentLocation,
            timeline);
    }
}
