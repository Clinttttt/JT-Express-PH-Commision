namespace JTExpress.Api.Features.Tracking;

public interface ITrackingService
{
    Task<TrackingResultDto?> TrackAsync(string trackingNumber);
}
