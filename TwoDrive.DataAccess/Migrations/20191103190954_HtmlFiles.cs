using Microsoft.EntityFrameworkCore.Migrations;

namespace TwoDrive.DataAccess.Migrations
{
    public partial class HtmlFiles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Extension",
                table: "Elements",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ShouldRender",
                table: "Elements",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Extension",
                table: "Elements");

            migrationBuilder.DropColumn(
                name: "ShouldRender",
                table: "Elements");
        }
    }
}
