using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StackOverloadProject.Data.Migrations
{
    public partial class CreateRelationshipWithUserAndAnswerComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "AnswerComment",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnswerComment_UserId",
                table: "AnswerComment",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerComment_AspNetUsers_UserId",
                table: "AnswerComment",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnswerComment_AspNetUsers_UserId",
                table: "AnswerComment");

            migrationBuilder.DropIndex(
                name: "IX_AnswerComment_UserId",
                table: "AnswerComment");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AnswerComment");
        }
    }
}
