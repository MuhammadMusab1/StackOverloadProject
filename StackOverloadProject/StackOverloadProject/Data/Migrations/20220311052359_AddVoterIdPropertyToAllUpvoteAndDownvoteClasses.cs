using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StackOverloadProject.Data.Migrations
{
    public partial class AddVoterIdPropertyToAllUpvoteAndDownvoteClasses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VotedByUserId",
                table: "UpvoteQuestion",
                newName: "VoterId");

            migrationBuilder.AddColumn<string>(
                name: "VoterId",
                table: "UpvoteAnswer",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "VoterId",
                table: "DownvoteQuestion",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "VoterId",
                table: "DownvoteAnswer",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VoterId",
                table: "UpvoteAnswer");

            migrationBuilder.DropColumn(
                name: "VoterId",
                table: "DownvoteQuestion");

            migrationBuilder.DropColumn(
                name: "VoterId",
                table: "DownvoteAnswer");

            migrationBuilder.RenameColumn(
                name: "VoterId",
                table: "UpvoteQuestion",
                newName: "VotedByUserId");
        }
    }
}
