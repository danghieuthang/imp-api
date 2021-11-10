using Microsoft.EntityFrameworkCore.Migrations;

namespace IMP.Infrastructure.Persistence.Migrations
{
    public partial class V25 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplyOncePerCustomer",
                table: "Voucher");

            migrationBuilder.DropColumn(
                name: "MinCheckoutItemsQuantity",
                table: "Voucher");

            migrationBuilder.DropColumn(
                name: "VoucherType",
                table: "Voucher");

            migrationBuilder.RenameColumn(
                name: "OnlyforStaff",
                table: "Voucher",
                newName: "OnlyforInfluencer");

            migrationBuilder.RenameColumn(
                name: "ApplyOncePerOrder",
                table: "Voucher",
                newName: "OnlyforCustomer");

            migrationBuilder.AddColumn<string>(
                name: "DiscountProducts",
                table: "Voucher",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscountProducts",
                table: "Voucher");

            migrationBuilder.RenameColumn(
                name: "OnlyforInfluencer",
                table: "Voucher",
                newName: "OnlyforStaff");

            migrationBuilder.RenameColumn(
                name: "OnlyforCustomer",
                table: "Voucher",
                newName: "ApplyOncePerOrder");

            migrationBuilder.AddColumn<bool>(
                name: "ApplyOncePerCustomer",
                table: "Voucher",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "MinCheckoutItemsQuantity",
                table: "Voucher",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VoucherType",
                table: "Voucher",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
