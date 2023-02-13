using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kanbardoo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ForeignKeyFromTableToBoard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tables_Boards_BoardID",
                table: "Tables");

            migrationBuilder.AlterColumn<int>(
                name: "BoardID",
                table: "Tables",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "ID",
                keyValue: 46920,
                column: "CreationDate",
                value: new DateTime(2023, 2, 13, 19, 0, 21, 532, DateTimeKind.Utc).AddTicks(3288));

            migrationBuilder.AddForeignKey(
                name: "FK_Tables_Boards_BoardID",
                table: "Tables",
                column: "BoardID",
                principalTable: "Boards",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tables_Boards_BoardID",
                table: "Tables");

            migrationBuilder.AlterColumn<int>(
                name: "BoardID",
                table: "Tables",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "ID",
                keyValue: 46920,
                column: "CreationDate",
                value: new DateTime(2023, 2, 13, 4, 28, 18, 297, DateTimeKind.Utc).AddTicks(7619));

            migrationBuilder.AddForeignKey(
                name: "FK_Tables_Boards_BoardID",
                table: "Tables",
                column: "BoardID",
                principalTable: "Boards",
                principalColumn: "ID");
        }
    }
}
