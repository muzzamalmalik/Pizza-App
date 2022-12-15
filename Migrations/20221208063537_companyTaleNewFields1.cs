using Microsoft.EntityFrameworkCore.Migrations;

namespace PizzaOrder.Migrations
{
    public partial class companyTaleNewFields1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SecondaryContractPerson",
                table: "Company",
                newName: "SecondaryContactPerson");

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Company",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Company",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Company");

            migrationBuilder.RenameColumn(
                name: "SecondaryContactPerson",
                table: "Company",
                newName: "SecondaryContractPerson");
        }
    }
}
