using Microsoft.EntityFrameworkCore.Migrations;

namespace PizzaOrder.Migrations
{
    public partial class CrustTableAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SizeId",
                table: "Toppings",
                newName: "ItemSizeId");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Toppings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                table: "Toppings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Toppings_CategoryId",
                table: "Toppings",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Toppings_CompanyId",
                table: "Toppings",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Toppings_ItemId",
                table: "Toppings",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Toppings_ItemSizeId",
                table: "Toppings",
                column: "ItemSizeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Toppings_Category_CategoryId",
                table: "Toppings",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Toppings_Company_CompanyId",
                table: "Toppings",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Toppings_Items_ItemId",
                table: "Toppings",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Toppings_ItemSize_ItemSizeId",
                table: "Toppings",
                column: "ItemSizeId",
                principalTable: "ItemSize",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Toppings_Category_CategoryId",
                table: "Toppings");

            migrationBuilder.DropForeignKey(
                name: "FK_Toppings_Company_CompanyId",
                table: "Toppings");

            migrationBuilder.DropForeignKey(
                name: "FK_Toppings_Items_ItemId",
                table: "Toppings");

            migrationBuilder.DropForeignKey(
                name: "FK_Toppings_ItemSize_ItemSizeId",
                table: "Toppings");

            migrationBuilder.DropIndex(
                name: "IX_Toppings_CategoryId",
                table: "Toppings");

            migrationBuilder.DropIndex(
                name: "IX_Toppings_CompanyId",
                table: "Toppings");

            migrationBuilder.DropIndex(
                name: "IX_Toppings_ItemId",
                table: "Toppings");

            migrationBuilder.DropIndex(
                name: "IX_Toppings_ItemSizeId",
                table: "Toppings");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Toppings");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "Toppings");

            migrationBuilder.RenameColumn(
                name: "ItemSizeId",
                table: "Toppings",
                newName: "SizeId");
        }
    }
}
