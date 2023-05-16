using Microsoft.EntityFrameworkCore.Migrations;

namespace PizzaOrder.Migrations
{
    public partial class addduplicatecheckindeals : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DealSectionDetail_DealSectionId",
                table: "DealSectionDetail");

            migrationBuilder.CreateIndex(
                name: "IX_DealSectionDetail_DealSectionId_ItemId",
                table: "DealSectionDetail",
                columns: new[] { "DealSectionId", "ItemId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DealSectionDetail_DealSectionId_ItemId",
                table: "DealSectionDetail");

            migrationBuilder.CreateIndex(
                name: "IX_DealSectionDetail_DealSectionId",
                table: "DealSectionDetail",
                column: "DealSectionId");
        }
    }
}
