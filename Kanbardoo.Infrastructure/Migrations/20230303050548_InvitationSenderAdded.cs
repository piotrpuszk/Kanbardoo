using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kanbardoo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InvitationSenderAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SenderID",
                table: "Invitations",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_SenderID",
                table: "Invitations",
                column: "SenderID");

            migrationBuilder.AddForeignKey(
                name: "FK_Invitations_Users_SenderID",
                table: "Invitations",
                column: "SenderID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invitations_Users_SenderID",
                table: "Invitations");

            migrationBuilder.DropIndex(
                name: "IX_Invitations_SenderID",
                table: "Invitations");

            migrationBuilder.DropColumn(
                name: "SenderID",
                table: "Invitations");
        }
    }
}
