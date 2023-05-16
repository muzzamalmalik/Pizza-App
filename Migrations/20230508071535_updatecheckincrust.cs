using Microsoft.EntityFrameworkCore.Migrations;

namespace PizzaOrder.Migrations
{
    public partial class updatecheckincrust : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Toppings_Name_CompanyId_ItemSizeId_ItemId",
                table: "Toppings");

            migrationBuilder.DropIndex(
                name: "IX_Crusts_Name_CompanyId_ItemSizeId_ItemId",
                table: "Crusts");

            migrationBuilder.CreateIndex(
                name: "IX_Toppings_ItemId",
                table: "Toppings",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Toppings_Name_CompanyId_ItemSizeId",
                table: "Toppings",
                columns: new[] { "Name", "CompanyId", "ItemSizeId" },
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Crusts_ItemId",
                table: "Crusts",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Crusts_Name_CompanyId_ItemSizeId",
                table: "Crusts",
                columns: new[] { "Name", "CompanyId", "ItemSizeId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Crusts_Items_ItemId",
                table: "Crusts",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Toppings_Items_ItemId",
                table: "Toppings",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Crusts_Items_ItemId",
                table: "Crusts");

            migrationBuilder.DropForeignKey(
                name: "FK_Toppings_Items_ItemId",
                table: "Toppings");

            migrationBuilder.DropIndex(
                name: "IX_Toppings_ItemId",
                table: "Toppings");

            migrationBuilder.DropIndex(
                name: "IX_Toppings_Name_CompanyId_ItemSizeId",
                table: "Toppings");

            migrationBuilder.DropIndex(
                name: "IX_Crusts_ItemId",
                table: "Crusts");

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
    }
}
