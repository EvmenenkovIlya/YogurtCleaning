using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace YogurtCleaning.DataLayer.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class AddOrderEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bundle_Order_OrderId",
                table: "Bundle");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Client_ClientId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Service_Order_OrderId",
                table: "Service");

            migrationBuilder.DropIndex(
                name: "IX_Service_OrderId",
                table: "Service");

            migrationBuilder.DropIndex(
                name: "IX_Bundle_OrderId",
                table: "Bundle");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Service");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Bundle");

            migrationBuilder.DropColumn(
                name: "PriceForBathroom",
                table: "Bundle");

            migrationBuilder.DropColumn(
                name: "PriceForRoom",
                table: "Bundle");

            migrationBuilder.DropColumn(
                name: "PriceForSquareMeter",
                table: "Bundle");

            migrationBuilder.AlterColumn<int>(
                name: "ClientId",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Measure",
                table: "Bundle",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Bundle",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "BundleOrder",
                columns: table => new
                {
                    BundlesId = table.Column<int>(type: "int", nullable: false),
                    OrdersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BundleOrder", x => new { x.BundlesId, x.OrdersId });
                    table.ForeignKey(
                        name: "FK_BundleOrder_Bundle_BundlesId",
                        column: x => x.BundlesId,
                        principalTable: "Bundle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BundleOrder_Order_OrdersId",
                        column: x => x.OrdersId,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderService",
                columns: table => new
                {
                    OrdersId = table.Column<int>(type: "int", nullable: false),
                    ServicesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderService", x => new { x.OrdersId, x.ServicesId });
                    table.ForeignKey(
                        name: "FK_OrderService_Order_OrdersId",
                        column: x => x.OrdersId,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderService_Service_ServicesId",
                        column: x => x.ServicesId,
                        principalTable: "Service",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BundleOrder_OrdersId",
                table: "BundleOrder",
                column: "OrdersId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderService_ServicesId",
                table: "OrderService",
                column: "ServicesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Client_ClientId",
                table: "Order",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Client_ClientId",
                table: "Order");

            migrationBuilder.DropTable(
                name: "BundleOrder");

            migrationBuilder.DropTable(
                name: "OrderService");

            migrationBuilder.DropColumn(
                name: "Measure",
                table: "Bundle");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Bundle");

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Service",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ClientId",
                table: "Order",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Bundle",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PriceForBathroom",
                table: "Bundle",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PriceForRoom",
                table: "Bundle",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PriceForSquareMeter",
                table: "Bundle",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Service_OrderId",
                table: "Service",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Bundle_OrderId",
                table: "Bundle",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bundle_Order_OrderId",
                table: "Bundle",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Client_ClientId",
                table: "Order",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Service_Order_OrderId",
                table: "Service",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id");
        }
    }
}
