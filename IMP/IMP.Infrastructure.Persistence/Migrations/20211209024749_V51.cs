using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IMP.Infrastructure.Persistence.Migrations
{
    public partial class V51 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderCode",
                table: "VoucherTransactions");

            migrationBuilder.DropColumn(
                name: "OrderCreated",
                table: "VoucherTransactions");

            migrationBuilder.RenameColumn(
                name: "TotalPrice",
                table: "VoucherTransactions",
                newName: "TotalProductAmount");

            migrationBuilder.AddColumn<int>(
                name: "CampaignId",
                table: "VoucherTransactions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductQuantity",
                table: "VoucherTransactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalOrderAmount",
                table: "VoucherTransactions",
                type: "decimal(18,6)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<double>(
                name: "BackgroundBrightness",
                table: "Pages",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CampaignId",
                table: "VoucherTransactions");

            migrationBuilder.DropColumn(
                name: "ProductQuantity",
                table: "VoucherTransactions");

            migrationBuilder.DropColumn(
                name: "TotalOrderAmount",
                table: "VoucherTransactions");

            migrationBuilder.DropColumn(
                name: "BackgroundBrightness",
                table: "Pages");

            migrationBuilder.RenameColumn(
                name: "TotalProductAmount",
                table: "VoucherTransactions",
                newName: "TotalPrice");

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
        }
    }
}
