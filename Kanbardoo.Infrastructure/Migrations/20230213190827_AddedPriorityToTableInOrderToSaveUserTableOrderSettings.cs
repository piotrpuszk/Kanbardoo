using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kanbardoo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedPriorityToTableInOrderToSaveUserTableOrderSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "Tables",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "ID",
                keyValue: 46920,
                column: "CreationDate",
                value: new DateTime(2023, 2, 13, 19, 8, 27, 360, DateTimeKind.Utc).AddTicks(9352));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Priority",
                table: "Tables");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "ID",
                keyValue: 46920,
                column: "CreationDate",
                value: new DateTime(2023, 2, 13, 19, 0, 21, 532, DateTimeKind.Utc).AddTicks(3288));
        }
    }
}
