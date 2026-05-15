namespace JTExpress.Api.Features.Tracking;

public sealed record TrackingResultDto(
    string TrackingNumber,
    string Status,
    string Sender,
    string Recipient,
    string EstimatedDelivery,
    string CurrentLocation,
    IReadOnlyList<TrackingEventDto> Timeline);
