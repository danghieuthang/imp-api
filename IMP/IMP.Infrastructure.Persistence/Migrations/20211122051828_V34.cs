using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IMP.Infrastructure.Persistence.Migrations
{
    public partial class V34 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VoucherCodes_ApplicationUsers_InfluencerId",
                table: "VoucherCodes");

            migrationBuilder.RenameColumn(
                name: "InfluencerId",
                table: "VoucherCodes",
                newName: "CampaignMemberId");

            migrationBuilder.RenameIndex(
                name: "IX_VoucherCodes_InfluencerId",
                table: "VoucherCodes",
                newName: "IX_VoucherCodes_CampaignMemberId");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "MemberActivities",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "ApplicationUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Message = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_ApplicationUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_ApplicationUserId",
                table: "Notifications",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_VoucherCodes_CampaignMembers_CampaignMemberId",
                table: "VoucherCodes",
                column: "CampaignMemberId",
                principalTable: "CampaignMembers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VoucherCodes_CampaignMembers_CampaignMemberId",
                table: "VoucherCodes");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "ApplicationUsers");

            migrationBuilder.RenameColumn(
                name: "CampaignMemberId",
                table: "VoucherCodes",
                newName: "InfluencerId");

            migrationBuilder.RenameIndex(
                name: "IX_VoucherCodes_CampaignMemberId",
                table: "VoucherCodes",
                newName: "IX_VoucherCodes_InfluencerId");

            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "MemberActivities",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_VoucherCodes_ApplicationUsers_InfluencerId",
                table: "VoucherCodes",
                column: "InfluencerId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
