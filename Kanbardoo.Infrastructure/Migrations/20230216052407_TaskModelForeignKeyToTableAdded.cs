using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kanbardoo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TaskModelForeignKeyToTableAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Tables_TableID",
                table: "Tasks");

            migrationBuilder.AlterColumn<int>(
                name: "TableID",
                table: "Tasks",
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
                value: new DateTime(2023, 2, 16, 5, 24, 7, 391, DateTimeKind.Utc).AddTicks(2619));

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Tables_TableID",
                table: "Tasks",
                column: "TableID",
                principalTable: "Tables",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Tables_TableID",
                table: "Tasks");

            migrationBuilder.AlterColumn<int>(
                name: "TableID",
                table: "Tasks",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "ID",
                keyValue: 46920,
                column: "CreationDate",
                value: new DateTime(2023, 2, 13, 19, 8, 27, 360, DateTimeKind.Utc).AddTicks(9352));

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Tables_TableID",
                table: "Tasks",
                column: "TableID",
                principalTable: "Tables",
                principalColumn: "ID");
        }
    }
}
