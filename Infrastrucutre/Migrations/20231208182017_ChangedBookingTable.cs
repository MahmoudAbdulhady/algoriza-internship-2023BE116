using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastrucutre.Migrations
{
    public partial class ChangedBookingTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Times_TimeId",
                table: "Bookings");

            migrationBuilder.DropTable(
                name: "DaysOfTheWeeks");

            migrationBuilder.DropTable(
                name: "DayTimes");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_TimeId",
                table: "Bookings");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2a36eea4-4315-4cf0-9490-48b205e63433");

            migrationBuilder.RenameColumn(
                name: "TimeId",
                table: "Bookings",
                newName: "AppointmentId");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "AccountRole", "ConcurrencyStamp", "DateOfBirth", "Email", "EmailConfirmed", "FirstName", "FullName", "Gender", "ImageUrl", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "dde6dad3-5d30-4f27-87f1-ec5fb8a441df", 0, 0, "3ad027c3-eeaf-48d5-851e-226b3a415861", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified).AddTicks(9), "VeeztaAdmin@gmail.com", false, "Veezta", "VezetaAdmin", 1, "Admin", "Admin", false, null, null, null, null, null, false, "8f485a39-0a2e-4492-b3c7-f0c325b17281", false, null });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_AppointmentId",
                table: "Bookings",
                column: "AppointmentId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Appointments_AppointmentId",
                table: "Bookings",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "AppointmentId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
                keyValue: "dde6dad3-5d30-4f27-87f1-ec5fb8a441df");

            migrationBuilder.RenameColumn(
                name: "AppointmentId",
                table: "Bookings",
                newName: "TimeId");

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

            migrationBuilder.CreateTable(
                name: "DayTimes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Time = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DayTimes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "AccountRole", "ConcurrencyStamp", "DateOfBirth", "Email", "EmailConfirmed", "FirstName", "FullName", "Gender", "ImageUrl", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "2a36eea4-4315-4cf0-9490-48b205e63433", 0, 0, "ad1d8da9-5a11-4097-99e9-b11e46a62186", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified).AddTicks(9), "VeeztaAdmin@gmail.com", false, "Veezta", "VezetaAdmin", 1, "Admin", "Admin", false, null, null, null, null, null, false, "d2e3f9b2-3c48-4c5f-b0b4-036add6fa640", false, null });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_TimeId",
                table: "Bookings",
                column: "TimeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Times_TimeId",
                table: "Bookings",
                column: "TimeId",
                principalTable: "Times",
                principalColumn: "TimeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
