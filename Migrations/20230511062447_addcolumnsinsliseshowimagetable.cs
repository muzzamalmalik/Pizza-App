using Microsoft.EntityFrameworkCore.Migrations;

namespace PizzaOrder.Migrations
{
    public partial class addcolumnsinsliseshowimagetable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageDescription",
                table: "SlideShow");

            migrationBuilder.AddColumn<string>(
                name: "Heading",
                table: "SlideShowImages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageDescription",
                table: "SlideShowImages",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Heading",
                table: "SlideShowImages");

            migrationBuilder.DropColumn(
                name: "ImageDescription",
                table: "SlideShowImages");

            migrationBuilder.AddColumn<string>(
                name: "ImageDescription",
                table: "SlideShow",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
