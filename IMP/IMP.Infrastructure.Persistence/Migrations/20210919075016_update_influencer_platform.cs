using Microsoft.EntityFrameworkCore.Migrations;

namespace IMP.Infrastructure.Persistence.Migrations
{
    public partial class update_influencer_platform : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Avatar",
                table: "InfluencerPlatforms",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Hashtag",
                table: "InfluencerPlatforms",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsVerified",
                table: "InfluencerPlatforms",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "InfluencerPlatforms");

            migrationBuilder.DropColumn(
                name: "Hashtag",
                table: "InfluencerPlatforms");

            migrationBuilder.DropColumn(
                name: "IsVerified",
                table: "InfluencerPlatforms");
        }
    }
}
