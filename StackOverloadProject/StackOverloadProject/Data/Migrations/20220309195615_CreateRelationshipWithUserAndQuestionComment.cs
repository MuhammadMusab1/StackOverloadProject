using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StackOverloadProject.Data.Migrations
{
    public partial class CreateRelationshipWithUserAndQuestionComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "QuestionComment",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuestionComment_UserId",
                table: "QuestionComment",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionComment_AspNetUsers_UserId",
                table: "QuestionComment",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionComment_AspNetUsers_UserId",
                table: "QuestionComment");

            migrationBuilder.DropIndex(
                name: "IX_QuestionComment_UserId",
                table: "QuestionComment");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "QuestionComment");
        }
    }
}
