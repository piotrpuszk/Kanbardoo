using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kanbardoo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedUserBoardRolesToBoardEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsersRoles");

            migrationBuilder.CreateTable(
                name: "UserBoardsRoles",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserID = table.Column<int>(type: "INTEGER", nullable: false),
                    RoleID = table.Column<int>(type: "INTEGER", nullable: false),
                    BoardID = table.Column<int>(type: "INTEGER", nullable: false),
                    KanBoardID = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBoardsRoles", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UserBoardsRoles_Boards_KanBoardID",
                        column: x => x.KanBoardID,
                        principalTable: "Boards",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_UserBoardsRoles_Roles_RoleID",
                        column: x => x.RoleID,
                        principalTable: "Roles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserBoardsRoles_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserBoardsRoles_KanBoardID",
                table: "UserBoardsRoles",
                column: "KanBoardID");

            migrationBuilder.CreateIndex(
                name: "IX_UserBoardsRoles_RoleID",
                table: "UserBoardsRoles",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_UserBoardsRoles_UserID",
                table: "UserBoardsRoles",
                column: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserBoardsRoles");

            migrationBuilder.CreateTable(
                name: "UsersRoles",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleID = table.Column<int>(type: "INTEGER", nullable: false),
                    UserID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersRoles", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UsersRoles_Roles_RoleID",
                        column: x => x.RoleID,
                        principalTable: "Roles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsersRoles_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsersRoles_RoleID",
                table: "UsersRoles",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_UsersRoles_UserID",
                table: "UsersRoles",
                column: "UserID");
        }
    }
}
