using Microsoft.EntityFrameworkCore.Migrations;

namespace PizzaOrder.Migrations
{
    public partial class adddulicateindealsection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DealSection_DealId",
                table: "DealSection");

            migrationBuilder.CreateIndex(
                name: "IX_DealSection_DealId_CategoryId",
                table: "DealSection",
                columns: new[] { "DealId", "CategoryId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DealSection_DealId_CategoryId",
                table: "DealSection");

            migrationBuilder.CreateIndex(
                name: "IX_DealSection_DealId",
                table: "DealSection",
                column: "DealId");
        }
    }
}
