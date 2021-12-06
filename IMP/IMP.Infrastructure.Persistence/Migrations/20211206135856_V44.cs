using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IMP.Infrastructure.Persistence.Migrations
{
    public partial class V44 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HoldTime",
                table: "VoucherCodes",
                newName: "Expired");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "HoldTime",
                table: "Voucher",
                type: "time",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Expired",
                table: "VoucherCodes",
                newName: "HoldTime");

            migrationBuilder.AlterColumn<DateTime>(
                name: "HoldTime",
                table: "Voucher",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "time",
                oldNullable: true);
        }
    }
}
