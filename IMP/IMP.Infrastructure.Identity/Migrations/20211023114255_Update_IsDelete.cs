using Microsoft.EntityFrameworkCore.Migrations;

namespace IMP.Infrastructure.Identity.Migrations
{
    public partial class Update_IsDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsDelete",
                schema: "Identity",
                table: "RefreshTokens",
                newName: "IsDeleted");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                schema: "Identity",
                table: "RefreshTokens",
                newName: "IsDelete");
        }
    }
}
