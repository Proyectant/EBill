using Microsoft.EntityFrameworkCore.Migrations;

namespace ERacuni.Server.Migrations
{
    public partial class DTO : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "84c63455-2f11-4207-9d14-2ac7bb65c7b1");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6e6607f5-a90a-4200-ad03-41813ba1a9aa");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName", "firstName", "lastName" },
                values: new object[] { "aad2f2d6-4530-4733-87ca-09060cdbe79d", 0, "cff64499-a604-4865-b4fb-f8b233c4ae83", "User", null, false, false, null, null, null, null, null, false, "f8d97a5b-3555-4c9b-86e6-f759146d1c8c", false, "proyectant", "Almedin", "Ljajic" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "aad2f2d6-4530-4733-87ca-09060cdbe79d");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName", "firstName", "lastName" },
                values: new object[] { "84c63455-2f11-4207-9d14-2ac7bb65c7b1", 0, "898881a9-08c8-4345-af6d-be0ba36609dd", "User", null, false, false, null, null, null, null, null, false, "138e1e08-cb8a-4c17-8b7d-29a4d2970a8e", false, "proyectant", "Almedin", "Ljajic" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "6e6607f5-a90a-4200-ad03-41813ba1a9aa", 0, "bilosta", "IdentityUser", null, false, false, null, null, null, "admin", null, false, "bilosta", false, "admin" });
        }
    }
}
