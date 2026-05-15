# J&T Express PH — Production-Ready Build Guide
> Stack: .NET Core 8 Web API + React TypeScript (Vite)
> Pattern: **Vertical Slice Architecture** + Repository + Service Layer

---

## Architecture Overview

```
Request → Controller → Service (IService) → Repository (IRepository) → JSON Data Store
                ↓
        ApiResponse<T> wrapper → Client
```

Each **feature slice** (Services, Rates, Tracking, Branches) is fully self-contained inside its own folder. A slice owns its Controller, Service, Repository, DTOs, and interfaces — nothing leaks across slices. The only shared code is infrastructure that applies to all slices: `ApiResponse<T>`, `ExceptionMiddleware`, and DI registration.

**Rules that apply everywhere:**
- Controllers route only — zero business logic
- Services own all business logic and calculations
- Repositories own all data access (swappable without touching services or controllers)
- No magic strings — all config lives in `appsettings.json`
- No hardcoded data — seed data lives in `Data/*.json` files

---

## Solution Structure

```
JTExpress/
│
├── JTExpress.Api/
│   ├── Common/
│   │   ├── ApiResponse.cs
│   │   ├── ExceptionMiddleware.cs
│   │   └── Extensions/
│   │       └── ServiceCollectionExtensions.cs
│   │
│   ├── Features/
│   │   ├── Services/
│   │   │   ├── ServicesController.cs
│   │   │   ├── IServicesService.cs
│   │   │   ├── ServicesService.cs
│   │   │   ├── IServicesRepository.cs
│   │   │   ├── ServicesRepository.cs
│   │   │   └── ServiceDto.cs
│   │   ├── Rates/
│   │   │   ├── RatesController.cs
│   │   │   ├── IRatesService.cs
│   │   │   ├── RatesService.cs
│   │   │   ├── IRatesRepository.cs
│   │   │   ├── RatesRepository.cs
│   │   │   └── RateDto.cs
│   │   ├── Tracking/
│   │   │   ├── TrackingController.cs
│   │   │   ├── ITrackingService.cs
│   │   │   ├── TrackingService.cs
│   │   │   ├── ITrackingRepository.cs
│   │   │   ├── TrackingRepository.cs
│   │   │   └── TrackingDto.cs
│   │   └── Branches/
│   │       ├── BranchesController.cs
│   │       ├── IBranchesService.cs
│   │       ├── BranchesService.cs
│   │       ├── IBranchesRepository.cs
│   │       ├── BranchesRepository.cs
│   │       └── BranchDto.cs
│   │
│   ├── Data/
│   │   ├── services.json
│   │   ├── rates.json
│   │   ├── tracking.json
│   │   └── branches.json
│   │
│   ├── appsettings.json
│   ├── appsettings.Development.json
│   └── Program.cs
│
└── jt-express-web/
    ├── src/
    │   ├── api/
    │   │   ├── client.ts
    │   │   └── endpoints/
    │   │       ├── servicesApi.ts
    │   │       ├── ratesApi.ts
    │   │       ├── trackingApi.ts
    │   │       └── branchesApi.ts
    │   ├── features/
    │   │   ├── home/
    │   │   │   ├── HomePage.tsx
    │   │   │   └── HomePage.module.css
    │   │   ├── services/
    │   │   │   ├── hooks/useServices.ts
    │   │   │   ├── ServicesPage.tsx
    │   │   │   └── ServicesPage.module.css
    │   │   ├── rates/
    │   │   │   ├── hooks/useRates.ts
    │   │   │   ├── RatesPage.tsx
    │   │   │   └── RatesPage.module.css
    │   │   ├── tracking/
    │   │   │   ├── hooks/useTracking.ts
    │   │   │   ├── TrackingPage.tsx
    │   │   │   └── TrackingPage.module.css
    │   │   └── branches/
    │   │       ├── hooks/useBranches.ts
    │   │       ├── BranchesPage.tsx
    │   │       └── BranchesPage.module.css
    │   ├── components/
    │   │   └── shared/
    │   │       ├── Navbar/
    │   │       │   ├── Navbar.tsx
    │   │       │   └── Navbar.module.css
    │   │       ├── Footer/
    │   │       │   ├── Footer.tsx
    │   │       │   └── Footer.module.css
    │   │       ├── LoadingSpinner/
    │   │       │   ├── LoadingSpinner.tsx
    │   │       │   └── LoadingSpinner.module.css
    │   │       └── ErrorMessage/
    │   │           ├── ErrorMessage.tsx
    │   │           └── ErrorMessage.module.css
    │   ├── styles/
    │   │   ├── globals.css        ← reset + CSS custom properties (design tokens)
    │   │   └── variables.css      ← --color-*, --spacing-*, --font-* tokens
    │   ├── types/
    │   │   └── index.ts
    │   ├── config/
    │   │   └── env.ts
    │   └── App.tsx
    ├── .env
    ├── .env.production
    └── vite.config.ts
```

> **Why no Tailwind?** This guide uses CSS Modules + CSS custom properties. Each component owns its own `.module.css` file. Styles are scoped — no class name collisions, no purge config, no utility soup. See the UI Guide for full details.

---

## BACKEND — .NET Core 8 Web API

### Setup

```bash
dotnet new webapi -n JTExpress.Api --use-controllers
cd JTExpress.Api
# No extra NuGet packages needed — all patterns use built-in .NET DI and System.Text.Json
```

---

### `appsettings.json`

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

### `appsettings.Development.json`

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

> **Rule:** Never hardcode URLs or paths in C# files. Always read from `IConfiguration`.

---

### `Program.cs`

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

---

### `Common/ApiResponse.cs`

All endpoints return this — consistent shape for the frontend.

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

---

### `Common/ExceptionMiddleware.cs`

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

---

### `Common/Extensions/ServiceCollectionExtensions.cs`

All DI registrations in one place — `Program.cs` stays clean.

```csharp
using JTExpress.Api.Features.Services;
using JTExpress.Api.Features.Rates;
using JTExpress.Api.Features.Tracking;
using JTExpress.Api.Features.Branches;

namespace JTExpress.Api.Common.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<IServicesRepository>(sp =>
            new ServicesRepository(configuration["DataPaths:Services"]!,
                                   sp.GetRequiredService<ILogger<ServicesRepository>>()));
        services.AddScoped<IServicesService, ServicesService>();

        services.AddSingleton<IRatesRepository>(sp =>
            new RatesRepository(configuration["DataPaths:Rates"]!,
                                sp.GetRequiredService<ILogger<RatesRepository>>()));
        services.AddScoped<IRatesService, RatesService>();

        services.AddSingleton<ITrackingRepository>(sp =>
            new TrackingRepository(configuration["DataPaths:Tracking"]!,
                                   sp.GetRequiredService<ILogger<TrackingRepository>>()));
        services.AddScoped<ITrackingService, TrackingService>();

        services.AddSingleton<IBranchesRepository>(sp =>
            new BranchesRepository(configuration["DataPaths:Branches"]!,
                                   sp.GetRequiredService<ILogger<BranchesRepository>>()));
        services.AddScoped<IBranchesService, BranchesService>();

        return services;
    }
}
```

---

## Feature: Services

### `Data/services.json`

```json
[
  { "id": 1, "name": "Express Delivery",  "description": "Next-day delivery within Metro Manila.",         "icon": "⚡", "priceLabel": "₱89 and up"  },
  { "id": 2, "name": "Standard Delivery", "description": "2–5 business days to any province nationwide.", "icon": "📦", "priceLabel": "₱60 and up"  },
  { "id": 3, "name": "Cash on Delivery",  "description": "Buyer pays only upon receiving the parcel.",    "icon": "💵", "priceLabel": "Free"         },
  { "id": 4, "name": "Bulky Cargo",       "description": "Specialised handling for oversized/heavy items.","icon": "🏗️", "priceLabel": "Custom rate" },
  { "id": 5, "name": "Door-to-Door",      "description": "Picked up and delivered to your exact address.", "icon": "🚪", "priceLabel": "₱79 and up"  }
]
```

### `Features/Services/ServiceDto.cs`

```csharp
namespace JTExpress.Api.Features.Services;

public sealed record ServiceDto(
    int    Id,
    string Name,
    string Description,
    string Icon,
    string PriceLabel);
```

### `Features/Services/IServicesRepository.cs`

```csharp
namespace JTExpress.Api.Features.Services;

public interface IServicesRepository
{
    Task<IReadOnlyList<ServiceDto>> GetAllAsync();
}
```

### `Features/Services/ServicesRepository.cs`

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

### `Features/Services/IServicesService.cs`

```csharp
namespace JTExpress.Api.Features.Services;

public interface IServicesService
{
    Task<IReadOnlyList<ServiceDto>> GetAllServicesAsync();
}
```

### `Features/Services/ServicesService.cs`

```csharp
namespace JTExpress.Api.Features.Services;

public sealed class ServicesService(IServicesRepository repository) : IServicesService
{
    public Task<IReadOnlyList<ServiceDto>> GetAllServicesAsync() =>
        repository.GetAllAsync();
}
```

### `Features/Services/ServicesController.cs`

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

---

## Feature: Rates

### `Data/rates.json`

```json
[
  { "zone": "Metro Manila", "firstKg": 89,  "succeedingKg": 20 },
  { "zone": "Luzon",        "firstKg": 99,  "succeedingKg": 30 },
  { "zone": "Visayas",      "firstKg": 119, "succeedingKg": 40 },
  { "zone": "Mindanao",     "firstKg": 129, "succeedingKg": 45 }
]
```

### `Features/Rates/RateDto.cs`

```csharp
namespace JTExpress.Api.Features.Rates;

public sealed record RateDto(string Zone, decimal FirstKg, decimal SucceedingKg);

public sealed record RateCalculationResultDto(
    string  Zone,
    double  Weight,
    decimal EstimatedRate,
    string  FormattedRate);
```

### `Features/Rates/IRatesRepository.cs`

```csharp
namespace JTExpress.Api.Features.Rates;

public interface IRatesRepository
{
    Task<IReadOnlyList<RateDto>> GetAllAsync();
    Task<RateDto?> GetByZoneAsync(string zone);
}
```

### `Features/Rates/RatesRepository.cs`

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

### `Features/Rates/IRatesService.cs`

```csharp
namespace JTExpress.Api.Features.Rates;

public interface IRatesService
{
    Task<IReadOnlyList<RateDto>> GetAllRatesAsync();
    Task<RateCalculationResultDto?> CalculateAsync(string zone, double weight);
}
```

### `Features/Rates/RatesService.cs`

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

### `Features/Rates/RatesController.cs`

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

---

## Feature: Tracking

### `Data/tracking.json`

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

### `Features/Tracking/TrackingDto.cs`

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

### `Features/Tracking/ITrackingRepository.cs`

```csharp
namespace JTExpress.Api.Features.Tracking;

public interface ITrackingRepository
{
    Task<TrackingResultDto?> GetByTrackingNumberAsync(string trackingNumber);
}
```

### `Features/Tracking/TrackingRepository.cs`

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

        _cache = list.ToDictionary(t => t.TrackingNumber.ToUpperInvariant());
        logger.LogInformation("Loaded {Count} tracking records from {Path}", _cache.Count, filePath);
    }

    public Task<TrackingResultDto?> GetByTrackingNumberAsync(string trackingNumber) =>
        Task.FromResult(_cache.GetValueOrDefault(trackingNumber.ToUpperInvariant()));
}
```

### `Features/Tracking/ITrackingService.cs` / `TrackingService.cs` / `TrackingController.cs`

```csharp
// ITrackingService.cs
namespace JTExpress.Api.Features.Tracking;

public interface ITrackingService
{
    Task<TrackingResultDto?> TrackAsync(string trackingNumber);
}

// TrackingService.cs
public sealed class TrackingService(ITrackingRepository repository) : ITrackingService
{
    public Task<TrackingResultDto?> TrackAsync(string trackingNumber) =>
        repository.GetByTrackingNumberAsync(trackingNumber);
}

// TrackingController.cs
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
            return NotFound(ApiResponse<object>.Fail($"No parcel found for tracking number '{trackingNumber}'."));

        return Ok(ApiResponse<TrackingResultDto>.Ok(result));
    }
}
```

---

## Feature: Branches

Full implementation follows the same Dto → Interface → Repository → Service → Controller pattern as Services. See the Dev Workflow Guide for the exact build order per slice.

---

## FRONTEND — React TypeScript (Vite)

### Setup

```bash
npm create vite@latest jt-express-web -- --template react-ts
cd jt-express-web
npm install react-router-dom axios
```

> No Tailwind. Styles use CSS Modules + CSS custom properties. See the UI Guide.

### `.env` (development)

```
VITE_API_BASE_URL=http://localhost:5000/api
```

### `.env.production`

```
VITE_API_BASE_URL=https://your-production-api.com/api
```

---

### `src/config/env.ts`

```ts
const env = {
  apiBaseUrl: import.meta.env.VITE_API_BASE_URL as string,
} as const;

if (!env.apiBaseUrl) {
  throw new Error("VITE_API_BASE_URL is not defined. Check your .env file.");
}

export default env;
```

---

### `src/api/client.ts`

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

---

### `src/types/index.ts`

```ts
export interface ApiResponse<T> {
  success: boolean;
  data:    T | null;
  error:   string | null;
}

export interface Service {
  id:          number;
  name:        string;
  description: string;
  icon:        string;
  priceLabel:  string;
}

export interface Rate {
  zone:         string;
  firstKg:      number;
  succeedingKg: number;
}

export interface RateCalculationResult {
  zone:          string;
  weight:        number;
  estimatedRate: number;
  formattedRate: string;
}

export interface TrackingEvent {
  date:     string;
  status:   string;
  location: string;
}

export interface TrackingResult {
  trackingNumber:    string;
  status:            string;
  sender:            string;
  recipient:         string;
  estimatedDelivery: string;
  timeline:          TrackingEvent[];
}

export interface Branch {
  id:        number;
  name:      string;
  address:   string;
  region:    string;
  phone:     string;
  hours:     string;
  latitude:  number;
  longitude: number;
}
```

---

## Production Best Practices Checklist

### .NET Core API
- [x] No hardcoded data — all seed data in `Data/*.json` files
- [x] No hardcoded config — CORS origins, file paths all from `appsettings.json`
- [x] Thin controllers — zero business logic, only call service and return response
- [x] Interface-first — every service and repository has an interface (testable, swappable)
- [x] Singleton repositories — JSON loaded once at startup, cached in memory
- [x] Global exception middleware — no raw errors ever reach the client
- [x] Consistent response shape — `ApiResponse<T>` on every endpoint
- [x] Proper HTTP status codes — `200`, `400`, `404` used correctly
- [x] Dependency injection — no `new` in controllers or services
- [x] `ProducesResponseType` — Swagger documents all possible responses
- [x] Logging — `ILogger<T>` in repositories; use `LogInformation`, `LogError`

### React TypeScript
- [x] No hardcoded URLs — `VITE_API_BASE_URL` read via `src/config/env.ts`
- [x] No `import.meta.env` scattered in components — one config file only
- [x] No fetch logic in components — all API calls live in custom hooks
- [x] No `any` types — strict TypeScript throughout
- [x] API layer is isolated — `src/api/` only; swap Axios for fetch without touching pages
- [x] Axios interceptor unwraps `ApiResponse<T>` — pages never see the wrapper
- [x] All async states handled — every hook exposes `loading`, `error`, `data`
- [x] Shared `LoadingSpinner` + `ErrorMessage` — consistent UX
- [x] CSS Modules — no class name collisions, scoped per component
- [x] `.env` and `.env.production` kept separate — never commit secrets

---

## Running the Project

```bash
# Terminal 1 — Backend
cd JTExpress.Api
dotnet run
# → http://localhost:5000
# → Swagger: http://localhost:5000/swagger

# Terminal 2 — Frontend
cd jt-express-web
npm run dev
# → http://localhost:5173
```

---

> **Swapping to a real DB later:** Replace each `*Repository.cs` implementation with an EF Core version. Everything else — controllers, services, interfaces, frontend — stays exactly the same.
