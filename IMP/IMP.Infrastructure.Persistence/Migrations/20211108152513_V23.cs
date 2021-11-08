using Microsoft.EntityFrameworkCore.Migrations;

namespace IMP.Infrastructure.Persistence.Migrations
{
    public partial class V23 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUsers_Rankings_RankingId",
                table: "ApplicationUsers");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationUsers_RankingId",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "RankingId",
                table: "ApplicationUsers");

            migrationBuilder.CreateIndex(
                name: "IX_Rankings_InfluencerId",
                table: "Rankings",
                column: "InfluencerId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Rankings_ApplicationUsers_InfluencerId",
                table: "Rankings",
                column: "InfluencerId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rankings_ApplicationUsers_InfluencerId",
                table: "Rankings");

            migrationBuilder.DropIndex(
                name: "IX_Rankings_InfluencerId",
                table: "Rankings");

            migrationBuilder.AddColumn<int>(
                name: "RankingId",
                table: "ApplicationUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUsers_RankingId",
                table: "ApplicationUsers",
                column: "RankingId",
                unique: true,
                filter: "[RankingId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUsers_Rankings_RankingId",
                table: "ApplicationUsers",
                column: "RankingId",
                principalTable: "Rankings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
