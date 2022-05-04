using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StackOverloadProject.Data.Migrations
{
    public partial class RenameUpvoteToUpvoteQuestion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Upvote");

            migrationBuilder.CreateTable(
                name: "UpvoteQuestion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UpvoteQuestion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UpvoteQuestion_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UpvoteQuestion_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UpvoteQuestion_QuestionId",
                table: "UpvoteQuestion",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_UpvoteQuestion_UserId",
                table: "UpvoteQuestion",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UpvoteQuestion");

            migrationBuilder.CreateTable(
                name: "Upvote",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Upvote", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Upvote_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Upvote_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Upvote_QuestionId",
                table: "Upvote",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Upvote_UserId",
                table: "Upvote",
                column: "UserId");
        }
    }
}
