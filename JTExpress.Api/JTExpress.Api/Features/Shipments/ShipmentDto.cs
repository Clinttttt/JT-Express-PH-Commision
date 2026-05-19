namespace JTExpress.Api.Features.Shipments;

public sealed record ShipmentDto(
    int Id,
    string TrackingNumber,
    string Status,
    string Sender,
    string Recipient,
    string EstimatedDelivery,
    string CurrentLocation,
    List<ShipmentEventDto> Timeline);

public sealed record CreateShipmentDto(
    string TrackingNumber,
    string Sender,
    string Recipient,
    string EstimatedDelivery);

public sealed record UpdateShipmentDto(
    string Status,
    string CurrentLocation);

public sealed record ShipmentEventDto(
    int Id,
    string Date,
    string Status,
    string Location);

public sealed record AddTrackingEventDto(
    string Date,
    string Status,
    string Location);
