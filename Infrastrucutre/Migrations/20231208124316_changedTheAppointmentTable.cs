using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastrucutre.Migrations
{
    public partial class changedTheAppointmentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Appointments_AppointementAppointmentId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Times_Appointments_AppointmentId",
                table: "Times");

            migrationBuilder.DropIndex(
                name: "IX_Times_AppointmentId",
                table: "Times");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_AppointementAppointmentId",
                table: "Bookings");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "80b67ba8-aea7-4ba1-87d5-f9c5e0acc58e");

            migrationBuilder.DropColumn(
                name: "AppointementAppointmentId",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "DayOfTheWeek",
                table: "Appointments",
                newName: "DayTimeId");

            migrationBuilder.AddColumn<int>(
                name: "AppointementAppointmentId",
                table: "Times",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DayOfWeekId",
                table: "Appointments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "AccountRole", "ConcurrencyStamp", "DateOfBirth", "Email", "EmailConfirmed", "FirstName", "FullName", "Gender", "ImageUrl", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "a72f3d63-2852-46f1-a581-271281e7c8b6", 0, 0, "8164d310-bed6-4ce3-b5b3-26e5e556e609", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified).AddTicks(9), "VeeztaAdmin@gmail.com", false, "Veezta", "VezetaAdmin", 1, "Admin", "Admin", false, null, null, null, null, null, false, "e09369d3-6e47-4f8b-9608-e5eca3f67baa", false, null });

            migrationBuilder.CreateIndex(
                name: "IX_Times_AppointementAppointmentId",
                table: "Times",
                column: "AppointementAppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_DayOfWeekId",
                table: "Appointments",
                column: "DayOfWeekId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_DayTimeId",
                table: "Appointments",
                column: "DayTimeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_DaysOfTheWeeks_DayOfWeekId",
                table: "Appointments",
                column: "DayOfWeekId",
                principalTable: "DaysOfTheWeeks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_DayTimes_DayTimeId",
                table: "Appointments",
                column: "DayTimeId",
                principalTable: "DayTimes",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_DaysOfTheWeeks_DayOfWeekId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_DayTimes_DayTimeId",
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

            migrationBuilder.DropIndex(
                name: "IX_Appointments_DayTimeId",
                table: "Appointments");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a72f3d63-2852-46f1-a581-271281e7c8b6");

            migrationBuilder.DropColumn(
                name: "AppointementAppointmentId",
                table: "Times");

            migrationBuilder.DropColumn(
                name: "DayOfWeekId",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "DayTimeId",
                table: "Appointments",
                newName: "DayOfTheWeek");

            migrationBuilder.AddColumn<int>(
                name: "AppointementAppointmentId",
                table: "Bookings",
                type: "int",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "AccountRole", "ConcurrencyStamp", "DateOfBirth", "Email", "EmailConfirmed", "FirstName", "FullName", "Gender", "ImageUrl", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "80b67ba8-aea7-4ba1-87d5-f9c5e0acc58e", 0, 0, "d25c3f26-ec7e-440c-8072-aba3e9a59ea6", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified).AddTicks(9), "VeeztaAdmin@gmail.com", false, "Veezta", "VezetaAdmin", 1, "Admin", "Admin", false, null, null, null, null, null, false, "dda8380a-c42f-423f-862f-451db734730f", false, null });

            migrationBuilder.CreateIndex(
                name: "IX_Times_AppointmentId",
                table: "Times",
                column: "AppointmentId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Times_Appointments_AppointmentId",
                table: "Times",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "AppointmentId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
