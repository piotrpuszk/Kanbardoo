using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kanbardoo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AutoGenerationForStatusesRestoredAndAddedTestUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "TaskStatuses",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "BoardStatuses",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "ID", "CreationDate", "Name" },
                values: new object[] { 46920, new DateTime(2023, 2, 11, 17, 24, 20, 662, DateTimeKind.Utc).AddTicks(3552), "piotrpuszk" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "ID",
                keyValue: 46920);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "TaskStatuses",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "BoardStatuses",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);
        }
    }
}
