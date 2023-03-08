using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PizzaOrder.Migrations
{
    public partial class toppingCartDetailsTableAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DealCartDetails");

            migrationBuilder.DropColumn(
                name: "ToppingId",
                table: "OrderDetail");

            migrationBuilder.AddColumn<int>(
                name: "BillMode",
                table: "OrderDetail",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ToppingCartDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ToppingId = table.Column<int>(type: "int", nullable: false),
                    OrderDetailId = table.Column<int>(type: "int", nullable: false),
                    CretedById = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateById = table.Column<int>(type: "int", nullable: true),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToppingCartDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ToppingCartDetails_OrderDetail_OrderDetailId",
                        column: x => x.OrderDetailId,
                        principalTable: "OrderDetail",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ToppingCartDetails_Toppings_ToppingId",
                        column: x => x.ToppingId,
                        principalTable: "Toppings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ToppingCartDetails_OrderDetailId",
                table: "ToppingCartDetails",
                column: "OrderDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_ToppingCartDetails_ToppingId",
                table: "ToppingCartDetails",
                column: "ToppingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ToppingCartDetails");

            migrationBuilder.DropColumn(
                name: "BillMode",
                table: "OrderDetail");

            migrationBuilder.AddColumn<int>(
                name: "ToppingId",
                table: "OrderDetail",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DealCartDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    CretedById = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DealId = table.Column<int>(type: "int", nullable: true),
                    DealSectionId = table.Column<int>(type: "int", nullable: true),
                    FlavourName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemId = table.Column<int>(type: "int", nullable: true),
                    OrderId = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealCartDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DealCartDetails_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DealCartDetails_Deals_DealId",
                        column: x => x.DealId,
                        principalTable: "Deals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DealCartDetails_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DealCartDetails_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DealCartDetails_CategoryId",
                table: "DealCartDetails",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_DealCartDetails_DealId",
                table: "DealCartDetails",
                column: "DealId");

            migrationBuilder.CreateIndex(
                name: "IX_DealCartDetails_ItemId",
                table: "DealCartDetails",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_DealCartDetails_OrderId",
                table: "DealCartDetails",
                column: "OrderId");
        }
    }
}
