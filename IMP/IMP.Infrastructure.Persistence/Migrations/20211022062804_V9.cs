using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IMP.Infrastructure.Persistence.Migrations
{
    public partial class V9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Condition",
                table: "Campaigns");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Campaigns",
                newName: "Openning");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "Campaigns",
                newName: "Evaluating");

            migrationBuilder.AlterColumn<double>(
                name: "FontSize",
                table: "Pages",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "Announcing",
                table: "Campaigns",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Applying",
                table: "Campaigns",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Closeing",
                table: "Campaigns",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPrizePerVoucher",
                table: "Campaigns",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "PrizeMoney",
                table: "Campaigns",
                type: "decimal(18,6)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "CampaignConditions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AgeFrom = table.Column<int>(type: "int", nullable: true),
                    AgeTo = table.Column<int>(type: "int", nullable: true),
                    UnlimitedAge = table.Column<bool>(type: "bit", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: true),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    Interests = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Jobs = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ChildStatus = table.Column<bool>(type: "bit", nullable: true),
                    MaritalStatus = table.Column<bool>(type: "bit", nullable: true),
                    CampaignId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampaignConditions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CampaignConditions_Campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaigns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CampaignConditions_CampaignId",
                table: "CampaignConditions",
                column: "CampaignId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CampaignConditions");

            migrationBuilder.DropColumn(
                name: "Announcing",
                table: "Campaigns");

            migrationBuilder.DropColumn(
                name: "Applying",
                table: "Campaigns");

            migrationBuilder.DropColumn(
                name: "Closeing",
                table: "Campaigns");

            migrationBuilder.DropColumn(
                name: "IsPrizePerVoucher",
                table: "Campaigns");

            migrationBuilder.DropColumn(
                name: "PrizeMoney",
                table: "Campaigns");

            migrationBuilder.RenameColumn(
                name: "Openning",
                table: "Campaigns",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "Evaluating",
                table: "Campaigns",
                newName: "EndDate");

            migrationBuilder.AlterColumn<int>(
                name: "FontSize",
                table: "Pages",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<string>(
                name: "Condition",
                table: "Campaigns",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);
        }
    }
}
