using System;
using System.Collections.Generic;

namespace JTExpress.Api;

public partial class Branch
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string Region { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Hours { get; set; } = null!;

    public double Latitude { get; set; }

    public double Longitude { get; set; }
}
