using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YogurtCleaning.DataLayer.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cleaner",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    Passport = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    DateOfStartWork = table.Column<DateTime>(type: "datetime", nullable: false),
                    Schedule = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<decimal>(type: "decimal(2,2)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cleaner", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Rating = table.Column<decimal>(type: "decimal(2,2)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "District",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_District", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CleanerDistrict",
                columns: table => new
                {
                    CleanersId = table.Column<int>(type: "int", nullable: false),
                    DistrictsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CleanerDistrict", x => new { x.CleanersId, x.DistrictsId });
                    table.ForeignKey(
                        name: "FK_CleanerDistrict_Cleaner_CleanersId",
                        column: x => x.CleanersId,
                        principalTable: "Cleaner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CleanerDistrict_District_DistrictsId",
                        column: x => x.DistrictsId,
                        principalTable: "District",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CleaningObject",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    NumberOfRooms = table.Column<int>(type: "int", nullable: false),
                    NumberOfBathrooms = table.Column<int>(type: "int", nullable: false),
                    Square = table.Column<int>(type: "int", nullable: false),
                    NumberOfWindows = table.Column<int>(type: "int", nullable: false),
                    NumberOfBalconies = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    DistrictId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CleaningObject", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CleaningObject_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CleaningObject_District_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "District",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CleaningObjectId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(7,2)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Order_CleaningObject_CleaningObjectId",
                        column: x => x.CleaningObjectId,
                        principalTable: "CleaningObject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Order_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Bundle",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    PriceForRoom = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    PriceForBathroom = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    PriceForSquareMeter = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bundle", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bundle_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CleanerOrder",
                columns: table => new
                {
                    CleanersBandId = table.Column<int>(type: "int", nullable: false),
                    OrdersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CleanerOrder", x => new { x.CleanersBandId, x.OrdersId });
                    table.ForeignKey(
                        name: "FK_CleanerOrder_Cleaner_CleanersBandId",
                        column: x => x.CleanersBandId,
                        principalTable: "Cleaner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CleanerOrder_Order_OrdersId",
                        column: x => x.OrdersId,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Summary = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    ClientId = table.Column<int>(type: "int", nullable: true),
                    CleanerId = table.Column<int>(type: "int", nullable: true),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comment_Cleaner_CleanerId",
                        column: x => x.CleanerId,
                        principalTable: "Cleaner",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Comment_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Comment_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Service",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    BundleId = table.Column<int>(type: "int", nullable: true),
                    CleanerId = table.Column<int>(type: "int", nullable: true),
                    OrderId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Service", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Service_Bundle_BundleId",
                        column: x => x.BundleId,
                        principalTable: "Bundle",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Service_Cleaner_CleanerId",
                        column: x => x.CleanerId,
                        principalTable: "Cleaner",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Service_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "District",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Admiralteisky" },
                    { 2, "Vasileostrovskiy" },
                    { 3, "Vyborgsky" },
                    { 4, "Kalininsky" },
                    { 5, "Kirovsky" },
                    { 6, "Kolpinsky" },
                    { 7, "Krasnogvardeisky" },
                    { 8, "Krasnoselsky" },
                    { 9, "Kronstadt" },
                    { 10, "Kurortniy" },
                    { 11, "Moskovsky" },
                    { 12, "Nevsky" },
                    { 13, "Petrogradsky" },
                    { 14, "Petrodvorets" },
                    { 15, "Primorsky" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bundle_OrderId",
                table: "Bundle",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_CleanerDistrict_DistrictsId",
                table: "CleanerDistrict",
                column: "DistrictsId");

            migrationBuilder.CreateIndex(
                name: "IX_CleanerOrder_OrdersId",
                table: "CleanerOrder",
                column: "OrdersId");

            migrationBuilder.CreateIndex(
                name: "IX_CleaningObject_ClientId",
                table: "CleaningObject",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_CleaningObject_DistrictId",
                table: "CleaningObject",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_CleanerId",
                table: "Comment",
                column: "CleanerId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_ClientId",
                table: "Comment",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_OrderId",
                table: "Comment",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_CleaningObjectId",
                table: "Order",
                column: "CleaningObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_ClientId",
                table: "Order",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Service_BundleId",
                table: "Service",
                column: "BundleId");

            migrationBuilder.CreateIndex(
                name: "IX_Service_CleanerId",
                table: "Service",
                column: "CleanerId");

            migrationBuilder.CreateIndex(
                name: "IX_Service_OrderId",
                table: "Service",
                column: "OrderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CleanerDistrict");

            migrationBuilder.DropTable(
                name: "CleanerOrder");

            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.DropTable(
                name: "Service");

            migrationBuilder.DropTable(
                name: "Bundle");

            migrationBuilder.DropTable(
                name: "Cleaner");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "CleaningObject");

            migrationBuilder.DropTable(
                name: "Client");

            migrationBuilder.DropTable(
                name: "District");
        }
    }
}
