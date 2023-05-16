using Microsoft.EntityFrameworkCore.Migrations;

namespace PizzaOrder.Migrations
{
    public partial class updateduplicatecheckintoppingandcrusts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Toppings_Name_CompanyId_ItemSizeId",
                table: "Toppings");

            migrationBuilder.DropIndex(
                name: "IX_Crusts_Name_CompanyId_ItemSizeId",
                table: "Crusts");

            migrationBuilder.CreateIndex(
                name: "IX_Toppings_Name_CompanyId_ItemSizeId_ItemId",
                table: "Toppings",
                columns: new[] { "Name", "CompanyId", "ItemSizeId", "ItemId" },
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Crusts_Name_CompanyId_ItemSizeId_ItemId",
                table: "Crusts",
                columns: new[] { "Name", "CompanyId", "ItemSizeId", "ItemId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Toppings_Name_CompanyId_ItemSizeId_ItemId",
                table: "Toppings");

            migrationBuilder.DropIndex(
                name: "IX_Crusts_Name_CompanyId_ItemSizeId_ItemId",
                table: "Crusts");

            migrationBuilder.CreateIndex(
                name: "IX_Toppings_Name_CompanyId_ItemSizeId",
                table: "Toppings",
                columns: new[] { "Name", "CompanyId", "ItemSizeId" },
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Crusts_Name_CompanyId_ItemSizeId",
                table: "Crusts",
                columns: new[] { "Name", "CompanyId", "ItemSizeId" },
                unique: true);
        }
    }
}
