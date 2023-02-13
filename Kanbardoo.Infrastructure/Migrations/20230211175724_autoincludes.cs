using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kanbardoo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class autoincludes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "ID",
                keyValue: 46920,
                column: "CreationDate",
                value: new DateTime(2023, 2, 11, 17, 57, 24, 112, DateTimeKind.Utc).AddTicks(8640));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "ID",
                keyValue: 46920,
                column: "CreationDate",
                value: new DateTime(2023, 2, 11, 17, 24, 20, 662, DateTimeKind.Utc).AddTicks(3552));
        }
    }
}
