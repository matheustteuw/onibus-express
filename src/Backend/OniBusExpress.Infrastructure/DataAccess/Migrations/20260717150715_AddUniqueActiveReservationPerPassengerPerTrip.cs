using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OniBusExpress.Infrastructure.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueActiveReservationPerPassengerPerTrip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Reservations_TripId_PassengerId",
                table: "Reservations",
                columns: new[] { "TripId", "PassengerId" },
                unique: true,
                filter: "[Status] = 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reservations_TripId_PassengerId",
                table: "Reservations");
        }
    }
}
