using Microsoft.EntityFrameworkCore.Migrations;

namespace PizzaOrder.Migrations
{
    public partial class removeforiegnkeyfromcrustsandtopping : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Crusts_Items_ItemId",
                table: "Crusts");

            migrationBuilder.DropForeignKey(
                name: "FK_Toppings_Items_ItemId",
                table: "Toppings");

            migrationBuilder.DropIndex(
                name: "IX_Toppings_ItemId",
                table: "Toppings");

            migrationBuilder.DropIndex(
                name: "IX_Crusts_ItemId",
                table: "Crusts");

            migrationBuilder.AlterColumn<int>(
                name: "ItemId",
                table: "Crusts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_CretedById",
                table: "OrderDetail",
                column: "CretedById");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_Users_CretedById",
                table: "OrderDetail",
                column: "CretedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_Users_CretedById",
                table: "OrderDetail");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetail_CretedById",
                table: "OrderDetail");

            migrationBuilder.AlterColumn<int>(
                name: "ItemId",
                table: "Crusts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Toppings_ItemId",
                table: "Toppings",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Crusts_ItemId",
                table: "Crusts",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Crusts_Items_ItemId",
                table: "Crusts",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Toppings_Items_ItemId",
                table: "Toppings",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
