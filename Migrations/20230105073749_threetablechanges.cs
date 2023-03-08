using Microsoft.EntityFrameworkCore.Migrations;

namespace PizzaOrder.Migrations
{
    public partial class threetablechanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Crusts_Category_CategoryId",
                table: "Crusts");

            migrationBuilder.DropForeignKey(
                name: "FK_Crusts_Items_ItemId",
                table: "Crusts");

            migrationBuilder.DropForeignKey(
                name: "FK_Crusts_ItemSize_ItemSizeId",
                table: "Crusts");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetailAdditionalDetails_Toppings_ToppingId",
                table: "OrderDetailAdditionalDetails");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetailAdditionalDetails_ToppingId",
                table: "OrderDetailAdditionalDetails");

            migrationBuilder.DropIndex(
                name: "IX_Crusts_CategoryId",
                table: "Crusts");

            migrationBuilder.DropIndex(
                name: "IX_Crusts_ItemId",
                table: "Crusts");

            migrationBuilder.DropIndex(
                name: "IX_Crusts_ItemSizeId",
                table: "Crusts");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Toppings");

            migrationBuilder.DropColumn(
                name: "ReferenceType",
                table: "OrderDetailAdditionalDetails");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Crusts");

            migrationBuilder.RenameColumn(
                name: "ToppingId",
                table: "OrderDetailAdditionalDetails",
                newName: "ReferenceTypeId");

            migrationBuilder.AddColumn<int>(
                name: "ReferenceId",
                table: "OrderDetailAdditionalDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReferenceId",
                table: "OrderDetailAdditionalDetails");

            migrationBuilder.RenameColumn(
                name: "ReferenceTypeId",
                table: "OrderDetailAdditionalDetails",
                newName: "ToppingId");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Toppings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ReferenceType",
                table: "OrderDetailAdditionalDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Crusts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetailAdditionalDetails_ToppingId",
                table: "OrderDetailAdditionalDetails",
                column: "ToppingId");

            migrationBuilder.CreateIndex(
                name: "IX_Crusts_CategoryId",
                table: "Crusts",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Crusts_ItemId",
                table: "Crusts",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Crusts_ItemSizeId",
                table: "Crusts",
                column: "ItemSizeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Crusts_Category_CategoryId",
                table: "Crusts",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Crusts_Items_ItemId",
                table: "Crusts",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Crusts_ItemSize_ItemSizeId",
                table: "Crusts",
                column: "ItemSizeId",
                principalTable: "ItemSize",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetailAdditionalDetails_Toppings_ToppingId",
                table: "OrderDetailAdditionalDetails",
                column: "ToppingId",
                principalTable: "Toppings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
