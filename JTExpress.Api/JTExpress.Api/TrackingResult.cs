using System;
using System.Collections.Generic;

namespace JTExpress.Api;

public partial class TrackingResult
{
    public int Id { get; set; }

    public string TrackingNumber { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string Sender { get; set; } = null!;

    public string Recipient { get; set; } = null!;

    public string EstimatedDelivery { get; set; } = null!;

    public string CurrentLocation { get; set; } = null!;

    public virtual ICollection<TrackingEvent> TrackingEvents { get; set; } = new List<TrackingEvent>();
}
