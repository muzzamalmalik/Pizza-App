using Microsoft.EntityFrameworkCore.Migrations;

namespace PizzaOrder.Migrations
{
    public partial class tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DealId",
                table: "DealSectionDetail");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "DealSection",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_DealSectionDetail_ItemId",
                table: "DealSectionDetail",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_DealSectionDetail_Items_ItemId",
                table: "DealSectionDetail",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DealSectionDetail_Items_ItemId",
                table: "DealSectionDetail");

            migrationBuilder.DropIndex(
                name: "IX_DealSectionDetail_ItemId",
                table: "DealSectionDetail");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "DealSection");

            migrationBuilder.AddColumn<int>(
                name: "DealId",
                table: "DealSectionDetail",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
