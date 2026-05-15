# J&T Express PH — Developer Workflow Guide
> This is a school project. Keep the code simple and readable — no over-engineering.
> Follow the rules (no hardcoded values, thin controllers, hooks only), but don't add things that aren't needed.

---

## How to Use This Guide

Work through each job in order. Each job has a clear goal, the exact files to create, and what "done" looks like. Finish one job completely before starting the next.

Backend jobs come first. Once the API is working and tested in Swagger, move to the frontend jobs.

---

## BACKEND JOBS

---

### Job 1 — Scaffold the API Project

**Goal:** Get a working .NET 8 Web API project on your machine.

```bash
dotnet new webapi -n JTExpress.Api --use-controllers
cd JTExpress.Api
dotnet run
```

Verify it runs and Swagger opens at `http://localhost:5000/swagger`. The default WeatherForecast endpoint can stay for now — you'll delete it in Job 2.

**Done when:** `dotnet run` succeeds and Swagger UI is visible.

---

### Job 2 — Configure the Project

**Goal:** Set up `appsettings.json`, delete boilerplate, and create the folder structure.

**Files to create/edit:**

1. Delete `WeatherForecast.cs` and `Controllers/WeatherForecastController.cs`

2. Edit `appsettings.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "Cors": {
    "AllowedOrigins": [ "http://localhost:5173" ]
  },
  "DataPaths": {
    "Services":  "Data/services.json",
    "Rates":     "Data/rates.json",
    "Tracking":  "Data/tracking.json",
    "Branches":  "Data/branches.json"
  }
}
```

3. Edit `appsettings.Development.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug"
    }
  },
  "Cors": {
    "AllowedOrigins": [ "http://localhost:5173", "http://localhost:3000" ]
  }
}
```

4. Create these empty folders manually (or they'll be created when you add files):
   - `Common/`
   - `Common/Extensions/`
   - `Features/Services/`
   - `Features/Rates/`
   - `Features/Tracking/`
   - `Features/Branches/`
   - `Data/`

**Done when:** `dotnet run` still works with no errors, boilerplate is gone, and folders exist.

---

### Job 3 — Build the Common Layer

**Goal:** Create the three shared files used by all features. Do this once — never touch these again.

**Files to create:**

**`Common/ApiResponse.cs`**
```csharp
namespace JTExpress.Api.Common;

public sealed class ApiResponse<T>
{
    public bool    Success { get; init; }
    public T?      Data    { get; init; }
    public string? Error   { get; init; }

    public static ApiResponse<T> Ok(T data)     => new() { Success = true,  Data = data };
    public static ApiResponse<T> Fail(string e) => new() { Success = false, Error = e };
}
```

**`Common/ExceptionMiddleware.cs`**
```csharp
using System.Net;
using System.Text.Json;

namespace JTExpress.Api.Common;

public sealed class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception for {Path}", context.Request.Path);
            context.Response.StatusCode  = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var response = ApiResponse<object>.Fail("An unexpected error occurred.");
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
```

**`Common/Extensions/ServiceCollectionExtensions.cs`**

Leave this file open — you'll add each feature's DI registration as you build it. For now, create it with an empty method body:

```csharp
namespace JTExpress.Api.Common.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Features registered here as you build them
        return services;
    }
}
```

**`Program.cs`** — replace the whole file:

```csharp
using JTExpress.Api.Common;
using JTExpress.Api.Common.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "J&T Express PH API", Version = "v1" });
});

var allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>() ?? [];

builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactClient", policy =>
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("ReactClient");
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
```

**Done when:** `dotnet run` still compiles clean. Swagger shows no endpoints (that's fine — you'll add them next).

---

### Job 4 — Build the Services Feature (Slice 1)

**Goal:** Complete one full vertical slice — DTO → Repository → Service → Controller — plus its seed data. This is your template for the remaining 3 slices.

**Step 4a — Create seed data**

`Data/services.json`:
```json
[
  { "id": 1, "name": "Express Delivery",  "description": "Next-day delivery within Metro Manila.",          "icon": "⚡", "priceLabel": "₱89 and up"  },
  { "id": 2, "name": "Standard Delivery", "description": "2–5 business days to any province nationwide.",  "icon": "📦", "priceLabel": "₱60 and up"  },
  { "id": 3, "name": "Cash on Delivery",  "description": "Buyer pays only upon receiving the parcel.",     "icon": "💵", "priceLabel": "Free"         },
  { "id": 4, "name": "Bulky Cargo",       "description": "Specialised handling for oversized/heavy items.", "icon": "🏗️", "priceLabel": "Custom rate" },
  { "id": 5, "name": "Door-to-Door",      "description": "Picked up and delivered to your exact address.", "icon": "🚪", "priceLabel": "₱79 and up"  }
]
```

> After adding JSON files: right-click each file in Visual Studio → Properties → "Copy to Output Directory" → "Copy if newer". Otherwise the API won't find them at runtime.

**Step 4b — DTO**

`Features/Services/ServiceDto.cs`:
```csharp
namespace JTExpress.Api.Features.Services;

public sealed record ServiceDto(
    int    Id,
    string Name,
    string Description,
    string Icon,
    string PriceLabel);
```

**Step 4c — Repository interface + implementation**

`Features/Services/IServicesRepository.cs`:
```csharp
namespace JTExpress.Api.Features.Services;

public interface IServicesRepository
{
    Task<IReadOnlyList<ServiceDto>> GetAllAsync();
}
```

`Features/Services/ServicesRepository.cs`:
```csharp
using System.Text.Json;

namespace JTExpress.Api.Features.Services;

public sealed class ServicesRepository : IServicesRepository
{
    private readonly IReadOnlyList<ServiceDto> _cache;

    public ServicesRepository(string filePath, ILogger<ServicesRepository> logger)
    {
        var json = File.ReadAllText(filePath);
        _cache   = JsonSerializer.Deserialize<List<ServiceDto>>(json,
                       new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                   ?? [];
        logger.LogInformation("Loaded {Count} services from {Path}", _cache.Count, filePath);
    }

    public Task<IReadOnlyList<ServiceDto>> GetAllAsync() => Task.FromResult(_cache);
}
```

**Step 4d — Service interface + implementation**

`Features/Services/IServicesService.cs`:
```csharp
namespace JTExpress.Api.Features.Services;

public interface IServicesService
{
    Task<IReadOnlyList<ServiceDto>> GetAllServicesAsync();
}
```

`Features/Services/ServicesService.cs`:
```csharp
namespace JTExpress.Api.Features.Services;

public sealed class ServicesService(IServicesRepository repository) : IServicesService
{
    public Task<IReadOnlyList<ServiceDto>> GetAllServicesAsync() =>
        repository.GetAllAsync();
}
```

**Step 4e — Controller**

`Features/Services/ServicesController.cs`:
```csharp
using JTExpress.Api.Common;
using Microsoft.AspNetCore.Mvc;

namespace JTExpress.Api.Features.Services;

[ApiController]
[Route("api/[controller]")]
public sealed class ServicesController(IServicesService service) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<ServiceDto>>), 200)]
    public async Task<IActionResult> GetAll()
    {
        var result = await service.GetAllServicesAsync();
        return Ok(ApiResponse<IReadOnlyList<ServiceDto>>.Ok(result));
    }
}
```

**Step 4f — Register in DI**

Open `Common/Extensions/ServiceCollectionExtensions.cs` and add:

```csharp
using JTExpress.Api.Features.Services;

// inside AddApplicationServices:
services.AddSingleton<IServicesRepository>(sp =>
    new ServicesRepository(configuration["DataPaths:Services"]!,
                           sp.GetRequiredService<ILogger<ServicesRepository>>()));
services.AddScoped<IServicesService, ServicesService>();
```

**Done when:** `dotnet run` → Swagger → `GET /api/services` → returns all 5 services as JSON.

---

### Job 5 — Build the Branches Feature (Slice 2)

**Goal:** Branch list + optional region filter.

**Step 5a — Seed data**

`Data/branches.json`:
```json
[
  { "id": 1, "name": "J&T Manila Main",  "address": "1234 Taft Ave, Ermita, Manila",            "region": "NCR",      "phone": "(02) 8123-4567",  "hours": "Mon-Sat 8AM-6PM", "latitude": 14.5764, "longitude": 120.9890 },
  { "id": 2, "name": "J&T Quezon City",  "address": "456 EDSA, Quezon City",                    "region": "NCR",      "phone": "(02) 8234-5678",  "hours": "Mon-Sat 8AM-6PM", "latitude": 14.6760, "longitude": 121.0437 },
  { "id": 3, "name": "J&T Cebu Main",    "address": "789 Colon St, Cebu City",                  "region": "Visayas",  "phone": "(032) 123-4567",  "hours": "Mon-Sat 8AM-6PM", "latitude": 10.2931, "longitude": 123.8995 },
  { "id": 4, "name": "J&T Davao",        "address": "321 Rizal St, Davao City",                 "region": "Mindanao", "phone": "(082) 234-5678",  "hours": "Mon-Sat 8AM-6PM", "latitude":  7.0644, "longitude": 125.6077 },
  { "id": 5, "name": "J&T Pampanga",     "address": "SM Clark, Angeles City, Pampanga",         "region": "Luzon",    "phone": "(045) 345-6789",  "hours": "Mon-Sat 8AM-6PM", "latitude": 15.1350, "longitude": 120.5960 },
  { "id": 6, "name": "J&T Iloilo",       "address": "SM City Iloilo, Mandurriao, Iloilo City",  "region": "Visayas",  "phone": "(033) 456-7890",  "hours": "Mon-Sat 8AM-6PM", "latitude": 10.7202, "longitude": 122.5621 }
]
```

**Step 5b — DTO**

`Features/Branches/BranchDto.cs`:
```csharp
namespace JTExpress.Api.Features.Branches;

public sealed record BranchDto(
    int    Id,
    string Name,
    string Address,
    string Region,
    string Phone,
    string Hours,
    double Latitude,
    double Longitude);
```

**Step 5c — Repository**

`Features/Branches/IBranchesRepository.cs`:
```csharp
namespace JTExpress.Api.Features.Branches;

public interface IBranchesRepository
{
    Task<IReadOnlyList<BranchDto>> GetAllAsync();
    Task<IReadOnlyList<BranchDto>> GetByRegionAsync(string region);
}
```

`Features/Branches/BranchesRepository.cs`:
```csharp
using System.Text.Json;

namespace JTExpress.Api.Features.Branches;

public sealed class BranchesRepository : IBranchesRepository
{
    private readonly IReadOnlyList<BranchDto> _cache;

    public BranchesRepository(string filePath, ILogger<BranchesRepository> logger)
    {
        var json = File.ReadAllText(filePath);
        _cache   = JsonSerializer.Deserialize<List<BranchDto>>(json,
                       new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                   ?? [];
        logger.LogInformation("Loaded {Count} branches from {Path}", _cache.Count, filePath);
    }

    public Task<IReadOnlyList<BranchDto>> GetAllAsync() => Task.FromResult(_cache);

    public Task<IReadOnlyList<BranchDto>> GetByRegionAsync(string region)
    {
        IReadOnlyList<BranchDto> result = _cache
            .Where(b => b.Region.Equals(region, StringComparison.OrdinalIgnoreCase))
            .ToList();
        return Task.FromResult(result);
    }
}
```

**Step 5d — Service**

`Features/Branches/IBranchesService.cs`:
```csharp
namespace JTExpress.Api.Features.Branches;

public interface IBranchesService
{
    Task<IReadOnlyList<BranchDto>> GetBranchesAsync(string? region);
}
```

`Features/Branches/BranchesService.cs`:
```csharp
namespace JTExpress.Api.Features.Branches;

public sealed class BranchesService(IBranchesRepository repository) : IBranchesService
{
    public Task<IReadOnlyList<BranchDto>> GetBranchesAsync(string? region) =>
        string.IsNullOrWhiteSpace(region)
            ? repository.GetAllAsync()
            : repository.GetByRegionAsync(region);
}
```

**Step 5e — Controller**

`Features/Branches/BranchesController.cs`:
```csharp
using JTExpress.Api.Common;
using Microsoft.AspNetCore.Mvc;

namespace JTExpress.Api.Features.Branches;

[ApiController]
[Route("api/[controller]")]
public sealed class BranchesController(IBranchesService service) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<BranchDto>>), 200)]
    public async Task<IActionResult> GetAll([FromQuery] string? region = null)
    {
        var result = await service.GetBranchesAsync(region);
        return Ok(ApiResponse<IReadOnlyList<BranchDto>>.Ok(result));
    }
}
```

**Step 5f — Register in DI** (add to `ServiceCollectionExtensions.cs`)

```csharp
using JTExpress.Api.Features.Branches;

services.AddSingleton<IBranchesRepository>(sp =>
    new BranchesRepository(configuration["DataPaths:Branches"]!,
                           sp.GetRequiredService<ILogger<BranchesRepository>>()));
services.AddScoped<IBranchesService, BranchesService>();
```

**Done when:**
- `GET /api/branches` returns all 6 branches
- `GET /api/branches?region=NCR` returns only the 2 NCR branches

---

### Job 6 — Build the Rates Feature (Slice 3)

**Goal:** Rate table + calculate endpoint.

**Step 6a — Seed data**

`Data/rates.json`:
```json
[
  { "zone": "Metro Manila", "firstKg": 89,  "succeedingKg": 20 },
  { "zone": "Luzon",        "firstKg": 99,  "succeedingKg": 30 },
  { "zone": "Visayas",      "firstKg": 119, "succeedingKg": 40 },
  { "zone": "Mindanao",     "firstKg": 129, "succeedingKg": 45 }
]
```

**Step 6b — DTOs**

`Features/Rates/RateDto.cs`:
```csharp
namespace JTExpress.Api.Features.Rates;

public sealed record RateDto(string Zone, decimal FirstKg, decimal SucceedingKg);

public sealed record RateCalculationResultDto(
    string  Zone,
    double  Weight,
    decimal EstimatedRate,
    string  FormattedRate);
```

**Step 6c — Repository**

`Features/Rates/IRatesRepository.cs`:
```csharp
namespace JTExpress.Api.Features.Rates;

public interface IRatesRepository
{
    Task<IReadOnlyList<RateDto>> GetAllAsync();
    Task<RateDto?> GetByZoneAsync(string zone);
}
```

`Features/Rates/RatesRepository.cs`:
```csharp
using System.Text.Json;

namespace JTExpress.Api.Features.Rates;

public sealed class RatesRepository : IRatesRepository
{
    private readonly IReadOnlyList<RateDto> _cache;

    public RatesRepository(string filePath, ILogger<RatesRepository> logger)
    {
        var json = File.ReadAllText(filePath);
        _cache   = JsonSerializer.Deserialize<List<RateDto>>(json,
                       new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                   ?? [];
        logger.LogInformation("Loaded {Count} rate zones from {Path}", _cache.Count, filePath);
    }

    public Task<IReadOnlyList<RateDto>> GetAllAsync() => Task.FromResult(_cache);

    public Task<RateDto?> GetByZoneAsync(string zone) =>
        Task.FromResult(_cache.FirstOrDefault(r =>
            r.Zone.Equals(zone, StringComparison.OrdinalIgnoreCase)));
}
```

**Step 6d — Service**

`Features/Rates/IRatesService.cs`:
```csharp
namespace JTExpress.Api.Features.Rates;

public interface IRatesService
{
    Task<IReadOnlyList<RateDto>> GetAllRatesAsync();
    Task<RateCalculationResultDto?> CalculateAsync(string zone, double weight);
}
```

`Features/Rates/RatesService.cs`:
```csharp
namespace JTExpress.Api.Features.Rates;

public sealed class RatesService(IRatesRepository repository) : IRatesService
{
    public Task<IReadOnlyList<RateDto>> GetAllRatesAsync() =>
        repository.GetAllAsync();

    public async Task<RateCalculationResultDto?> CalculateAsync(string zone, double weight)
    {
        var rate = await repository.GetByZoneAsync(zone);
        if (rate is null) return null;

        // Formula: firstKg + (ceiling of extra kg × succeedingKg rate)
        var extra = weight > 1 ? Math.Ceiling(weight - 1) * (double)rate.SucceedingKg : 0;
        var total = rate.FirstKg + (decimal)extra;

        return new RateCalculationResultDto(
            Zone:          rate.Zone,
            Weight:        weight,
            EstimatedRate: total,
            FormattedRate: $"₱{total:N0}");
    }
}
```

**Step 6e — Controller**

`Features/Rates/RatesController.cs`:
```csharp
using JTExpress.Api.Common;
using Microsoft.AspNetCore.Mvc;

namespace JTExpress.Api.Features.Rates;

[ApiController]
[Route("api/[controller]")]
public sealed class RatesController(IRatesService service) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<RateDto>>), 200)]
    public async Task<IActionResult> GetAll()
    {
        var result = await service.GetAllRatesAsync();
        return Ok(ApiResponse<IReadOnlyList<RateDto>>.Ok(result));
    }

    [HttpGet("calculate")]
    [ProducesResponseType(typeof(ApiResponse<RateCalculationResultDto>), 200)]
    [ProducesResponseType(typeof(ApiResponse<object>), 400)]
    public async Task<IActionResult> Calculate([FromQuery] string zone, [FromQuery] double weight)
    {
        if (string.IsNullOrWhiteSpace(zone) || weight <= 0)
            return BadRequest(ApiResponse<object>.Fail("Zone and a positive weight are required."));

        var result = await service.CalculateAsync(zone, weight);

        if (result is null)
            return BadRequest(ApiResponse<object>.Fail($"Zone '{zone}' was not found."));

        return Ok(ApiResponse<RateCalculationResultDto>.Ok(result));
    }
}
```

**Step 6f — Register in DI**

```csharp
using JTExpress.Api.Features.Rates;

services.AddSingleton<IRatesRepository>(sp =>
    new RatesRepository(configuration["DataPaths:Rates"]!,
                        sp.GetRequiredService<ILogger<RatesRepository>>()));
services.AddScoped<IRatesService, RatesService>();
```

**Done when:**
- `GET /api/rates` returns all 4 zones
- `GET /api/rates/calculate?zone=Metro Manila&weight=2.3` returns `₱129`
- `GET /api/rates/calculate?zone=fake&weight=1` returns `400` with error message

---

### Job 7 — Build the Tracking Feature (Slice 4)

**Goal:** Track a parcel by tracking number, return full timeline.

**Step 7a — Seed data**

`Data/tracking.json`:
```json
[
  {
    "trackingNumber": "JT123456789PH",
    "status": "Out for Delivery",
    "sender": "Manila Warehouse",
    "recipient": "Juan Dela Cruz",
    "estimatedDelivery": "Today",
    "timeline": [
      { "date": "May 14 08:00 AM", "status": "Out for Delivery", "location": "Cebu City Hub" },
      { "date": "May 13 10:00 PM", "status": "Arrived at Hub",   "location": "Cebu City Hub" },
      { "date": "May 13 02:00 PM", "status": "In Transit",       "location": "Manila Warehouse" },
      { "date": "May 12 09:00 AM", "status": "Parcel Picked Up", "location": "Quezon City" }
    ]
  },
  {
    "trackingNumber": "JT987654321PH",
    "status": "Delivered",
    "sender": "Lazada Seller",
    "recipient": "Maria Santos",
    "estimatedDelivery": "Delivered on May 13",
    "timeline": [
      { "date": "May 13 03:00 PM", "status": "Delivered",        "location": "Makati City" },
      { "date": "May 13 09:00 AM", "status": "Out for Delivery", "location": "Makati Hub" },
      { "date": "May 12 11:00 PM", "status": "Arrived at Hub",   "location": "Makati Hub" }
    ]
  }
]
```

**Step 7b — DTOs**

`Features/Tracking/TrackingDto.cs`:
```csharp
namespace JTExpress.Api.Features.Tracking;

public sealed record TrackingEventDto(string Date, string Status, string Location);

public sealed record TrackingResultDto(
    string                          TrackingNumber,
    string                          Status,
    string                          Sender,
    string                          Recipient,
    string                          EstimatedDelivery,
    IReadOnlyList<TrackingEventDto> Timeline);
```

**Step 7c — Repository**

`Features/Tracking/ITrackingRepository.cs`:
```csharp
namespace JTExpress.Api.Features.Tracking;

public interface ITrackingRepository
{
    Task<TrackingResultDto?> GetByTrackingNumberAsync(string trackingNumber);
}
```

`Features/Tracking/TrackingRepository.cs`:
```csharp
using System.Text.Json;

namespace JTExpress.Api.Features.Tracking;

public sealed class TrackingRepository : ITrackingRepository
{
    private readonly Dictionary<string, TrackingResultDto> _cache;

    public TrackingRepository(string filePath, ILogger<TrackingRepository> logger)
    {
        var json = File.ReadAllText(filePath);
        var list = JsonSerializer.Deserialize<List<TrackingResultDto>>(json,
                       new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                   ?? [];

        // Store by uppercase key so lookup is case-insensitive
        _cache = list.ToDictionary(t => t.TrackingNumber.ToUpperInvariant());
        logger.LogInformation("Loaded {Count} tracking records from {Path}", _cache.Count, filePath);
    }

    public Task<TrackingResultDto?> GetByTrackingNumberAsync(string trackingNumber) =>
        Task.FromResult(_cache.GetValueOrDefault(trackingNumber.ToUpperInvariant()));
}
```

**Step 7d — Service**

`Features/Tracking/ITrackingService.cs`:
```csharp
namespace JTExpress.Api.Features.Tracking;

public interface ITrackingService
{
    Task<TrackingResultDto?> TrackAsync(string trackingNumber);
}
```

`Features/Tracking/TrackingService.cs`:
```csharp
namespace JTExpress.Api.Features.Tracking;

public sealed class TrackingService(ITrackingRepository repository) : ITrackingService
{
    public Task<TrackingResultDto?> TrackAsync(string trackingNumber) =>
        repository.GetByTrackingNumberAsync(trackingNumber);
}
```

**Step 7e — Controller**

`Features/Tracking/TrackingController.cs`:
```csharp
using JTExpress.Api.Common;
using Microsoft.AspNetCore.Mvc;

namespace JTExpress.Api.Features.Tracking;

[ApiController]
[Route("api/[controller]")]
public sealed class TrackingController(ITrackingService service) : ControllerBase
{
    [HttpGet("{trackingNumber}")]
    [ProducesResponseType(typeof(ApiResponse<TrackingResultDto>), 200)]
    [ProducesResponseType(typeof(ApiResponse<object>), 404)]
    public async Task<IActionResult> Track(string trackingNumber)
    {
        if (string.IsNullOrWhiteSpace(trackingNumber))
            return BadRequest(ApiResponse<object>.Fail("Tracking number is required."));

        var result = await service.TrackAsync(trackingNumber);

        if (result is null)
            return NotFound(ApiResponse<object>.Fail(
                $"No parcel found for tracking number '{trackingNumber}'."));

        return Ok(ApiResponse<TrackingResultDto>.Ok(result));
    }
}
```

**Step 7f — Register in DI**

```csharp
using JTExpress.Api.Features.Tracking;

services.AddSingleton<ITrackingRepository>(sp =>
    new TrackingRepository(configuration["DataPaths:Tracking"]!,
                           sp.GetRequiredService<ILogger<TrackingRepository>>()));
services.AddScoped<ITrackingService, TrackingService>();
```

**Done when:**
- `GET /api/tracking/JT123456789PH` → full tracking result with timeline
- `GET /api/tracking/jt123456789ph` → same result (case-insensitive)
- `GET /api/tracking/FAKE999` → `404` with error message

---

### Job 8 — Final Backend Verification

Before moving to the frontend, test every endpoint in Swagger.

| Endpoint | Expected Result |
|----------|----------------|
| `GET /api/services` | 200 — array of 5 services |
| `GET /api/branches` | 200 — array of 6 branches |
| `GET /api/branches?region=Visayas` | 200 — 2 branches |
| `GET /api/branches?region=fake` | 200 — empty array (not an error) |
| `GET /api/rates` | 200 — array of 4 zones |
| `GET /api/rates/calculate?zone=Metro Manila&weight=1` | 200 — ₱89 |
| `GET /api/rates/calculate?zone=Metro Manila&weight=2.3` | 200 — ₱129 |
| `GET /api/rates/calculate?zone=&weight=1` | 400 — validation error |
| `GET /api/tracking/JT123456789PH` | 200 — full tracking result |
| `GET /api/tracking/FAKE999` | 404 — not found error |

**Done when:** All rows above pass. API is complete. Leave `dotnet run` running.

---

## FRONTEND JOBS

---

### Job 9 — Scaffold the React Project

**Goal:** Working Vite + React TypeScript project with CSS custom properties ready.

```bash
npm create vite@latest jt-express-web -- --template react-ts
cd jt-express-web
npm install react-router-dom axios
npm run dev
```

Verify the default Vite page loads at `http://localhost:5173`.

Then clean up the scaffold:
- Delete `src/assets/react.svg`
- Delete everything inside `src/App.css` (keep the file, clear its contents)
- Replace `src/index.css` with your global styles (see UI Guide for the full CSS)
- Replace `src/App.tsx` content with a blank component for now

Create the env files:

`.env`:
```
VITE_API_BASE_URL=http://localhost:5000/api
```

`.env.production`:
```
VITE_API_BASE_URL=https://your-production-api.com/api
```

**Done when:** `npm run dev` runs clean with no errors.

---

### Job 10 — Wire the API Layer

**Goal:** Set up the env config, Axios client, shared types, and all 4 API endpoint files. None of these are visible yet — they're infrastructure.

Create these files in order:

**`src/config/env.ts`**
```ts
const env = {
  apiBaseUrl: import.meta.env.VITE_API_BASE_URL as string,
} as const;

if (!env.apiBaseUrl) {
  throw new Error("VITE_API_BASE_URL is not defined. Check your .env file.");
}

export default env;
```

**`src/api/client.ts`**
```ts
import axios from "axios";
import env from "../config/env";

const apiClient = axios.create({
  baseURL: env.apiBaseUrl,
  headers: { "Content-Type": "application/json" },
  timeout: 10_000,
});

apiClient.interceptors.response.use(
  (response) => {
    const body = response.data;
    if (body && body.success === false) {
      return Promise.reject(new Error(body.error ?? "An error occurred."));
    }
    return response;
  },
  (error) => {
    const message =
      error.response?.data?.error ??
      error.message ??
      "Network error. Please try again.";
    return Promise.reject(new Error(message));
  }
);

export default apiClient;
```

**`src/types/index.ts`**

Paste the full types from the Production Guide (ApiResponse, Service, Rate, RateCalculationResult, TrackingEvent, TrackingResult, Branch).

**`src/api/endpoints/servicesApi.ts`**
```ts
import apiClient from "../client";
import type { ApiResponse, Service } from "../../types";

export const fetchServices = async (): Promise<Service[]> => {
  const res = await apiClient.get<ApiResponse<Service[]>>("/services");
  return res.data.data ?? [];
};
```

**`src/api/endpoints/ratesApi.ts`**
```ts
import apiClient from "../client";
import type { ApiResponse, Rate, RateCalculationResult } from "../../types";

export const fetchRates = async (): Promise<Rate[]> => {
  const res = await apiClient.get<ApiResponse<Rate[]>>("/rates");
  return res.data.data ?? [];
};

export const calculateRate = async (
  zone: string,
  weight: number
): Promise<RateCalculationResult> => {
  const res = await apiClient.get<ApiResponse<RateCalculationResult>>(
    "/rates/calculate",
    { params: { zone, weight } }
  );
  return res.data.data!;
};
```

**`src/api/endpoints/trackingApi.ts`**
```ts
import apiClient from "../client";
import type { ApiResponse, TrackingResult } from "../../types";

export const trackParcel = async (trackingNumber: string): Promise<TrackingResult> => {
  const res = await apiClient.get<ApiResponse<TrackingResult>>(
    `/tracking/${encodeURIComponent(trackingNumber)}`
  );
  return res.data.data!;
};
```

**`src/api/endpoints/branchesApi.ts`**
```ts
import apiClient from "../client";
import type { ApiResponse, Branch } from "../../types";

export const fetchBranches = async (region?: string): Promise<Branch[]> => {
  const params = region ? { region } : {};
  const res = await apiClient.get<ApiResponse<Branch[]>>("/branches", { params });
  return res.data.data ?? [];
};
```

**Done when:** Files exist, TypeScript compiles clean (`npm run build` with no errors).

---

### Job 11 — Build Shared Components

**Goal:** Create the 4 shared components used across all pages. These are small — build them all in one sitting.

Follow the `.tsx` + `.module.css` pattern from the UI Guide for each one. Each component lives in its own folder under `src/components/shared/`.

Build in this order: `LoadingSpinner` → `ErrorMessage` → `Navbar` → `Footer`.

Refer to the UI Guide for the exact markup and CSS for each component. Keep them simple — no props beyond what the pages actually need.

**Done when:** All 4 components exist and TypeScript is clean.

---

### Job 12 — Build Each Feature Page

Build one feature at a time. For each: write the hook first, then the page component. Don't move to the next feature until the current one renders correctly in the browser.

**Order:** Services → Branches → Rates → Tracking

For each feature the steps are the same:

1. Create `src/features/[feature]/hooks/use[Feature].ts`
2. Create `src/features/[feature]/[Feature]Page.tsx`
3. Create `src/features/[feature]/[Feature]Page.module.css`
4. Add the route temporarily in `App.tsx` and test in the browser

**Simple rule for hooks:** Every hook returns `{ data, loading, error }` at minimum. Don't put display logic inside hooks. Don't put fetch logic inside page components.

**Done when:** All 4 pages load data from the live API and show loading/error states correctly.

---

### Job 13 — Build the Home Page

**Goal:** A static page with a hero section, 4 quick-access cards, and a "Why J&T" section. No API calls.

Create `src/features/home/HomePage.tsx` and `HomePage.module.css`.

The home page just has links and text — it is the simplest page. Build it last so you already know all the routes.

**Done when:** Home page renders, all 4 quick-access cards link to the correct routes.

---

### Job 14 — Wire App.tsx and Final Check

**Goal:** Connect all routes, verify everything works end-to-end.

**`src/App.tsx`**
```tsx
import { BrowserRouter, Routes, Route } from "react-router-dom";
import Navbar       from "./components/shared/Navbar/Navbar";
import Footer       from "./components/shared/Footer/Footer";
import HomePage     from "./features/home/HomePage";
import ServicesPage from "./features/services/ServicesPage";
import RatesPage    from "./features/rates/RatesPage";
import TrackingPage from "./features/tracking/TrackingPage";
import BranchesPage from "./features/branches/BranchesPage";

export default function App() {
  return (
    <BrowserRouter>
      <div className="app-shell">
        <Navbar />
        <main className="app-main">
          <Routes>
            <Route path="/"         element={<HomePage />}     />
            <Route path="/services" element={<ServicesPage />}  />
            <Route path="/rates"    element={<RatesPage />}     />
            <Route path="/tracking" element={<TrackingPage />}  />
            <Route path="/branches" element={<BranchesPage />}  />
          </Routes>
        </main>
        <Footer />
      </div>
    </BrowserRouter>
  );
}
```

**Final checklist before submitting:**

- [ ] Backend: `dotnet run` starts with no errors, all 4 JSON files loaded (check console logs)
- [ ] Frontend: `npm run dev` starts with no errors
- [ ] Home page: renders, CTA button and 4 cards work
- [ ] Services page: loads from API, cards display correctly
- [ ] Rates page: table loads, calculator returns correct result
- [ ] Tracking page: `JT123456789PH` shows timeline, fake number shows error
- [ ] Branches page: all branches load, region filter works
- [ ] Navbar: active link is highlighted on all pages
- [ ] No hardcoded data anywhere in `.tsx` or `.cs` files
- [ ] TypeScript: `npm run build` produces no type errors

---

## Simple Rules to Remember

**Backend**
- If you're writing business logic in a controller, it belongs in the service.
- If you're writing a `new` keyword in a controller or service, it probably belongs in the DI registration.
- If you're writing a file path as a string literal in C#, it belongs in `appsettings.json`.

**Frontend**
- If you're calling `axios` inside a component, move it to a hook.
- If you're writing `import.meta.env` inside a component, move it to `env.ts`.
- If you're writing the same loading spinner markup twice, move it to `LoadingSpinner`.
- If something is unclear, keep it simple — this is a project assignment, not production software.
