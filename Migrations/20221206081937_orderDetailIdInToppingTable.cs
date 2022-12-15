using Microsoft.EntityFrameworkCore.Migrations;

namespace PizzaOrder.Migrations
{
    public partial class orderDetailIdInToppingTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderDetailId",
                table: "Toppings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Toppings_OrderDetailId",
                table: "Toppings",
                column: "OrderDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_Toppings_OrderDetail_OrderDetailId",
                table: "Toppings",
                column: "OrderDetailId",
                principalTable: "OrderDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Toppings_OrderDetail_OrderDetailId",
                table: "Toppings");

            migrationBuilder.DropIndex(
                name: "IX_Toppings_OrderDetailId",
                table: "Toppings");

            migrationBuilder.DropColumn(
                name: "OrderDetailId",
                table: "Toppings");
        }
    }
}
