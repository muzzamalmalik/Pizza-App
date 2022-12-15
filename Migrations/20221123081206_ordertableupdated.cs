using Microsoft.EntityFrameworkCore.Migrations;

namespace PizzaOrder.Migrations
{
    public partial class ordertableupdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReferenceTypeId",
                table: "OrderDetail",
                newName: "ToppingId");

            migrationBuilder.RenameColumn(
                name: "ReferenceId",
                table: "OrderDetail",
                newName: "ItemSizeId");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "OrderDetail",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CrustId",
                table: "OrderDetail",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DealId",
                table: "OrderDetail",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                table: "OrderDetail",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_CategoryId",
                table: "OrderDetail",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_CrustId",
                table: "OrderDetail",
                column: "CrustId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_DealId",
                table: "OrderDetail",
                column: "DealId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_ItemId",
                table: "OrderDetail",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_ItemSizeId",
                table: "OrderDetail",
                column: "ItemSizeId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_ToppingId",
                table: "OrderDetail",
                column: "ToppingId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_Category_CategoryId",
                table: "OrderDetail",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_Crusts_CrustId",
                table: "OrderDetail",
                column: "CrustId",
                principalTable: "Crusts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_Deals_DealId",
                table: "OrderDetail",
                column: "DealId",
                principalTable: "Deals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_Items_ItemId",
                table: "OrderDetail",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_ItemSize_ItemSizeId",
                table: "OrderDetail",
                column: "ItemSizeId",
                principalTable: "ItemSize",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_Toppings_ToppingId",
                table: "OrderDetail",
                column: "ToppingId",
                principalTable: "Toppings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_Category_CategoryId",
                table: "OrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_Crusts_CrustId",
                table: "OrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_Deals_DealId",
                table: "OrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_Items_ItemId",
                table: "OrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_ItemSize_ItemSizeId",
                table: "OrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_Toppings_ToppingId",
                table: "OrderDetail");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetail_CategoryId",
                table: "OrderDetail");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetail_CrustId",
                table: "OrderDetail");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetail_DealId",
                table: "OrderDetail");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetail_ItemId",
                table: "OrderDetail");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetail_ItemSizeId",
                table: "OrderDetail");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetail_ToppingId",
                table: "OrderDetail");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "OrderDetail");

            migrationBuilder.DropColumn(
                name: "CrustId",
                table: "OrderDetail");

            migrationBuilder.DropColumn(
                name: "DealId",
                table: "OrderDetail");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "OrderDetail");

            migrationBuilder.RenameColumn(
                name: "ToppingId",
                table: "OrderDetail",
                newName: "ReferenceTypeId");

            migrationBuilder.RenameColumn(
                name: "ItemSizeId",
                table: "OrderDetail",
                newName: "ReferenceId");
        }
    }
}
