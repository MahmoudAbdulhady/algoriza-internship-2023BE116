using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastrucutre.Migrations
{
    public partial class addedTwoNewLookupTablesDayOftheWeekAndDayTimesTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Appointments_AppointmentId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_AppointmentId",
                table: "Bookings");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "54f9243c-6bd8-4204-b5c0-565e747efb58");

            migrationBuilder.DropColumn(
                name: "AppointmentId",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "DaysOfTheWeek",
                table: "Appointments",
                newName: "DayOfTheWeek");

            migrationBuilder.AlterColumn<byte>(
                name: "Status",
                table: "Bookings",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "AppointementAppointmentId",
                table: "Bookings",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DaysOfTheWeeks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DaysOfTheWeeks", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "AccountRole", "ConcurrencyStamp", "DateOfBirth", "Email", "EmailConfirmed", "FirstName", "FullName", "Gender", "ImageUrl", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "2344fc4b-56a5-44b9-97f0-bb54a2ff36ce", 0, 0, "4f9bd2bc-5d96-424d-8f80-feac8b48034a", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified).AddTicks(9), "VeeztaAdmin@gmail.com", false, "Veezta", "VezetaAdmin", 1, "Admin", "Admin", false, null, null, null, null, null, false, "f32af9d6-a3a3-4764-9d81-c8495f130c6e", false, null });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_AppointementAppointmentId",
                table: "Bookings",
                column: "AppointementAppointmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Appointments_AppointementAppointmentId",
                table: "Bookings",
                column: "AppointementAppointmentId",
                principalTable: "Appointments",
                principalColumn: "AppointmentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Appointments_AppointementAppointmentId",
                table: "Bookings");

            migrationBuilder.DropTable(
                name: "DaysOfTheWeeks");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_AppointementAppointmentId",
                table: "Bookings");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2344fc4b-56a5-44b9-97f0-bb54a2ff36ce");

            migrationBuilder.DropColumn(
                name: "AppointementAppointmentId",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "DayOfTheWeek",
                table: "Appointments",
                newName: "DaysOfTheWeek");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Bookings",
                type: "int",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AddColumn<int>(
                name: "AppointmentId",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "AccountRole", "ConcurrencyStamp", "DateOfBirth", "Email", "EmailConfirmed", "FirstName", "FullName", "Gender", "ImageUrl", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "54f9243c-6bd8-4204-b5c0-565e747efb58", 0, 0, "854d48fa-bc9e-4915-89a2-0c7bfc99eedd", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified).AddTicks(9), "VeeztaAdmin@gmail.com", false, "Veezta", "VezetaAdmin", 1, "Admin", "Admin", false, null, null, null, null, null, false, "9073e335-3398-49b7-a394-cc3754e7effb", false, null });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_AppointmentId",
                table: "Bookings",
                column: "AppointmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Appointments_AppointmentId",
                table: "Bookings",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "AppointmentId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
