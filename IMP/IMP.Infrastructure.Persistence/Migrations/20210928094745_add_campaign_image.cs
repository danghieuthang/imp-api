using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IMP.Infrastructure.Persistence.Migrations
{
    public partial class add_campaign_image : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReferalWebsite",
                table: "Campaigns",
                newName: "ReferralWebsite");

            migrationBuilder.RenameColumn(
                name: "AditionalInfomation",
                table: "Campaigns",
                newName: "AdditionalInfomation");

            migrationBuilder.CreateTable(
                name: "CampaignImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CampaignId = table.Column<int>(type: "int", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Position = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampaignImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CampaignImages_Campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaigns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CampaignImages_CampaignId",
                table: "CampaignImages",
                column: "CampaignId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CampaignImages");

            migrationBuilder.RenameColumn(
                name: "ReferralWebsite",
                table: "Campaigns",
                newName: "ReferalWebsite");

            migrationBuilder.RenameColumn(
                name: "AdditionalInfomation",
                table: "Campaigns",
                newName: "AditionalInfomation");
        }
    }
}
