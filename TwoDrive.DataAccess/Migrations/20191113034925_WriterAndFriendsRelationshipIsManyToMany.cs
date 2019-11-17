using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TwoDrive.DataAccess.Migrations
{
    public partial class WriterAndFriendsRelationshipIsManyToMany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomClaim_Elements_ElementId",
                table: "CustomClaim");

            migrationBuilder.DropForeignKey(
                name: "FK_Writers_Writers_FriendId",
                table: "Writers");

            migrationBuilder.DropIndex(
                name: "IX_Writers_FriendId",
                table: "Writers");

            migrationBuilder.DropColumn(
                name: "FriendId",
                table: "Writers");

            migrationBuilder.CreateTable(
                name: "WriterFriend",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    WriterId = table.Column<int>(nullable: false),
                    FriendId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WriterFriend", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WriterFriend_Writers_FriendId",
                        column: x => x.FriendId,
                        principalTable: "Writers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WriterFriend_Writers_WriterId",
                        column: x => x.WriterId,
                        principalTable: "Writers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WriterFriend_FriendId",
                table: "WriterFriend",
                column: "FriendId");

            migrationBuilder.CreateIndex(
                name: "IX_WriterFriend_WriterId",
                table: "WriterFriend",
                column: "WriterId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomClaim_Elements_ElementId",
                table: "CustomClaim",
                column: "ElementId",
                principalTable: "Elements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomClaim_Elements_ElementId",
                table: "CustomClaim");

            migrationBuilder.DropTable(
                name: "WriterFriend");

            migrationBuilder.AddColumn<int>(
                name: "FriendId",
                table: "Writers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Writers_FriendId",
                table: "Writers",
                column: "FriendId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomClaim_Elements_ElementId",
                table: "CustomClaim",
                column: "ElementId",
                principalTable: "Elements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Writers_Writers_FriendId",
                table: "Writers",
                column: "FriendId",
                principalTable: "Writers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
