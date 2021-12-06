using Microsoft.EntityFrameworkCore.Migrations;

namespace IMP.Infrastructure.Persistence.Migrations
{
    public partial class V45 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApplicationUserId",
                table: "VoucherInfluencers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VoucherInfluencers_ApplicationUserId",
                table: "VoucherInfluencers",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_VoucherInfluencers_ApplicationUsers_ApplicationUserId",
                table: "VoucherInfluencers",
                column: "ApplicationUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VoucherInfluencers_ApplicationUsers_ApplicationUserId",
                table: "VoucherInfluencers");

            migrationBuilder.DropIndex(
                name: "IX_VoucherInfluencers_ApplicationUserId",
                table: "VoucherInfluencers");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "VoucherInfluencers");
        }
    }
}
