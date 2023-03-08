using Microsoft.EntityFrameworkCore.Migrations;

namespace PizzaOrder.Migrations
{
    public partial class newfieldadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderStatus",
                table: "OrderTransaction",
                newName: "OrderStatusOld");

            migrationBuilder.AddColumn<int>(
                name: "CurrentStatus",
                table: "OrderTransaction",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentStatus",
                table: "OrderTransaction");

            migrationBuilder.RenameColumn(
                name: "OrderStatusOld",
                table: "OrderTransaction",
                newName: "OrderStatus");
        }
    }
}
