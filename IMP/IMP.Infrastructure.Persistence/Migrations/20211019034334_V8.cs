using Microsoft.EntityFrameworkCore.Migrations;

namespace IMP.Infrastructure.Persistence.Migrations
{
    public partial class V8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Campaigns_ApplicationUsers_BrandId",
                table: "Campaigns");

            migrationBuilder.RenameColumn(
                name: "UserQuantity",
                table: "Voucher",
                newName: "QuantityUsed");

            migrationBuilder.AlterColumn<int>(
                name: "VoucherId",
                table: "VoucherCodes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "InfluencerId",
                table: "VoucherCodes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Pages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "Campaigns",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_VoucherCodes_InfluencerId",
                table: "VoucherCodes",
                column: "InfluencerId");

            migrationBuilder.CreateIndex(
                name: "IX_Campaigns_CreatedById",
                table: "Campaigns",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Campaigns_ApplicationUsers_CreatedById",
                table: "Campaigns",
                column: "CreatedById",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Campaigns_Brands_BrandId",
                table: "Campaigns",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VoucherCodes_ApplicationUsers_InfluencerId",
                table: "VoucherCodes",
                column: "InfluencerId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Campaigns_ApplicationUsers_CreatedById",
                table: "Campaigns");

            migrationBuilder.DropForeignKey(
                name: "FK_Campaigns_Brands_BrandId",
                table: "Campaigns");

            migrationBuilder.DropForeignKey(
                name: "FK_VoucherCodes_ApplicationUsers_InfluencerId",
                table: "VoucherCodes");

            migrationBuilder.DropIndex(
                name: "IX_VoucherCodes_InfluencerId",
                table: "VoucherCodes");

            migrationBuilder.DropIndex(
                name: "IX_Campaigns_CreatedById",
                table: "Campaigns");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Campaigns");

            migrationBuilder.RenameColumn(
                name: "QuantityUsed",
                table: "Voucher",
                newName: "UserQuantity");

            migrationBuilder.AlterColumn<int>(
                name: "VoucherId",
                table: "VoucherCodes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "InfluencerId",
                table: "VoucherCodes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Campaigns_ApplicationUsers_BrandId",
                table: "Campaigns",
                column: "BrandId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
