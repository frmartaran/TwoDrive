using Microsoft.EntityFrameworkCore.Migrations;

namespace TwoDrive.DataAccess.Migrations
{
    public partial class OneToManyRelationshipCustomClaim : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomClaim_Writers_WriterId",
                table: "CustomClaim");

            migrationBuilder.AlterColumn<int>(
                name: "WriterId",
                table: "CustomClaim",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomClaim_Writers_WriterId",
                table: "CustomClaim",
                column: "WriterId",
                principalTable: "Writers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomClaim_Writers_WriterId",
                table: "CustomClaim");

            migrationBuilder.AlterColumn<int>(
                name: "WriterId",
                table: "CustomClaim",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_CustomClaim_Writers_WriterId",
                table: "CustomClaim",
                column: "WriterId",
                principalTable: "Writers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
