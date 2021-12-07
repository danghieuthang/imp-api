using Microsoft.EntityFrameworkCore.Migrations;

namespace IMP.Infrastructure.Persistence.Migrations
{
    public partial class V46 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Products",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Products",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VoucherInfluencers_InfluencerId",
                table: "VoucherInfluencers",
                column: "InfluencerId");

            migrationBuilder.CreateIndex(
                name: "IX_VoucherInfluencers_VoucherId",
                table: "VoucherInfluencers",
                column: "VoucherId");

            migrationBuilder.AddForeignKey(
                name: "FK_VoucherInfluencers_ApplicationUsers_InfluencerId",
                table: "VoucherInfluencers",
                column: "InfluencerId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VoucherInfluencers_Voucher_VoucherId",
                table: "VoucherInfluencers",
                column: "VoucherId",
                principalTable: "Voucher",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VoucherInfluencers_ApplicationUsers_InfluencerId",
                table: "VoucherInfluencers");

            migrationBuilder.DropForeignKey(
                name: "FK_VoucherInfluencers_Voucher_VoucherId",
                table: "VoucherInfluencers");

            migrationBuilder.DropIndex(
                name: "IX_VoucherInfluencers_InfluencerId",
                table: "VoucherInfluencers");

            migrationBuilder.DropIndex(
                name: "IX_VoucherInfluencers_VoucherId",
                table: "VoucherInfluencers");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Products");

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
    }
}
