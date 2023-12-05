using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastrucutre.Migrations
{
    public partial class ChanedTheDeleteRelationBetweenTimeTableAndBookingTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Times_TimeId",
                table: "Bookings");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Times_TimeId",
                table: "Bookings",
                column: "TimeId",
                principalTable: "Times",
                principalColumn: "TimeId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Times_TimeId",
                table: "Bookings");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Times_TimeId",
                table: "Bookings",
                column: "TimeId",
                principalTable: "Times",
                principalColumn: "TimeId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
