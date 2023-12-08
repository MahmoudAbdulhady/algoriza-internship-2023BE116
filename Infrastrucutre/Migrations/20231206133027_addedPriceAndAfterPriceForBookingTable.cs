using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastrucutre.Migrations
{
    public partial class addedPriceAndAfterPriceForBookingTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PriceAfterCoupon",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "PriceAfterCoupon",
                table: "Bookings");
        }
    }
}
