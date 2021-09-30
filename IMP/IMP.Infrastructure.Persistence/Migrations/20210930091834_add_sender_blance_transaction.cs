using Microsoft.EntityFrameworkCore.Migrations;

namespace IMP.Infrastructure.Persistence.Migrations
{
    public partial class add_sender_blance_transaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ReceiverBalance",
                table: "WalletTransactions",
                type: "decimal(18,6)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SenderBalance",
                table: "WalletTransactions",
                type: "decimal(18,6)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceiverBalance",
                table: "WalletTransactions");

            migrationBuilder.DropColumn(
                name: "SenderBalance",
                table: "WalletTransactions");
        }
    }
}
