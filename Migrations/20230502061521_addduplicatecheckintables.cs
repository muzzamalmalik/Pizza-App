using Microsoft.EntityFrameworkCore.Migrations;

namespace PizzaOrder.Migrations
{
    public partial class addduplicatecheckintables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Toppings_Name_CompanyId",
                table: "Toppings",
                columns: new[] { "Name", "CompanyId" },
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Crusts_Name_CompanyId",
                table: "Crusts",
                columns: new[] { "Name", "CompanyId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Toppings_Name_CompanyId",
                table: "Toppings");

            migrationBuilder.DropIndex(
                name: "IX_Crusts_Name_CompanyId",
                table: "Crusts");
        }
    }
}
