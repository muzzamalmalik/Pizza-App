using Microsoft.EntityFrameworkCore.Migrations;

namespace PizzaOrder.Migrations
{
    public partial class orderdetailtablefieldadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Instructions",
                table: "OrderDetail",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Instructions",
                table: "OrderDetail");
        }
    }
}
