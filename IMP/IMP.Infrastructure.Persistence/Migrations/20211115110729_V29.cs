using Microsoft.EntityFrameworkCore.Migrations;

namespace IMP.Infrastructure.Persistence.Migrations
{
    public partial class V29 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovelById",
                table: "Campaigns");

            migrationBuilder.DropColumn(
                name: "ApproveById",
                table: "CampaignMembers");

            migrationBuilder.AddColumn<int>(
                name: "ApprovedById",
                table: "Campaigns",
                type: "int",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovedById",
                table: "Campaigns");

            migrationBuilder.AddColumn<int>(
                name: "ApprovelById",
                table: "Campaigns",
                type: "int",
                maxLength: 50,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ApproveById",
                table: "CampaignMembers",
                type: "int",
                nullable: true);
        }
    }
}
