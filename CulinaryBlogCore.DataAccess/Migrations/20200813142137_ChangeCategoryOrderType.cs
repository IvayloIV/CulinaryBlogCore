using Microsoft.EntityFrameworkCore.Migrations;

namespace CulinaryBlogCore.DataAccess.Migrations
{
    public partial class ChangeCategoryOrderType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "Order",
                table: "Category",
                nullable: false,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Order",
                table: "Category",
                nullable: false,
                oldClrType: typeof(long));
        }
    }
}
