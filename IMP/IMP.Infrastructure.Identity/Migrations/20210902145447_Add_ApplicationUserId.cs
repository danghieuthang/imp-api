using Microsoft.EntityFrameworkCore.Migrations;

namespace IMP.Infrastructure.Identity.Migrations
{
    public partial class Add_ApplicationUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApplicationUserId",
                schema: "Identity",
                table: "User",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_ApplicationUserId",
                schema: "Identity",
                table: "User",
                column: "ApplicationUserId",
                unique: true,
                filter: "[ApplicationUserId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_User_ApplicationUserId",
                schema: "Identity",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                schema: "Identity",
                table: "User");
        }
    }
}
