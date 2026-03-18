using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FurnitureERP.Infrastructure.Migrations
{
    public partial class AddStockTransactions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StockTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MaterialId = table.Column<int>(type: "INTEGER", nullable: false),
                    MaterialName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    MaterialUnit = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Quantity = table.Column<decimal>(type: "TEXT", precision: 18, scale: 4, nullable: false),
                    StockBefore = table.Column<decimal>(type: "TEXT", precision: 18, scale: 4, nullable: false),
                    StockAfter = table.Column<decimal>(type: "TEXT", precision: 18, scale: 4, nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Reference = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Notes = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockTransactions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StockTransactions_MaterialId",
                table: "StockTransactions",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransactions_TransactionDate",
                table: "StockTransactions",
                column: "TransactionDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "StockTransactions");
        }
    }
}
