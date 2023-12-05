using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastrucutre.Migrations
{
    public partial class RemovedAppointementTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_AspNetUsers_CustomUserId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Doctors_DoctorId",
                table: "Appointments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Appointments",
                table: "Appointments");

            migrationBuilder.RenameTable(
                name: "Appointments",
                newName: "Appointement");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_DoctorId",
                table: "Appointement",
                newName: "IX_Appointement_DoctorId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_CustomUserId",
                table: "Appointement",
                newName: "IX_Appointement_CustomUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Appointement",
                table: "Appointement",
                column: "AppointmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointement_AspNetUsers_CustomUserId",
                table: "Appointement",
                column: "CustomUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointement_Doctors_DoctorId",
                table: "Appointement",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "DoctorId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointement_AspNetUsers_CustomUserId",
                table: "Appointement");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointement_Doctors_DoctorId",
                table: "Appointement");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Appointement",
                table: "Appointement");

            migrationBuilder.RenameTable(
                name: "Appointement",
                newName: "Appointments");

            migrationBuilder.RenameIndex(
                name: "IX_Appointement_DoctorId",
                table: "Appointments",
                newName: "IX_Appointments_DoctorId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointement_CustomUserId",
                table: "Appointments",
                newName: "IX_Appointments_CustomUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Appointments",
                table: "Appointments",
                column: "AppointmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_AspNetUsers_CustomUserId",
                table: "Appointments",
                column: "CustomUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Doctors_DoctorId",
                table: "Appointments",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "DoctorId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
