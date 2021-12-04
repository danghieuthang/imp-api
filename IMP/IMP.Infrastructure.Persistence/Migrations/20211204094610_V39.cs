using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IMP.Infrastructure.Persistence.Migrations
{
    public partial class V39 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VoucherCodes_Campaigns_CampaignId",
                table: "VoucherCodes");

            migrationBuilder.DropIndex(
                name: "IX_VoucherCodes_CampaignId",
                table: "VoucherCodes");

            migrationBuilder.DropColumn(
                name: "CampaignId",
                table: "VoucherCodes");

            migrationBuilder.AddColumn<string>(
                name: "Order",
                table: "VoucherTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrderCode",
                table: "VoucherTransactions",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OrderCreated",
                table: "VoucherTransactions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "TotalDiscount",
                table: "VoucherTransactions",
                type: "decimal(18,6)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "VoucherTransactions",
                type: "decimal(18,6)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "VoucherTransactions");

            migrationBuilder.DropColumn(
                name: "OrderCode",
                table: "VoucherTransactions");

            migrationBuilder.DropColumn(
                name: "OrderCreated",
                table: "VoucherTransactions");

            migrationBuilder.DropColumn(
                name: "TotalDiscount",
                table: "VoucherTransactions");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "VoucherTransactions");

            migrationBuilder.AddColumn<int>(
                name: "CampaignId",
                table: "VoucherCodes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VoucherCodes_CampaignId",
                table: "VoucherCodes",
                column: "CampaignId");

            migrationBuilder.AddForeignKey(
                name: "FK_VoucherCodes_Campaigns_CampaignId",
                table: "VoucherCodes",
                column: "CampaignId",
                principalTable: "Campaigns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
