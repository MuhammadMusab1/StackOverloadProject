using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StackOverloadProject.Data.Migrations
{
    public partial class CreateRelationshipBetweenUserAndAnswerClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Answer",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Answer_UserId",
                table: "Answer",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Answer_AspNetUsers_UserId",
                table: "Answer",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answer_AspNetUsers_UserId",
                table: "Answer");

            migrationBuilder.DropIndex(
                name: "IX_Answer_UserId",
                table: "Answer");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Answer");
        }
    }
}
