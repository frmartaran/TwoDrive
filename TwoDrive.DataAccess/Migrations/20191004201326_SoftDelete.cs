using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TwoDrive.DataAccess.Migrations
{
    public partial class SoftDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Modifications",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Modifications",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Elements",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Elements",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Modifications");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Modifications");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Elements");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Elements");
        }
    }
}
