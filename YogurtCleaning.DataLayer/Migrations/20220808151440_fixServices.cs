using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace YogurtCleaning.DataLayer.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class fixServices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Service_Bundle_BundleId",
                table: "Service");

            migrationBuilder.DropForeignKey(
                name: "FK_Service_Cleaner_CleanerId",
                table: "Service");

            migrationBuilder.DropIndex(
                name: "IX_Service_BundleId",
                table: "Service");

            migrationBuilder.DropIndex(
                name: "IX_Service_CleanerId",
                table: "Service");

            migrationBuilder.DropColumn(
                name: "BundleId",
                table: "Service");

            migrationBuilder.DropColumn(
                name: "CleanerId",
                table: "Service");

            migrationBuilder.CreateTable(
                name: "BundleService",
                columns: table => new
                {
                    BundlesId = table.Column<int>(type: "int", nullable: false),
                    ServicesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BundleService", x => new { x.BundlesId, x.ServicesId });
                    table.ForeignKey(
                        name: "FK_BundleService_Bundle_BundlesId",
                        column: x => x.BundlesId,
                        principalTable: "Bundle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BundleService_Service_ServicesId",
                        column: x => x.ServicesId,
                        principalTable: "Service",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CleanerService",
                columns: table => new
                {
                    CleanersId = table.Column<int>(type: "int", nullable: false),
                    ServicesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CleanerService", x => new { x.CleanersId, x.ServicesId });
                    table.ForeignKey(
                        name: "FK_CleanerService_Cleaner_CleanersId",
                        column: x => x.CleanersId,
                        principalTable: "Cleaner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CleanerService_Service_ServicesId",
                        column: x => x.ServicesId,
                        principalTable: "Service",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BundleService_ServicesId",
                table: "BundleService",
                column: "ServicesId");

            migrationBuilder.CreateIndex(
                name: "IX_CleanerService_ServicesId",
                table: "CleanerService",
                column: "ServicesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BundleService");

            migrationBuilder.DropTable(
                name: "CleanerService");

            migrationBuilder.AddColumn<int>(
                name: "BundleId",
                table: "Service",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CleanerId",
                table: "Service",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Service_BundleId",
                table: "Service",
                column: "BundleId");

            migrationBuilder.CreateIndex(
                name: "IX_Service_CleanerId",
                table: "Service",
                column: "CleanerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Service_Bundle_BundleId",
                table: "Service",
                column: "BundleId",
                principalTable: "Bundle",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Service_Cleaner_CleanerId",
                table: "Service",
                column: "CleanerId",
                principalTable: "Cleaner",
                principalColumn: "Id");
        }
    }
}
