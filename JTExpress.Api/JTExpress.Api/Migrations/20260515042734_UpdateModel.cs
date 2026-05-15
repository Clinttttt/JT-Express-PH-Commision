using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JTExpress.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "Icon" },
                values: new object[] { "Next-day delivery within Metro Manila. Perfect for urgent shipments and time-sensitive packages.", "⚡" });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "Icon" },
                values: new object[] { "2-5 business days to any province nationwide. Reliable and cost-effective for regular shipments.", "📦" });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "Icon" },
                values: new object[] { "Buyer pays only upon receiving the parcel. Secure payment option for online sellers.", "💵" });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Description", "Icon" },
                values: new object[] { "Specialized handling for oversized and heavy items. Professional care for valuable goods.", "🏗️" });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Description", "Icon" },
                values: new object[] { "Picked up and delivered to your exact address. Convenient service for busy customers.", "🚪" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "Icon" },
                values: new object[] { "Next-day delivery within Metro Manila.", "Express" });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "Icon" },
                values: new object[] { "2-5 business days to any province nationwide.", "Parcel" });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "Icon" },
                values: new object[] { "Buyer pays only upon receiving the parcel.", "COD" });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Description", "Icon" },
                values: new object[] { "Specialized handling for oversized and heavy items.", "Cargo" });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Description", "Icon" },
                values: new object[] { "Picked up and delivered to your exact address.", "Door" });
        }
    }
}
