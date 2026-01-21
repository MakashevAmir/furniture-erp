using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FurnitureERP.Infrastructure.Migrations
{
    public partial class AddMaterialsEmployeesAndBOM : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    Position = table.Column<string>(type: "TEXT", nullable: false),
                    HourlyRate = table.Column<decimal>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LaborBoms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProductId = table.Column<int>(type: "INTEGER", nullable: false),
                    EmployeeId = table.Column<int>(type: "INTEGER", nullable: true),
                    Position = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    HoursRequired = table.Column<decimal>(type: "TEXT", precision: 18, scale: 4, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    SequenceNumber = table.Column<int>(type: "INTEGER", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LaborBoms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LaborBoms_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MaterialBoms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProductId = table.Column<int>(type: "INTEGER", nullable: false),
                    MaterialId = table.Column<int>(type: "INTEGER", nullable: false),
                    QuantityRequired = table.Column<decimal>(type: "TEXT", precision: 18, scale: 4, nullable: false),
                    WastagePercentage = table.Column<decimal>(type: "TEXT", precision: 5, scale: 2, nullable: false, defaultValue: 0m),
                    Notes = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialBoms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialBoms_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Materials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Category = table.Column<string>(type: "TEXT", nullable: false),
                    Unit = table.Column<string>(type: "TEXT", nullable: false),
                    PricePerUnit = table.Column<decimal>(type: "TEXT", nullable: false),
                    CurrentStock = table.Column<decimal>(type: "TEXT", nullable: false),
                    MinimumStock = table.Column<decimal>(type: "TEXT", nullable: false),
                    Supplier = table.Column<string>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materials", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LaborBoms_EmployeeId",
                table: "LaborBoms",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_LaborBoms_Position",
                table: "LaborBoms",
                column: "Position");

            migrationBuilder.CreateIndex(
                name: "IX_LaborBoms_ProductId",
                table: "LaborBoms",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_LaborBoms_ProductId_SequenceNumber",
                table: "LaborBoms",
                columns: new[] { "ProductId", "SequenceNumber" });

            migrationBuilder.CreateIndex(
                name: "IX_MaterialBoms_MaterialId",
                table: "MaterialBoms",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialBoms_ProductId",
                table: "MaterialBoms",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialBoms_ProductId_MaterialId",
                table: "MaterialBoms",
                columns: new[] { "ProductId", "MaterialId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "LaborBoms");

            migrationBuilder.DropTable(
                name: "MaterialBoms");

            migrationBuilder.DropTable(
                name: "Materials");
        }
    }
}
