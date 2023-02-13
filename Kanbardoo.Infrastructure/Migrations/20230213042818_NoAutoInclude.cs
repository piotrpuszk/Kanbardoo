using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kanbardoo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NoAutoInclude : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "ID",
                keyValue: 46920,
                column: "CreationDate",
                value: new DateTime(2023, 2, 13, 4, 28, 18, 297, DateTimeKind.Utc).AddTicks(7619));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "ID",
                keyValue: 46920,
                column: "CreationDate",
                value: new DateTime(2023, 2, 11, 17, 57, 24, 112, DateTimeKind.Utc).AddTicks(8640));
        }
    }
}
