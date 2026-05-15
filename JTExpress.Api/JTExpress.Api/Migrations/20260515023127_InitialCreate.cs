using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JTExpress.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Branches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    Region = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: false),
                    Hours = table.Column<string>(type: "text", nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Zone = table.Column<string>(type: "text", nullable: false),
                    FirstKg = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    SucceedingKg = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Icon = table.Column<string>(type: "text", nullable: false),
                    PriceLabel = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrackingResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TrackingNumber = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    Sender = table.Column<string>(type: "text", nullable: false),
                    Recipient = table.Column<string>(type: "text", nullable: false),
                    EstimatedDelivery = table.Column<string>(type: "text", nullable: false),
                    CurrentLocation = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackingResults", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrackingEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TrackingResultEntityId = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    Location = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackingEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrackingEvents_TrackingResults_TrackingResultEntityId",
                        column: x => x.TrackingResultEntityId,
                        principalTable: "TrackingResults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Branches",
                columns: new[] { "Id", "Address", "Hours", "Latitude", "Longitude", "Name", "Phone", "Region" },
                values: new object[,]
                {
                    { 1, "1234 Taft Ave, Ermita, Manila", "Mon-Sat 8AM-6PM", 14.5764, 120.989, "J&T Manila Main", "(02) 8123-4567", "Metro Manila" },
                    { 2, "456 EDSA, Quezon City", "Mon-Sat 8AM-6PM", 14.676, 121.0437, "J&T Quezon City", "(02) 8234-5678", "Metro Manila" },
                    { 3, "789 Colon St, Cebu City", "Mon-Sat 8AM-6PM", 10.293100000000001, 123.8995, "J&T Cebu Main", "(032) 123-4567", "Cebu" },
                    { 4, "321 Rizal St, Davao City", "Mon-Sat 8AM-6PM", 7.0644, 125.60769999999999, "J&T Davao", "(082) 234-5678", "Davao" },
                    { 5, "SM Clark, Angeles City, Pampanga", "Mon-Sat 8AM-6PM", 15.135, 120.596, "J&T Pampanga", "(045) 345-6789", "Pampanga" },
                    { 6, "SM City Iloilo, Mandurriao, Iloilo City", "Mon-Sat 8AM-6PM", 10.7202, 122.5621, "J&T Iloilo", "(033) 456-7890", "Iloilo" }
                });

            migrationBuilder.InsertData(
                table: "Rates",
                columns: new[] { "Id", "FirstKg", "SucceedingKg", "Zone" },
                values: new object[,]
                {
                    { 1, 89m, 19m, "Metro Manila" },
                    { 2, 120m, 29m, "Luzon" },
                    { 3, 150m, 39m, "Visayas" },
                    { 4, 150m, 39m, "Mindanao" },
                    { 5, 180m, 49m, "Island Provinces" }
                });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "Description", "Icon", "Name", "PriceLabel" },
                values: new object[,]
                {
                    { 1, "Next-day delivery within Metro Manila.", "Express", "Express Delivery", "PHP 89+" },
                    { 2, "2-5 business days to any province nationwide.", "Parcel", "Standard Delivery", "PHP 60+" },
                    { 3, "Buyer pays only upon receiving the parcel.", "COD", "Cash on Delivery", "Free" },
                    { 4, "Specialized handling for oversized and heavy items.", "Cargo", "Bulky Cargo", "Custom" },
                    { 5, "Picked up and delivered to your exact address.", "Door", "Door-to-Door", "PHP 79+" }
                });

            migrationBuilder.InsertData(
                table: "TrackingResults",
                columns: new[] { "Id", "CurrentLocation", "EstimatedDelivery", "Recipient", "Sender", "Status", "TrackingNumber" },
                values: new object[,]
                {
                    { 1, "Makati", "Delivered", "Juan Dela Cruz", "Manila Warehouse", "Delivered", "JT123456789PH" },
                    { 2, "Cebu Hub", "Tomorrow", "Maria Santos", "Cebu Seller", "In Transit", "JT987654321PH" },
                    { 3, "Davao", "Today", "Carlo Reyes", "Davao Warehouse", "Out for Delivery", "JT555000111PH" }
                });

            migrationBuilder.InsertData(
                table: "TrackingEvents",
                columns: new[] { "Id", "Date", "Location", "Status", "TrackingResultEntityId" },
                values: new object[,]
                {
                    { 1, "May 12 09:00 AM", "Quezon City", "Parcel Picked Up", 1 },
                    { 2, "May 12 02:00 PM", "Manila Warehouse", "In Transit", 1 },
                    { 3, "May 13 09:00 AM", "Makati Hub", "Out for Delivery", 1 },
                    { 4, "May 13 03:00 PM", "Makati", "Delivered", 1 },
                    { 5, "May 13 08:00 AM", "Cebu City", "Parcel Picked Up", 2 },
                    { 6, "May 13 05:00 PM", "Cebu Hub", "Arrived at Hub", 2 },
                    { 7, "May 14 09:00 AM", "Cebu Hub", "In Transit", 2 },
                    { 8, "May 14 10:00 AM", "Davao Warehouse", "Parcel Picked Up", 3 },
                    { 9, "May 14 07:00 PM", "Davao Hub", "Arrived at Hub", 3 },
                    { 10, "May 15 07:30 AM", "Davao", "Out for Delivery", 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrackingEvents_TrackingResultEntityId",
                table: "TrackingEvents",
                column: "TrackingResultEntityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Branches");

            migrationBuilder.DropTable(
                name: "Rates");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "TrackingEvents");

            migrationBuilder.DropTable(
                name: "TrackingResults");
        }
    }
}
