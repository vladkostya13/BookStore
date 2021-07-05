using Microsoft.EntityFrameworkCore.Migrations;

namespace BookStore.Infrastructure.Migrations
{
    public partial class AddedRolesToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "bfe3f0ea-019b-4e05-8626-5f623504bdd2", "d41db05b-67d1-40b4-a688-e6b070a75596", "User", "USER" },
                    { "31abe170-ae18-4a30-a58e-cda3a71da961", "ed74afe4-2d8b-4265-9cd5-56ce78f253e8", "Administrator", "ADMINISTRATOR" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "31abe170-ae18-4a30-a58e-cda3a71da961");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bfe3f0ea-019b-4e05-8626-5f623504bdd2");
        }
    }
}
