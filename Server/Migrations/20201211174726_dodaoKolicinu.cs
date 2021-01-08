using Microsoft.EntityFrameworkCore.Migrations;

namespace ERacuni.Server.Migrations
{
    public partial class dodaoKolicinu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "91d2f624-6788-4a2a-8f4d-3f41f2281bc3");

            migrationBuilder.AddColumn<int>(
                name: "contentProduct",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName", "firstName", "lastName" },
                values: new object[] { "fffafbdb-d804-4cbb-bc01-78953a165a55", 0, "10c66a2c-780e-4333-bd22-38175b138163", "User", null, false, false, null, null, null, null, null, false, "bcd57df1-655f-4273-917a-5e7b6e0f07c3", false, "proyectant", "Almedin", "Ljajic" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "fffafbdb-d804-4cbb-bc01-78953a165a55");

            migrationBuilder.DropColumn(
                name: "contentProduct",
                table: "Products");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName", "firstName", "lastName" },
                values: new object[] { "91d2f624-6788-4a2a-8f4d-3f41f2281bc3", 0, "4bdd67d7-45e6-4ece-94bf-0a793cdc30a4", "User", null, false, false, null, null, null, null, null, false, "1b77b02e-4cbe-480c-a752-f380cfe54e9b", false, "proyectant", "Almedin", "Ljajic" });
        }
    }
}
