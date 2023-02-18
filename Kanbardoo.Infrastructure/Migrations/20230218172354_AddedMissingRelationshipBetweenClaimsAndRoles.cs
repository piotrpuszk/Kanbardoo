using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kanbardoo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedMissingRelationshipBetweenClaimsAndRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Claims_Roles_KanRoleID",
                table: "Claims");

            migrationBuilder.DropIndex(
                name: "IX_Claims_KanRoleID",
                table: "Claims");

            migrationBuilder.DropColumn(
                name: "KanRoleID",
                table: "Claims");

            migrationBuilder.CreateTable(
                name: "RolesClaims",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleID = table.Column<int>(type: "INTEGER", nullable: false),
                    ClaimID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolesClaims", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RolesClaims_Claims_ClaimID",
                        column: x => x.ClaimID,
                        principalTable: "Claims",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolesClaims_Roles_RoleID",
                        column: x => x.RoleID,
                        principalTable: "Roles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RolesClaims_ClaimID",
                table: "RolesClaims",
                column: "ClaimID");

            migrationBuilder.CreateIndex(
                name: "IX_RolesClaims_RoleID",
                table: "RolesClaims",
                column: "RoleID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RolesClaims");

            migrationBuilder.AddColumn<int>(
                name: "KanRoleID",
                table: "Claims",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "ID",
                keyValue: 1,
                column: "KanRoleID",
                value: null);

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "ID",
                keyValue: 2,
                column: "KanRoleID",
                value: null);

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "ID",
                keyValue: 3,
                column: "KanRoleID",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Claims_KanRoleID",
                table: "Claims",
                column: "KanRoleID");

            migrationBuilder.AddForeignKey(
                name: "FK_Claims_Roles_KanRoleID",
                table: "Claims",
                column: "KanRoleID",
                principalTable: "Roles",
                principalColumn: "ID");
        }
    }
}
