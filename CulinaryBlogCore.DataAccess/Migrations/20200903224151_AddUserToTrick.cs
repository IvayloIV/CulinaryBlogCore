using Microsoft.EntityFrameworkCore.Migrations;

namespace CulinaryBlogCore.DataAccess.Migrations
{
    public partial class AddUserToTrick : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Trick",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trick_UserId",
                table: "Trick",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trick_AspNetUsers_UserId",
                table: "Trick",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trick_AspNetUsers_UserId",
                table: "Trick");

            migrationBuilder.DropIndex(
                name: "IX_Trick_UserId",
                table: "Trick");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Trick");
        }
    }
}
