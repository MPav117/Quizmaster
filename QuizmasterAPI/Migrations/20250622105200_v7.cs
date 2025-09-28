using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizmasterAPI.Migrations
{
    /// <inheritdoc />
    public partial class v7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Lobbies_InLobbyID",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_InLobbyID",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "InLobbyID",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InLobbyID",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Users_InLobbyID",
                table: "Users",
                column: "InLobbyID");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Lobbies_InLobbyID",
                table: "Users",
                column: "InLobbyID",
                principalTable: "Lobbies",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
