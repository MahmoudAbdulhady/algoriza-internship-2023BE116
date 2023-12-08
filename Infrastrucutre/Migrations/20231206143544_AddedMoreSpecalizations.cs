using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastrucutre.Migrations
{
    public partial class AddedMoreSpecalizations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2351633c-3caf-4c45-a979-8873f02187d6");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "AccountRole", "ConcurrencyStamp", "DateOfBirth", "Email", "EmailConfirmed", "FirstName", "FullName", "Gender", "ImageUrl", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "2192e432-3c0b-48e3-8ade-87ff2404e220", 0, 0, "d9e0b924-d545-4e43-a2bd-af2a6ef5c4d6", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified).AddTicks(9), "VeeztaAdmin@gmail.com", false, "Veezta", "VezetaAdmin", 1, "Admin", "Admin", false, null, null, null, null, null, false, "b8dd4de6-4eb7-460a-b69b-f49db0589a85", false, null });

            migrationBuilder.InsertData(
                table: "Specializations",
                columns: new[] { "SpecializationId", "SpecializationName" },
                values: new object[,]
                {
                    { 11, "Urology" },
                    { 12, "Ophthalmology" },
                    { 13, "Surgery" },
                    { 14, "Endocrinologist" },
                    { 15, "Gastroenterology" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2192e432-3c0b-48e3-8ade-87ff2404e220");

            migrationBuilder.DeleteData(
                table: "Specializations",
                keyColumn: "SpecializationId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Specializations",
                keyColumn: "SpecializationId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Specializations",
                keyColumn: "SpecializationId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Specializations",
                keyColumn: "SpecializationId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Specializations",
                keyColumn: "SpecializationId",
                keyValue: 15);

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "AccountRole", "ConcurrencyStamp", "DateOfBirth", "Email", "EmailConfirmed", "FirstName", "FullName", "Gender", "ImageUrl", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "2351633c-3caf-4c45-a979-8873f02187d6", 0, 0, "23d7b11f-6ada-4079-a3c5-4b0224ca8b05", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified).AddTicks(9), "VeeztaAdmin@gmail.com", false, "Veezta", "VezetaAdmin", 1, "Admin", "Admin", false, null, null, null, null, null, false, "e82c364e-bc5e-4798-8eb5-4e6b4030a448", false, null });
        }
    }
}
