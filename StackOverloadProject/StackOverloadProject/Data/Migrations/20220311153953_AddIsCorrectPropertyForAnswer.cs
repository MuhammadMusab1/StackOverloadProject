using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StackOverloadProject.Data.Migrations
{
    public partial class AddIsCorrectPropertyForAnswer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCorrect",
                table: "Answer",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCorrect",
                table: "Answer");
        }
    }
}
