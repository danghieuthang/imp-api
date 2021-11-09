using Microsoft.EntityFrameworkCore.Migrations;

namespace IMP.Infrastructure.Identity.Migrations
{
    public partial class AddBrandId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BrandId",
                schema: "Identity",
                table: "User",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BrandId",
                schema: "Identity",
                table: "User");
        }
    }
}
