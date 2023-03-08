using Microsoft.EntityFrameworkCore.Migrations;

namespace PizzaOrder.Migrations
{
    public partial class update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Deals_CompanyId",
                table: "Deals");

            migrationBuilder.DropIndex(
                name: "IX_Deals_Title",
                table: "Deals");

            migrationBuilder.CreateIndex(
                name: "IX_Deals_CompanyId_Title",
                table: "Deals",
                columns: new[] { "CompanyId", "Title" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Deals_CompanyId_Title",
                table: "Deals");

            migrationBuilder.CreateIndex(
                name: "IX_Deals_CompanyId",
                table: "Deals",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Deals_Title",
                table: "Deals",
                column: "Title",
                unique: true);
        }
    }
}
