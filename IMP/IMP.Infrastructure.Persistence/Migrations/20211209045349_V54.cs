using Microsoft.EntityFrameworkCore.Migrations;

namespace IMP.Infrastructure.Persistence.Migrations
{
    public partial class V54 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "EarningMoney",
                table: "VoucherTransactions",
                type: "decimal(18,6)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EarningMoney",
                table: "VoucherTransactions");
        }
    }
}
