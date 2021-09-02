using Microsoft.EntityFrameworkCore.Migrations;

namespace IMP.Infrastructure.Persistence.Migrations
{
    public partial class Update_ranking_user : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ApplicationUsers_RankingId",
                table: "ApplicationUsers");

            migrationBuilder.AlterColumn<int>(
                name: "RankingId",
                table: "ApplicationUsers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUsers_RankingId",
                table: "ApplicationUsers",
                column: "RankingId",
                unique: true,
                filter: "[RankingId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ApplicationUsers_RankingId",
                table: "ApplicationUsers");

            migrationBuilder.AlterColumn<int>(
                name: "RankingId",
                table: "ApplicationUsers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUsers_RankingId",
                table: "ApplicationUsers",
                column: "RankingId",
                unique: true);
        }
    }
}
