using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastrucutre.Migrations
{
    public partial class AddedCouponFlagForBookingTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCouponUsed",
                table: "Bookings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCouponUsed",
                table: "Bookings");
        }
    }
}
