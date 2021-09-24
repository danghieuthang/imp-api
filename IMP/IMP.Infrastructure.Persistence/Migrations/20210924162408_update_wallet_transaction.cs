using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IMP.Infrastructure.Persistence.Migrations
{
    public partial class update_wallet_transaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CampaignId",
                table: "VoucherCodes");

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "WalletTransactions",
                type: "decimal(18,6)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "BankId",
                table: "WalletTransactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BankTranNo",
                table: "WalletTransactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "PayDate",
                table: "WalletTransactions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "TransactionInfo",
                table: "WalletTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TransactionNo",
                table: "WalletTransactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TransactionStatus",
                table: "WalletTransactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TransactionType",
                table: "WalletTransactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ApplicationUserId",
                table: "Wallets",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Balance",
                table: "Wallets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_WalletTransactions_BankId",
                table: "WalletTransactions",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_ApplicationUserId",
                table: "Wallets",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Wallets_ApplicationUsers_ApplicationUserId",
                table: "Wallets",
                column: "ApplicationUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WalletTransactions_Banks_BankId",
                table: "WalletTransactions",
                column: "BankId",
                principalTable: "Banks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wallets_ApplicationUsers_ApplicationUserId",
                table: "Wallets");

            migrationBuilder.DropForeignKey(
                name: "FK_WalletTransactions_Banks_BankId",
                table: "WalletTransactions");

            migrationBuilder.DropIndex(
                name: "IX_WalletTransactions_BankId",
                table: "WalletTransactions");

            migrationBuilder.DropIndex(
                name: "IX_Wallets_ApplicationUserId",
                table: "Wallets");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "WalletTransactions");

            migrationBuilder.DropColumn(
                name: "BankId",
                table: "WalletTransactions");

            migrationBuilder.DropColumn(
                name: "BankTranNo",
                table: "WalletTransactions");

            migrationBuilder.DropColumn(
                name: "PayDate",
                table: "WalletTransactions");

            migrationBuilder.DropColumn(
                name: "TransactionInfo",
                table: "WalletTransactions");

            migrationBuilder.DropColumn(
                name: "TransactionNo",
                table: "WalletTransactions");

            migrationBuilder.DropColumn(
                name: "TransactionStatus",
                table: "WalletTransactions");

            migrationBuilder.DropColumn(
                name: "TransactionType",
                table: "WalletTransactions");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Wallets");

            migrationBuilder.DropColumn(
                name: "Balance",
                table: "Wallets");

            migrationBuilder.AddColumn<int>(
                name: "CampaignId",
                table: "VoucherCodes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
