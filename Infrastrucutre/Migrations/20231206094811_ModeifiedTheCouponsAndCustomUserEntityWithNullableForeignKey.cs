using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastrucutre.Migrations
{
    public partial class ModeifiedTheCouponsAndCustomUserEntityWithNullableForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coupons_AspNetUsers_PatientId",
                table: "Coupons");

            migrationBuilder.DropIndex(
                name: "IX_Coupons_PatientId",
                table: "Coupons");

            migrationBuilder.AlterColumn<string>(
                name: "PatientId",
                table: "Coupons",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_Coupons_PatientId",
                table: "Coupons",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Coupons_AspNetUsers_PatientId",
                table: "Coupons",
                column: "PatientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coupons_AspNetUsers_PatientId",
                table: "Coupons");

            migrationBuilder.DropIndex(
                name: "IX_Coupons_PatientId",
                table: "Coupons");

            migrationBuilder.AlterColumn<string>(
                name: "PatientId",
                table: "Coupons",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Coupons_PatientId",
                table: "Coupons",
                column: "PatientId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Coupons_AspNetUsers_PatientId",
                table: "Coupons",
                column: "PatientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
