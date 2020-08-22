using Microsoft.EntityFrameworkCore.Migrations;

namespace CulinaryBlogCore.DataAccess.Migrations
{
    public partial class AddFieldsToRecipe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "Rating",
                table: "Recipe",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AddColumn<long>(
                name: "ViewCount",
                table: "Recipe",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "VoteCount",
                table: "Recipe",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ViewCount",
                table: "Recipe");

            migrationBuilder.DropColumn(
                name: "VoteCount",
                table: "Recipe");

            migrationBuilder.AlterColumn<double>(
                name: "Rating",
                table: "Recipe",
                nullable: false,
                oldClrType: typeof(long));
        }
    }
}
