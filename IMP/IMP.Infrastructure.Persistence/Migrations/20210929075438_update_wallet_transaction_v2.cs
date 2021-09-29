using Microsoft.EntityFrameworkCore.Migrations;

namespace IMP.Infrastructure.Persistence.Migrations
{
    public partial class update_wallet_transaction_v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUsers_PaymentInfors_PaymentInforId",
                table: "ApplicationUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUsers_Wallets_WalletId",
                table: "ApplicationUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_WalletTransactions_Wallets_WalletId",
                table: "WalletTransactions");

            migrationBuilder.DropIndex(
                name: "IX_WalletTransactions_WalletId",
                table: "WalletTransactions");

            migrationBuilder.DropIndex(
                name: "IX_Wallets_ApplicationUserId",
                table: "Wallets");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationUsers_PaymentInforId",
                table: "ApplicationUsers");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationUsers_WalletId",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "WalletId",
                table: "WalletTransactions");

            migrationBuilder.DropColumn(
                name: "PaymentInforId",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "WalletId",
                table: "ApplicationUsers");

            migrationBuilder.RenameColumn(
                name: "TransactionNo",
                table: "WalletTransactions",
                newName: "VnpTransactionNo");

            migrationBuilder.AlterColumn<string>(
                name: "TransactionInfo",
                table: "WalletTransactions",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "WalletTransactions",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReceiverId",
                table: "WalletTransactions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SenderId",
                table: "WalletTransactions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WalletFromId",
                table: "WalletTransactions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WalletToId",
                table: "WalletTransactions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WalletTransactions_ReceiverId",
                table: "WalletTransactions",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_WalletTransactions_SenderId",
                table: "WalletTransactions",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_WalletTransactions_WalletFromId",
                table: "WalletTransactions",
                column: "WalletFromId");

            migrationBuilder.CreateIndex(
                name: "IX_WalletTransactions_WalletToId",
                table: "WalletTransactions",
                column: "WalletToId");

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_ApplicationUserId",
                table: "Wallets",
                column: "ApplicationUserId",
                unique: true,
                filter: "[ApplicationUserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentInfors_UserId",
                table: "PaymentInfors",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentInfors_ApplicationUsers_UserId",
                table: "PaymentInfors",
                column: "UserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WalletTransactions_ApplicationUsers_ReceiverId",
                table: "WalletTransactions",
                column: "ReceiverId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WalletTransactions_ApplicationUsers_SenderId",
                table: "WalletTransactions",
                column: "SenderId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WalletTransactions_Wallets_WalletFromId",
                table: "WalletTransactions",
                column: "WalletFromId",
                principalTable: "Wallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WalletTransactions_Wallets_WalletToId",
                table: "WalletTransactions",
                column: "WalletToId",
                principalTable: "Wallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentInfors_ApplicationUsers_UserId",
                table: "PaymentInfors");

            migrationBuilder.DropForeignKey(
                name: "FK_WalletTransactions_ApplicationUsers_ReceiverId",
                table: "WalletTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_WalletTransactions_ApplicationUsers_SenderId",
                table: "WalletTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_WalletTransactions_Wallets_WalletFromId",
                table: "WalletTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_WalletTransactions_Wallets_WalletToId",
                table: "WalletTransactions");

            migrationBuilder.DropIndex(
                name: "IX_WalletTransactions_ReceiverId",
                table: "WalletTransactions");

            migrationBuilder.DropIndex(
                name: "IX_WalletTransactions_SenderId",
                table: "WalletTransactions");

            migrationBuilder.DropIndex(
                name: "IX_WalletTransactions_WalletFromId",
                table: "WalletTransactions");

            migrationBuilder.DropIndex(
                name: "IX_WalletTransactions_WalletToId",
                table: "WalletTransactions");

            migrationBuilder.DropIndex(
                name: "IX_Wallets_ApplicationUserId",
                table: "Wallets");

            migrationBuilder.DropIndex(
                name: "IX_PaymentInfors_UserId",
                table: "PaymentInfors");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "WalletTransactions");

            migrationBuilder.DropColumn(
                name: "ReceiverId",
                table: "WalletTransactions");

            migrationBuilder.DropColumn(
                name: "SenderId",
                table: "WalletTransactions");

            migrationBuilder.DropColumn(
                name: "WalletFromId",
                table: "WalletTransactions");

            migrationBuilder.DropColumn(
                name: "WalletToId",
                table: "WalletTransactions");

            migrationBuilder.RenameColumn(
                name: "VnpTransactionNo",
                table: "WalletTransactions",
                newName: "TransactionNo");

            migrationBuilder.AlterColumn<string>(
                name: "TransactionInfo",
                table: "WalletTransactions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WalletId",
                table: "WalletTransactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PaymentInforId",
                table: "ApplicationUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WalletId",
                table: "ApplicationUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_WalletTransactions_WalletId",
                table: "WalletTransactions",
                column: "WalletId");

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_ApplicationUserId",
                table: "Wallets",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUsers_PaymentInforId",
                table: "ApplicationUsers",
                column: "PaymentInforId",
                unique: true,
                filter: "[PaymentInforId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUsers_WalletId",
                table: "ApplicationUsers",
                column: "WalletId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUsers_PaymentInfors_PaymentInforId",
                table: "ApplicationUsers",
                column: "PaymentInforId",
                principalTable: "PaymentInfors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUsers_Wallets_WalletId",
                table: "ApplicationUsers",
                column: "WalletId",
                principalTable: "Wallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WalletTransactions_Wallets_WalletId",
                table: "WalletTransactions",
                column: "WalletId",
                principalTable: "Wallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
