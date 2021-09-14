using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IMP.Infrastructure.Persistence.Migrations
{
    public partial class Update_profile_add_bank : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ApplicationUsers_PaymentInforId",
                table: "ApplicationUsers");

            migrationBuilder.AddColumn<string>(
                name: "AccountName",
                table: "PaymentInfors",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AccountNumber",
                table: "PaymentInfors",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BankId",
                table: "PaymentInfors",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "InfluencerPlatforms",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Follower",
                table: "InfluencerPlatforms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "PaymentInforId",
                table: "ApplicationUsers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "ChildStatus",
                table: "ApplicationUsers",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ApplicationUsers",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Job",
                table: "ApplicationUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "MaritalStatus",
                table: "ApplicationUsers",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Pet",
                table: "ApplicationUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Banks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banks", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentInfors_BankId",
                table: "PaymentInfors",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUsers_PaymentInforId",
                table: "ApplicationUsers",
                column: "PaymentInforId",
                unique: true,
                filter: "[PaymentInforId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentInfors_Banks_BankId",
                table: "PaymentInfors",
                column: "BankId",
                principalTable: "Banks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentInfors_Banks_BankId",
                table: "PaymentInfors");

            migrationBuilder.DropTable(
                name: "Banks");

            migrationBuilder.DropIndex(
                name: "IX_PaymentInfors_BankId",
                table: "PaymentInfors");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationUsers_PaymentInforId",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "AccountName",
                table: "PaymentInfors");

            migrationBuilder.DropColumn(
                name: "AccountNumber",
                table: "PaymentInfors");

            migrationBuilder.DropColumn(
                name: "BankId",
                table: "PaymentInfors");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "InfluencerPlatforms");

            migrationBuilder.DropColumn(
                name: "Follower",
                table: "InfluencerPlatforms");

            migrationBuilder.DropColumn(
                name: "ChildStatus",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "Job",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "MaritalStatus",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "Pet",
                table: "ApplicationUsers");

            migrationBuilder.AlterColumn<int>(
                name: "PaymentInforId",
                table: "ApplicationUsers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUsers_PaymentInforId",
                table: "ApplicationUsers",
                column: "PaymentInforId",
                unique: true);
        }
    }
}
