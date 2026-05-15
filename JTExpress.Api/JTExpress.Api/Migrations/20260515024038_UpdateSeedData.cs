using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JTExpress.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TrackingEvents",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Address", "Hours", "Latitude", "Longitude", "Name", "Phone" },
                values: new object[] { "123 Ayala Ave, Makati City", "Mon–Sat 8AM–6PM", 14.5547, 121.0244, "J&T Express — Makati", "(02) 8123-4001" });

            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Address", "Hours", "Name", "Phone" },
                values: new object[] { "45 Quezon Ave, QC", "Mon–Sat 8AM–6PM", "J&T Express — Quezon City", "(02) 8123-4002" });

            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Address", "Hours", "Latitude", "Longitude", "Name", "Phone", "Region" },
                values: new object[] { "78 Colon St, Cebu City", "Mon–Sat 8AM–6PM", 10.296900000000001, 123.9016, "J&T Express — Cebu", "(032) 412-3001", "Visayas" });

            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Address", "Hours", "Latitude", "Longitude", "Name", "Phone", "Region" },
                values: new object[] { "22 Ilustre St, Davao City", "Mon–Sat 8AM–6PM", 7.0731000000000002, 125.61279999999999, "J&T Express — Davao", "(082) 227-3001", "Mindanao" });

            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Address", "Hours", "Latitude", "Longitude", "Name", "Phone", "Region" },
                values: new object[] { "100 MacArthur Hwy, San Fernando", "Mon–Sat 8AM–6PM", 15.0794, 120.62, "J&T Express — Pampanga", "(045) 961-3001", "Luzon" });

            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Address", "Hours", "Latitude", "Longitude", "Name", "Phone", "Region" },
                values: new object[] { "55 Iznart St, Iloilo City", "Mon–Sat 8AM–6PM", 10.696899999999999, 122.56399999999999, "J&T Express — Iloilo", "(033) 335-3001", "Visayas" });

            migrationBuilder.UpdateData(
                table: "TrackingEvents",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Date", "Location", "Status" },
                values: new object[] { "May 13 03:00 PM", "Makati City", "Delivered" });

            migrationBuilder.UpdateData(
                table: "TrackingEvents",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Date", "Location", "Status" },
                values: new object[] { "May 13 09:00 AM", "Makati Hub", "Out for Delivery" });

            migrationBuilder.UpdateData(
                table: "TrackingEvents",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Date", "Status" },
                values: new object[] { "May 12 11:00 PM", "Arrived at Hub" });

            migrationBuilder.UpdateData(
                table: "TrackingEvents",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Date", "Location", "Status" },
                values: new object[] { "May 12 06:00 AM", "Quezon City", "Parcel Picked Up" });

            migrationBuilder.UpdateData(
                table: "TrackingEvents",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Date", "Location", "Status" },
                values: new object[] { "May 14 08:00 AM", "Manila Sorting Center", "In Transit" });

            migrationBuilder.UpdateData(
                table: "TrackingEvents",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Date", "Location", "Status" },
                values: new object[] { "May 13 04:00 PM", "Cebu City", "Parcel Picked Up" });

            migrationBuilder.UpdateData(
                table: "TrackingEvents",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Date", "Location", "Status", "TrackingResultEntityId" },
                values: new object[] { "May 15 07:00 AM", "Davao Hub", "Out for Delivery", 3 });

            migrationBuilder.UpdateData(
                table: "TrackingEvents",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Date", "Location", "Status" },
                values: new object[] { "May 14 10:00 PM", "Davao Hub", "Arrived at Hub" });

            migrationBuilder.UpdateData(
                table: "TrackingEvents",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Date", "Location", "Status" },
                values: new object[] { "May 14 02:00 PM", "Manila Hub", "In Transit" });

            migrationBuilder.UpdateData(
                table: "TrackingResults",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CurrentLocation", "EstimatedDelivery", "Recipient", "Sender" },
                values: new object[] { "Makati City", "May 13, 2025", "Maria Santos", "Juan dela Cruz" });

            migrationBuilder.UpdateData(
                table: "TrackingResults",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CurrentLocation", "EstimatedDelivery", "Recipient", "Sender" },
                values: new object[] { "Manila Sorting Center", "May 16, 2025", "Ana Lim", "Pedro Reyes" });

            migrationBuilder.UpdateData(
                table: "TrackingResults",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CurrentLocation", "EstimatedDelivery", "Recipient", "Sender" },
                values: new object[] { "Davao Hub", "May 15, 2025", "Rose Villanueva", "Carlo Mendoza" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Address", "Hours", "Latitude", "Longitude", "Name", "Phone" },
                values: new object[] { "1234 Taft Ave, Ermita, Manila", "Mon-Sat 8AM-6PM", 14.5764, 120.989, "J&T Manila Main", "(02) 8123-4567" });

            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Address", "Hours", "Name", "Phone" },
                values: new object[] { "456 EDSA, Quezon City", "Mon-Sat 8AM-6PM", "J&T Quezon City", "(02) 8234-5678" });

            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Address", "Hours", "Latitude", "Longitude", "Name", "Phone", "Region" },
                values: new object[] { "789 Colon St, Cebu City", "Mon-Sat 8AM-6PM", 10.293100000000001, 123.8995, "J&T Cebu Main", "(032) 123-4567", "Cebu" });

            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Address", "Hours", "Latitude", "Longitude", "Name", "Phone", "Region" },
                values: new object[] { "321 Rizal St, Davao City", "Mon-Sat 8AM-6PM", 7.0644, 125.60769999999999, "J&T Davao", "(082) 234-5678", "Davao" });

            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Address", "Hours", "Latitude", "Longitude", "Name", "Phone", "Region" },
                values: new object[] { "SM Clark, Angeles City, Pampanga", "Mon-Sat 8AM-6PM", 15.135, 120.596, "J&T Pampanga", "(045) 345-6789", "Pampanga" });

            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Address", "Hours", "Latitude", "Longitude", "Name", "Phone", "Region" },
                values: new object[] { "SM City Iloilo, Mandurriao, Iloilo City", "Mon-Sat 8AM-6PM", 10.7202, 122.5621, "J&T Iloilo", "(033) 456-7890", "Iloilo" });

            migrationBuilder.UpdateData(
                table: "TrackingEvents",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Date", "Location", "Status" },
                values: new object[] { "May 12 09:00 AM", "Quezon City", "Parcel Picked Up" });

            migrationBuilder.UpdateData(
                table: "TrackingEvents",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Date", "Location", "Status" },
                values: new object[] { "May 12 02:00 PM", "Manila Warehouse", "In Transit" });

            migrationBuilder.UpdateData(
                table: "TrackingEvents",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Date", "Status" },
                values: new object[] { "May 13 09:00 AM", "Out for Delivery" });

            migrationBuilder.UpdateData(
                table: "TrackingEvents",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Date", "Location", "Status" },
                values: new object[] { "May 13 03:00 PM", "Makati", "Delivered" });

            migrationBuilder.UpdateData(
                table: "TrackingEvents",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Date", "Location", "Status" },
                values: new object[] { "May 13 08:00 AM", "Cebu City", "Parcel Picked Up" });

            migrationBuilder.UpdateData(
                table: "TrackingEvents",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Date", "Location", "Status" },
                values: new object[] { "May 13 05:00 PM", "Cebu Hub", "Arrived at Hub" });

            migrationBuilder.UpdateData(
                table: "TrackingEvents",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Date", "Location", "Status", "TrackingResultEntityId" },
                values: new object[] { "May 14 09:00 AM", "Cebu Hub", "In Transit", 2 });

            migrationBuilder.UpdateData(
                table: "TrackingEvents",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Date", "Location", "Status" },
                values: new object[] { "May 14 10:00 AM", "Davao Warehouse", "Parcel Picked Up" });

            migrationBuilder.UpdateData(
                table: "TrackingEvents",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Date", "Location", "Status" },
                values: new object[] { "May 14 07:00 PM", "Davao Hub", "Arrived at Hub" });

            migrationBuilder.InsertData(
                table: "TrackingEvents",
                columns: new[] { "Id", "Date", "Location", "Status", "TrackingResultEntityId" },
                values: new object[] { 10, "May 15 07:30 AM", "Davao", "Out for Delivery", 3 });

            migrationBuilder.UpdateData(
                table: "TrackingResults",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CurrentLocation", "EstimatedDelivery", "Recipient", "Sender" },
                values: new object[] { "Makati", "Delivered", "Juan Dela Cruz", "Manila Warehouse" });

            migrationBuilder.UpdateData(
                table: "TrackingResults",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CurrentLocation", "EstimatedDelivery", "Recipient", "Sender" },
                values: new object[] { "Cebu Hub", "Tomorrow", "Maria Santos", "Cebu Seller" });

            migrationBuilder.UpdateData(
                table: "TrackingResults",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CurrentLocation", "EstimatedDelivery", "Recipient", "Sender" },
                values: new object[] { "Davao", "Today", "Carlo Reyes", "Davao Warehouse" });
        }
    }
}
