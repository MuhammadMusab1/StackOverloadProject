using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StackOverloadProject.Data.Migrations
{
    public partial class AddDownvoteAnswerClassAndItsRelationships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DownvoteAnswer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnswerId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DownvoteAnswer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DownvoteAnswer_Answer_AnswerId",
                        column: x => x.AnswerId,
                        principalTable: "Answer",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DownvoteAnswer_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DownvoteAnswer_AnswerId",
                table: "DownvoteAnswer",
                column: "AnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_DownvoteAnswer_UserId",
                table: "DownvoteAnswer",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DownvoteAnswer");
        }
    }
}
