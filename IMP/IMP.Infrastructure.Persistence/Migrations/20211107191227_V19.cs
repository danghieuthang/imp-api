using Microsoft.EntityFrameworkCore.Migrations;

namespace IMP.Infrastructure.Persistence.Migrations
{
    public partial class V19 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VoucherCodeApplicationUser_ApplicationUsers_ApplicationUserId",
                table: "VoucherCodeApplicationUser");

            migrationBuilder.DropForeignKey(
                name: "FK_VoucherCodeApplicationUser_VoucherCodes_VoucherCodeId",
                table: "VoucherCodeApplicationUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VoucherCodeApplicationUser",
                table: "VoucherCodeApplicationUser");

            migrationBuilder.RenameTable(
                name: "VoucherCodeApplicationUser",
                newName: "VoucherCodeApplicationUsers");

            migrationBuilder.RenameIndex(
                name: "IX_VoucherCodeApplicationUser_VoucherCodeId",
                table: "VoucherCodeApplicationUsers",
                newName: "IX_VoucherCodeApplicationUsers_VoucherCodeId");

            migrationBuilder.RenameIndex(
                name: "IX_VoucherCodeApplicationUser_ApplicationUserId",
                table: "VoucherCodeApplicationUsers",
                newName: "IX_VoucherCodeApplicationUsers_ApplicationUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VoucherCodeApplicationUsers",
                table: "VoucherCodeApplicationUsers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VoucherCodeApplicationUsers_ApplicationUsers_ApplicationUserId",
                table: "VoucherCodeApplicationUsers",
                column: "ApplicationUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VoucherCodeApplicationUsers_VoucherCodes_VoucherCodeId",
                table: "VoucherCodeApplicationUsers",
                column: "VoucherCodeId",
                principalTable: "VoucherCodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VoucherCodeApplicationUsers_ApplicationUsers_ApplicationUserId",
                table: "VoucherCodeApplicationUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_VoucherCodeApplicationUsers_VoucherCodes_VoucherCodeId",
                table: "VoucherCodeApplicationUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VoucherCodeApplicationUsers",
                table: "VoucherCodeApplicationUsers");

            migrationBuilder.RenameTable(
                name: "VoucherCodeApplicationUsers",
                newName: "VoucherCodeApplicationUser");

            migrationBuilder.RenameIndex(
                name: "IX_VoucherCodeApplicationUsers_VoucherCodeId",
                table: "VoucherCodeApplicationUser",
                newName: "IX_VoucherCodeApplicationUser_VoucherCodeId");

            migrationBuilder.RenameIndex(
                name: "IX_VoucherCodeApplicationUsers_ApplicationUserId",
                table: "VoucherCodeApplicationUser",
                newName: "IX_VoucherCodeApplicationUser_ApplicationUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VoucherCodeApplicationUser",
                table: "VoucherCodeApplicationUser",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VoucherCodeApplicationUser_ApplicationUsers_ApplicationUserId",
                table: "VoucherCodeApplicationUser",
                column: "ApplicationUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VoucherCodeApplicationUser_VoucherCodes_VoucherCodeId",
                table: "VoucherCodeApplicationUser",
                column: "VoucherCodeId",
                principalTable: "VoucherCodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
