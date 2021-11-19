using Microsoft.EntityFrameworkCore.Migrations;

namespace IMP.Infrastructure.Persistence.Migrations
{
    public partial class V32 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsCampany",
                table: "Brands",
                newName: "IsCompany");

            migrationBuilder.AddColumn<string>(
                name: "Introduction",
                table: "Brands",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Introduction",
                table: "Brands");

            migrationBuilder.RenameColumn(
                name: "IsCompany",
                table: "Brands",
                newName: "IsCampany");
        }
    }
}
