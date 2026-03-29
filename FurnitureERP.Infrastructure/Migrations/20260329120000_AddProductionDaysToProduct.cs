using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FurnitureERP.Infrastructure.Migrations
{
    public partial class AddProductionDaysToProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductionDays",
                table: "Products",
                type: "INTEGER",
                nullable: false,
                defaultValue: 1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductionDays",
                table: "Products");
        }
    }
}
