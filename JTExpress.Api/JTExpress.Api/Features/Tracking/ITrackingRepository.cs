namespace JTExpress.Api.Features.Tracking;

public interface ITrackingRepository
{
    Task<TrackingResultDto?> GetByTrackingNumberAsync(string trackingNumber);
}
