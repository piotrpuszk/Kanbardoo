using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kanbardoo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UseCorrectForeignKeyToBoardTableInKanUserBoardRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserBoardsRoles_Boards_KanBoardID",
                table: "UserBoardsRoles");

            migrationBuilder.DropIndex(
                name: "IX_UserBoardsRoles_KanBoardID",
                table: "UserBoardsRoles");

            migrationBuilder.DropColumn(
                name: "KanBoardID",
                table: "UserBoardsRoles");

            migrationBuilder.CreateIndex(
                name: "IX_UserBoardsRoles_BoardID",
                table: "UserBoardsRoles",
                column: "BoardID");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBoardsRoles_Boards_BoardID",
                table: "UserBoardsRoles",
                column: "BoardID",
                principalTable: "Boards",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserBoardsRoles_Boards_BoardID",
                table: "UserBoardsRoles");

            migrationBuilder.DropIndex(
                name: "IX_UserBoardsRoles_BoardID",
                table: "UserBoardsRoles");

            migrationBuilder.AddColumn<int>(
                name: "KanBoardID",
                table: "UserBoardsRoles",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserBoardsRoles_KanBoardID",
                table: "UserBoardsRoles",
                column: "KanBoardID");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBoardsRoles_Boards_KanBoardID",
                table: "UserBoardsRoles",
                column: "KanBoardID",
                principalTable: "Boards",
                principalColumn: "ID");
        }
    }
}
