using System;
using System.Collections.Generic;

namespace JTExpress.Api;

public partial class Rate
{
    public int Id { get; set; }

    public string Zone { get; set; } = null!;

    public decimal FirstKg { get; set; }

    public decimal SucceedingKg { get; set; }
}
