using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastrucutre.Migrations
{
    public partial class ModifiyingtheCouponEntityWithCouponName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CouponName",
                table: "Coupons",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CouponName",
                table: "Coupons");
        }
    }
}
