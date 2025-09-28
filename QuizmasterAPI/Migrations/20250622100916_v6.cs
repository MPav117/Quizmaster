using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizmasterAPI.Migrations
{
    /// <inheritdoc />
    public partial class v6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConnectionId",
                table: "Sessions",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "Ready",
                table: "Sessions",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "OfferedAnswer1",
                table: "Questions",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "OfferedAnswer2",
                table: "Questions",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "OfferedAnswer3",
                table: "Questions",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "OfferedAnswer4",
                table: "Questions",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "PointValue",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ReadyPlayers",
                table: "Lobbies",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConnectionId",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "Ready",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "OfferedAnswer1",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "OfferedAnswer2",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "OfferedAnswer3",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "OfferedAnswer4",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "PointValue",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "ReadyPlayers",
                table: "Lobbies");
        }
    }
}
