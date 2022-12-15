using Microsoft.EntityFrameworkCore.Migrations;

namespace PizzaOrder.Migrations
{
    public partial class slideshowimage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SlideShowId",
                table: "SlideShowImages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SlideShowImages_SlideShowId",
                table: "SlideShowImages",
                column: "SlideShowId");

            migrationBuilder.AddForeignKey(
                name: "FK_SlideShowImages_SlideShow_SlideShowId",
                table: "SlideShowImages",
                column: "SlideShowId",
                principalTable: "SlideShow",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SlideShowImages_SlideShow_SlideShowId",
                table: "SlideShowImages");

            migrationBuilder.DropIndex(
                name: "IX_SlideShowImages_SlideShowId",
                table: "SlideShowImages");

            migrationBuilder.DropColumn(
                name: "SlideShowId",
                table: "SlideShowImages");
        }
    }
}
