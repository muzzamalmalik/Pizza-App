using Microsoft.EntityFrameworkCore.Migrations;

namespace PizzaOrder.Migrations
{
    public partial class addforignkeyinorders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Orders_UpdateById",
                table: "Orders",
                column: "UpdateById");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_UpdateById",
                table: "Orders",
                column: "UpdateById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_UpdateById",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_UpdateById",
                table: "Orders");
        }
    }
}
