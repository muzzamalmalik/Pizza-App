using Microsoft.EntityFrameworkCore.Migrations;

namespace PizzaOrder.Migrations
{
    public partial class dealsectionforiegnkey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_DealSectionDetail_DealSectionId",
                table: "DealSectionDetail",
                column: "DealSectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_DealSectionDetail_DealSection_DealSectionId",
                table: "DealSectionDetail",
                column: "DealSectionId",
                principalTable: "DealSection",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DealSectionDetail_DealSection_DealSectionId",
                table: "DealSectionDetail");

            migrationBuilder.DropIndex(
                name: "IX_DealSectionDetail_DealSectionId",
                table: "DealSectionDetail");
        }
    }
}
