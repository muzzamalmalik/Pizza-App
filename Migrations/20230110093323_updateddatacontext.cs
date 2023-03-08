using Microsoft.EntityFrameworkCore.Migrations;

namespace PizzaOrder.Migrations
{
    public partial class updateddatacontext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_Category_CategoryId",
                table: "OrderDetail");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetail_CategoryId",
                table: "OrderDetail");

            migrationBuilder.DropIndex(
                name: "IX_Items_CompanyId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_Name",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "OrderDetail");

            migrationBuilder.CreateIndex(
                name: "IX_Items_CompanyId_Name",
                table: "Items",
                columns: new[] { "CompanyId", "Name" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Items_CompanyId_Name",
                table: "Items");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "OrderDetail",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_CategoryId",
                table: "OrderDetail",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_CompanyId",
                table: "Items",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_Name",
                table: "Items",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_Category_CategoryId",
                table: "OrderDetail",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
