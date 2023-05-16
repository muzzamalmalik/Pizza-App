using Microsoft.EntityFrameworkCore.Migrations;

namespace PizzaOrder.Migrations
{
    public partial class addduplicatecheckinitmsize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.CreateIndex(
                name: "IX_ItemSize_SizeDescription_ItemId",
                table: "ItemSize",
                columns: new[] { "SizeDescription", "ItemId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ItemSize_SizeDescription_ItemId",
                table: "ItemSize");
        }
    }
}
