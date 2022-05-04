using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StackOverloadProject.Data.Migrations
{
    public partial class AddDownvoteClassAndItsRelationships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DownvoteQuestion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DownvoteQuestion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DownvoteQuestion_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DownvoteQuestion_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DownvoteQuestion_QuestionId",
                table: "DownvoteQuestion",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_DownvoteQuestion_UserId",
                table: "DownvoteQuestion",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DownvoteQuestion");
        }
    }
}
