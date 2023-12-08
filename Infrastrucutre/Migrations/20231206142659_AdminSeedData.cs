using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastrucutre.Migrations
{
    public partial class AdminSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "AccountRole", "ConcurrencyStamp", "DateOfBirth", "Email", "EmailConfirmed", "FirstName", "FullName", "Gender", "ImageUrl", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "2351633c-3caf-4c45-a979-8873f02187d6", 0, 0, "23d7b11f-6ada-4079-a3c5-4b0224ca8b05", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified).AddTicks(9), "VeeztaAdmin@gmail.com", false, "Veezta", "VezetaAdmin", 1, "Admin", "Admin", false, null, null, null, null, null, false, "e82c364e-bc5e-4798-8eb5-4e6b4030a448", false, null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2351633c-3caf-4c45-a979-8873f02187d6");
        }
    }
}
