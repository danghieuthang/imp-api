using Microsoft.EntityFrameworkCore.Migrations;

namespace IMP.Infrastructure.Persistence.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "VoucherCodes",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VoucherId",
                table: "VoucherCodes",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VoucherCodes_VoucherId",
                table: "VoucherCodes",
                column: "VoucherId");

            migrationBuilder.AddForeignKey(
                name: "FK_VoucherCodes_Voucher_VoucherId",
                table: "VoucherCodes",
                column: "VoucherId",
                principalTable: "Voucher",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VoucherCodes_Voucher_VoucherId",
                table: "VoucherCodes");

            migrationBuilder.DropIndex(
                name: "IX_VoucherCodes_VoucherId",
                table: "VoucherCodes");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "VoucherCodes");

            migrationBuilder.DropColumn(
                name: "VoucherId",
                table: "VoucherCodes");
        }
    }
}
