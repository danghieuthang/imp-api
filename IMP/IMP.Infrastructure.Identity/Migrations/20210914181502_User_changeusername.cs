using Microsoft.EntityFrameworkCore.Migrations;

namespace IMP.Infrastructure.Identity.Migrations
{
    public partial class User_changeusername : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsChangeUsername",
                schema: "Identity",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsChangeUsername",
                schema: "Identity",
                table: "User");
        }
    }
}
