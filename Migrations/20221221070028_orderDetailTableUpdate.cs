using Microsoft.EntityFrameworkCore.Migrations;

namespace PizzaOrder.Migrations
{
    public partial class orderDetailTableUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
