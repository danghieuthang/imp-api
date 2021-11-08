using Microsoft.EntityFrameworkCore.Migrations;

namespace IMP.Infrastructure.Persistence.Migrations
{
    public partial class V21 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ApplyOncePerCustomer",
                table: "Voucher",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ApplyOncePerOrder",
                table: "Voucher",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountValue",
                table: "Voucher",
                type: "decimal(18,6)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "DiscountValueType",
                table: "Voucher",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinCheckoutItemsQuantity",
                table: "Voucher",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "OnlyforStaff",
                table: "Voucher",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "QuantityUsed",
                table: "Voucher",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VoucherType",
                table: "Voucher",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplyOncePerCustomer",
                table: "Voucher");

            migrationBuilder.DropColumn(
                name: "ApplyOncePerOrder",
                table: "Voucher");

            migrationBuilder.DropColumn(
                name: "DiscountValue",
                table: "Voucher");

            migrationBuilder.DropColumn(
                name: "DiscountValueType",
                table: "Voucher");

            migrationBuilder.DropColumn(
                name: "MinCheckoutItemsQuantity",
                table: "Voucher");

            migrationBuilder.DropColumn(
                name: "OnlyforStaff",
                table: "Voucher");

            migrationBuilder.DropColumn(
                name: "QuantityUsed",
                table: "Voucher");

            migrationBuilder.DropColumn(
                name: "VoucherType",
                table: "Voucher");
        }
    }
}
