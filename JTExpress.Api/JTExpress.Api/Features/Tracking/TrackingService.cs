using JTExpress.Api.Features.Shipments;

namespace JTExpress.Api.Features.Tracking;

public sealed class TrackingService(IShipmentsService shipmentsService) : ITrackingService
{
    public async Task<TrackingResultDto?> TrackAsync(string trackingNumber)
    {
        var shipment = await shipmentsService.GetByTrackingNumberAsync(trackingNumber);
        if (shipment is null)
            return null;

        return new TrackingResultDto(
            shipment.TrackingNumber,
            shipment.Status,
            shipment.Sender,
            shipment.Recipient,
            shipment.EstimatedDelivery,
            shipment.CurrentLocation,
            shipment.Timeline.Select(e => new TrackingEventDto(e.Date, e.Status, e.Location)).ToList());
    }
}
