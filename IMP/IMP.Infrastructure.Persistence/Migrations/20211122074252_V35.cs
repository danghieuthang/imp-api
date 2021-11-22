using Microsoft.EntityFrameworkCore.Migrations;

namespace IMP.Infrastructure.Persistence.Migrations
{
    public partial class V35 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "ApplicationUsers");

            migrationBuilder.AddColumn<int>(
                name: "RedirectId",
                table: "Notifications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ApplicationUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RedirectId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ApplicationUsers");

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "ApplicationUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
