using JTExpress.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace JTExpress.Api.Features.Tracking;

public sealed class TrackingRepository(AppDbContext dbContext) : ITrackingRepository
{
    public async Task<TrackingResultDto?> GetByTrackingNumberAsync(string trackingNumber)
    {
        var entity = await dbContext.TrackingResults
            .AsNoTracking()
            .Include(result => result.Timeline)
            .FirstOrDefaultAsync(result => result.TrackingNumber.ToLower() == trackingNumber.ToLower());

        if (entity is null)
        {
            return null;
        }

        var timeline = entity.Timeline
            .OrderBy(trackingEvent => trackingEvent.Id)
            .Select(trackingEvent => new TrackingEventDto(
                trackingEvent.Date,
                trackingEvent.Status,
                trackingEvent.Location))
            .ToList();

        return new TrackingResultDto(
            entity.TrackingNumber,
            entity.Status,
            entity.Sender,
            entity.Recipient,
            entity.EstimatedDelivery,
            entity.CurrentLocation,
            timeline);
    }
}
