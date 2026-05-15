namespace JTExpress.Api.Features.Branches;

public sealed record BranchDto(
    int Id,
    string Name,
    string Address,
    string Region,
    string Phone,
    string Hours,
    double Latitude,
    double Longitude);

public sealed record CreateBranchDto(
    string Name,
    string Address,
    string Region,
    string Phone,
    string Hours,
    double Latitude,
    double Longitude);

public sealed record UpdateBranchDto(
    string Name,
    string Address,
    string Region,
    string Phone,
    string Hours,
    double Latitude,
    double Longitude);
