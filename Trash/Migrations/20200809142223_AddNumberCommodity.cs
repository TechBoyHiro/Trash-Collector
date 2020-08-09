using Microsoft.EntityFrameworkCore.Migrations;

namespace Trash.Migrations
{
    public partial class AddNumberCommodity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Number",
                table: "UserCommodities",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Stock",
                table: "Commodities",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Number",
                table: "UserCommodities");

            migrationBuilder.DropColumn(
                name: "Stock",
                table: "Commodities");
        }
    }
}
