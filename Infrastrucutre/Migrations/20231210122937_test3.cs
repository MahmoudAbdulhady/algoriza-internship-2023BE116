using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastrucutre.Migrations
{
    public partial class test3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "dde6dad3-5d30-4f27-87f1-ec5fb8a441df");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "AccountRole", "ConcurrencyStamp", "DateOfBirth", "Email", "EmailConfirmed", "FirstName", "FullName", "Gender", "ImageUrl", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "66e0420e-027d-4e13-81a0-9139094b12ea", 0, 0, "84c1dea4-a912-4049-a85c-66ee878ab3e9", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified).AddTicks(9), "VeeztaAdmin@gmail.com", false, "Veezta", "VezetaAdmin", 1, "Admin", "Admin", false, null, null, null, null, null, false, "fd809feb-4d2a-4912-ac65-9ccbee32f0b4", false, null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "66e0420e-027d-4e13-81a0-9139094b12ea");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "AccountRole", "ConcurrencyStamp", "DateOfBirth", "Email", "EmailConfirmed", "FirstName", "FullName", "Gender", "ImageUrl", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "dde6dad3-5d30-4f27-87f1-ec5fb8a441df", 0, 0, "3ad027c3-eeaf-48d5-851e-226b3a415861", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified).AddTicks(9), "VeeztaAdmin@gmail.com", false, "Veezta", "VezetaAdmin", 1, "Admin", "Admin", false, null, null, null, null, null, false, "8f485a39-0a2e-4492-b3c7-f0c325b17281", false, null });
        }
    }
}
