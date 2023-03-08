using Microsoft.EntityFrameworkCore.Migrations;

namespace PizzaOrder.Migrations
{
    public partial class dealCartDetailsTableUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DealCartDetails_DealSection_DealSectionId",
                table: "DealCartDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_DealCartDetails_OrderDetail_OrderDetailId",
                table: "DealCartDetails");

            migrationBuilder.DropIndex(
                name: "IX_DealCartDetails_DealSectionId",
                table: "DealCartDetails");

            migrationBuilder.RenameColumn(
                name: "OrderDetailId",
                table: "DealCartDetails",
                newName: "OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_DealCartDetails_OrderDetailId",
                table: "DealCartDetails",
                newName: "IX_DealCartDetails_OrderId");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "DealCartDetails",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DealId",
                table: "DealCartDetails",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "DealCartDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DealCartDetails_CategoryId",
                table: "DealCartDetails",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_DealCartDetails_DealId",
                table: "DealCartDetails",
                column: "DealId");

            migrationBuilder.AddForeignKey(
                name: "FK_DealCartDetails_Category_CategoryId",
                table: "DealCartDetails",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DealCartDetails_Deals_DealId",
                table: "DealCartDetails",
                column: "DealId",
                principalTable: "Deals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DealCartDetails_Orders_OrderId",
                table: "DealCartDetails",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DealCartDetails_Category_CategoryId",
                table: "DealCartDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_DealCartDetails_Deals_DealId",
                table: "DealCartDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_DealCartDetails_Orders_OrderId",
                table: "DealCartDetails");

            migrationBuilder.DropIndex(
                name: "IX_DealCartDetails_CategoryId",
                table: "DealCartDetails");

            migrationBuilder.DropIndex(
                name: "IX_DealCartDetails_DealId",
                table: "DealCartDetails");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "DealCartDetails");

            migrationBuilder.DropColumn(
                name: "DealId",
                table: "DealCartDetails");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "DealCartDetails");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "DealCartDetails",
                newName: "OrderDetailId");

            migrationBuilder.RenameIndex(
                name: "IX_DealCartDetails_OrderId",
                table: "DealCartDetails",
                newName: "IX_DealCartDetails_OrderDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_DealCartDetails_DealSectionId",
                table: "DealCartDetails",
                column: "DealSectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_DealCartDetails_DealSection_DealSectionId",
                table: "DealCartDetails",
                column: "DealSectionId",
                principalTable: "DealSection",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DealCartDetails_OrderDetail_OrderDetailId",
                table: "DealCartDetails",
                column: "OrderDetailId",
                principalTable: "OrderDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
