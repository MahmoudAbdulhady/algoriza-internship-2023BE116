using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastrucutre.Migrations
{
    public partial class ChangedAppointmentTableAndTimesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_DaysOfTheWeeks_DayOfWeekId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Times_Appointments_AppointementAppointmentId",
                table: "Times");

            migrationBuilder.DropIndex(
                name: "IX_Times_AppointementAppointmentId",
                table: "Times");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_DayOfWeekId",
                table: "Appointments");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8dcfcd82-2b5b-4194-b4f1-f64bf7473653");

            migrationBuilder.DropColumn(
                name: "AppointementAppointmentId",
                table: "Times");

            migrationBuilder.DropColumn(
                name: "Time",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "DayOfWeekId",
                table: "Appointments",
                newName: "Days");

            migrationBuilder.AlterColumn<string>(
                name: "StartTime",
                table: "Times",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "EndTime",
                table: "Times",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "AccountRole", "ConcurrencyStamp", "DateOfBirth", "Email", "EmailConfirmed", "FirstName", "FullName", "Gender", "ImageUrl", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "6fd5d0d2-8610-49d4-b64f-fe5d2a84e211", 0, 0, "969ebe22-4012-436c-ab39-db5d823e0372", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified).AddTicks(9), "VeeztaAdmin@gmail.com", false, "Veezta", "VezetaAdmin", 1, "Admin", "Admin", false, null, null, null, null, null, false, "7b230cc0-ca03-4f62-a212-64669bd45a43", false, null });

            migrationBuilder.CreateIndex(
                name: "IX_Times_AppointmentId",
                table: "Times",
                column: "AppointmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Times_Appointments_AppointmentId",
                table: "Times",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "AppointmentId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Times_Appointments_AppointmentId",
                table: "Times");

            migrationBuilder.DropIndex(
                name: "IX_Times_AppointmentId",
                table: "Times");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6fd5d0d2-8610-49d4-b64f-fe5d2a84e211");

            migrationBuilder.RenameColumn(
                name: "Days",
                table: "Appointments",
                newName: "DayOfWeekId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartTime",
                table: "Times",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndTime",
                table: "Times",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "AppointementAppointmentId",
                table: "Times",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Time",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "AccountRole", "ConcurrencyStamp", "DateOfBirth", "Email", "EmailConfirmed", "FirstName", "FullName", "Gender", "ImageUrl", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "8dcfcd82-2b5b-4194-b4f1-f64bf7473653", 0, 0, "33d89f54-8db9-4d1d-aece-b167c5048a1d", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified).AddTicks(9), "VeeztaAdmin@gmail.com", false, "Veezta", "VezetaAdmin", 1, "Admin", "Admin", false, null, null, null, null, null, false, "f7649ca5-c6bc-47b0-8d2b-56f8acbb08c9", false, null });

            migrationBuilder.CreateIndex(
                name: "IX_Times_AppointementAppointmentId",
                table: "Times",
                column: "AppointementAppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_DayOfWeekId",
                table: "Appointments",
                column: "DayOfWeekId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_DaysOfTheWeeks_DayOfWeekId",
                table: "Appointments",
                column: "DayOfWeekId",
                principalTable: "DaysOfTheWeeks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Times_Appointments_AppointementAppointmentId",
                table: "Times",
                column: "AppointementAppointmentId",
                principalTable: "Appointments",
                principalColumn: "AppointmentId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
