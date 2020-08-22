using Microsoft.EntityFrameworkCore.Migrations;

namespace CulinaryBlogCore.DataAccess.Migrations
{
    public partial class AddImgPathToCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BackgroundImgPath",
                table: "Category",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackgroundImgPath",
                table: "Category");
        }
    }
}
