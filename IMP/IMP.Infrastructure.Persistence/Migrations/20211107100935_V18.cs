using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IMP.Infrastructure.Persistence.Migrations
{
    public partial class V18 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "VoucherCodes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QuantityUsed",
                table: "VoucherCodes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "VoucherCodeApplicationUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VoucherCodeId = table.Column<int>(type: "int", nullable: false),
                    ApplicationUserId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoucherCodeApplicationUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VoucherCodeApplicationUser_ApplicationUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VoucherCodeApplicationUser_VoucherCodes_VoucherCodeId",
                        column: x => x.VoucherCodeId,
                        principalTable: "VoucherCodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VoucherCodeApplicationUser_ApplicationUserId",
                table: "VoucherCodeApplicationUser",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_VoucherCodeApplicationUser_VoucherCodeId",
                table: "VoucherCodeApplicationUser",
                column: "VoucherCodeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VoucherCodeApplicationUser");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "VoucherCodes");

            migrationBuilder.DropColumn(
                name: "QuantityUsed",
                table: "VoucherCodes");
        }
    }
}
