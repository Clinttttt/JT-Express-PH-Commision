using System;
using System.Collections.Generic;

namespace JTExpress.Api;

public partial class TrackingEvent
{
    public int Id { get; set; }

    public int TrackingResultEntityId { get; set; }

    public string Date { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string Location { get; set; } = null!;

    public virtual TrackingResult TrackingResultEntity { get; set; } = null!;
}
