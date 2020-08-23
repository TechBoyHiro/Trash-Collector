using Microsoft.EntityFrameworkCore.Migrations;

namespace Trash.Migrations
{
    public partial class AddImageURLTOTrashType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "UserLocations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "UserLocations",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageURL",
                table: "TrashTypes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "UserLocations");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "UserLocations");

            migrationBuilder.DropColumn(
                name: "ImageURL",
                table: "TrashTypes");
        }
    }
}
