using Microsoft.EntityFrameworkCore.Migrations;

namespace Trash.Migrations
{
    public partial class AddImageURLToCommodity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageURL",
                table: "Commodities",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageURL",
                table: "Commodities");
        }
    }
}
