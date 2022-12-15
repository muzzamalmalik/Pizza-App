using Microsoft.EntityFrameworkCore.Migrations;

namespace PizzaOrder.Migrations
{
    public partial class ItemSizeUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                table: "ItemSize",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemSize_ItemId",
                table: "ItemSize",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemSize_Items_ItemId",
                table: "ItemSize",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemSize_Items_ItemId",
                table: "ItemSize");

            migrationBuilder.DropIndex(
                name: "IX_ItemSize_ItemId",
                table: "ItemSize");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "ItemSize");
        }
    }
}
