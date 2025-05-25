using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizmasterAPI.Migrations
{
    /// <inheritdoc />
    public partial class V3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lobbies_Quizzes_QuizID",
                table: "Lobbies");

            migrationBuilder.AddColumn<int>(
                name: "InLobbyID",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CreatorID",
                table: "Quizzes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "Questions",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "QuizID",
                table: "Lobbies",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "CreatorID",
                table: "Lobbies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Lobbies",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "IsPrivate",
                table: "Lobbies",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Lobbies",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Users_InLobbyID",
                table: "Users",
                column: "InLobbyID");

            migrationBuilder.CreateIndex(
                name: "IX_Quizzes_CreatorID",
                table: "Quizzes",
                column: "CreatorID");

            migrationBuilder.CreateIndex(
                name: "IX_Lobbies_CreatorID",
                table: "Lobbies",
                column: "CreatorID");

            migrationBuilder.AddForeignKey(
                name: "FK_Lobbies_Quizzes_QuizID",
                table: "Lobbies",
                column: "QuizID",
                principalTable: "Quizzes",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Lobbies_Users_CreatorID",
                table: "Lobbies",
                column: "CreatorID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Quizzes_Users_CreatorID",
                table: "Quizzes",
                column: "CreatorID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Lobbies_InLobbyID",
                table: "Users",
                column: "InLobbyID",
                principalTable: "Lobbies",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lobbies_Quizzes_QuizID",
                table: "Lobbies");

            migrationBuilder.DropForeignKey(
                name: "FK_Lobbies_Users_CreatorID",
                table: "Lobbies");

            migrationBuilder.DropForeignKey(
                name: "FK_Quizzes_Users_CreatorID",
                table: "Quizzes");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Lobbies_InLobbyID",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_InLobbyID",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Quizzes_CreatorID",
                table: "Quizzes");

            migrationBuilder.DropIndex(
                name: "IX_Lobbies_CreatorID",
                table: "Lobbies");

            migrationBuilder.DropColumn(
                name: "InLobbyID",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CreatorID",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "CreatorID",
                table: "Lobbies");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Lobbies");

            migrationBuilder.DropColumn(
                name: "IsPrivate",
                table: "Lobbies");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Lobbies");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Image",
                keyValue: null,
                column: "Image",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "Questions",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "QuizID",
                table: "Lobbies",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Lobbies_Quizzes_QuizID",
                table: "Lobbies",
                column: "QuizID",
                principalTable: "Quizzes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
