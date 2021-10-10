using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IMP.Infrastructure.Persistence.Migrations
{
    public partial class Update_Bio : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlockCampaigns");

            migrationBuilder.DropTable(
                name: "BlockPlatforms");

            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "Blocks");

            migrationBuilder.DropColumn(
                name: "Bio",
                table: "Blocks");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Blocks");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Blocks");

            migrationBuilder.DropColumn(
                name: "Text",
                table: "Blocks");

            migrationBuilder.DropColumn(
                name: "TextArea",
                table: "Blocks");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Blocks");

            migrationBuilder.DropColumn(
                name: "VideoUrl",
                table: "Blocks");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Pages",
                newName: "TextColor");

            migrationBuilder.RenameColumn(
                name: "PositionPage",
                table: "Pages",
                newName: "FontSize");

            migrationBuilder.RenameColumn(
                name: "BackgroundPhoto",
                table: "Pages",
                newName: "FontFamily");

            migrationBuilder.RenameColumn(
                name: "IsActived",
                table: "Blocks",
                newName: "Enable");

            migrationBuilder.AddColumn<string>(
                name: "Background",
                table: "Pages",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BackgroundType",
                table: "Pages",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BioLink",
                table: "Pages",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PageId",
                table: "Blocks",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Variant",
                table: "Blocks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BlockItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BlockId = table.Column<int>(type: "int", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlockItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlockItem_Blocks_BlockId",
                        column: x => x.BlockId,
                        principalTable: "Blocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlockItem_BlockId",
                table: "BlockItem",
                column: "BlockId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlockItem");

            migrationBuilder.DropColumn(
                name: "Background",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "BackgroundType",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "BioLink",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "Variant",
                table: "Blocks");

            migrationBuilder.RenameColumn(
                name: "TextColor",
                table: "Pages",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "FontSize",
                table: "Pages",
                newName: "PositionPage");

            migrationBuilder.RenameColumn(
                name: "FontFamily",
                table: "Pages",
                newName: "BackgroundPhoto");

            migrationBuilder.RenameColumn(
                name: "Enable",
                table: "Blocks",
                newName: "IsActived");

            migrationBuilder.AlterColumn<int>(
                name: "PageId",
                table: "Blocks",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Avatar",
                table: "Blocks",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Bio",
                table: "Blocks",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Blocks",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Blocks",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Text",
                table: "Blocks",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TextArea",
                table: "Blocks",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Blocks",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VideoUrl",
                table: "Blocks",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BlockCampaigns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BlockId = table.Column<int>(type: "int", nullable: false),
                    CampaignId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActived = table.Column<bool>(type: "bit", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Position = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlockCampaigns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlockCampaigns_Blocks_BlockId",
                        column: x => x.BlockId,
                        principalTable: "Blocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BlockCampaigns_Campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaigns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BlockPlatforms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BlockId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InfluencerPlatformId = table.Column<int>(type: "int", nullable: false),
                    IsActived = table.Column<bool>(type: "bit", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Position = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlockPlatforms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlockPlatforms_Blocks_BlockId",
                        column: x => x.BlockId,
                        principalTable: "Blocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BlockPlatforms_InfluencerPlatforms_InfluencerPlatformId",
                        column: x => x.InfluencerPlatformId,
                        principalTable: "InfluencerPlatforms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlockCampaigns_BlockId",
                table: "BlockCampaigns",
                column: "BlockId");

            migrationBuilder.CreateIndex(
                name: "IX_BlockCampaigns_CampaignId",
                table: "BlockCampaigns",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_BlockPlatforms_BlockId",
                table: "BlockPlatforms",
                column: "BlockId");

            migrationBuilder.CreateIndex(
                name: "IX_BlockPlatforms_InfluencerPlatformId",
                table: "BlockPlatforms",
                column: "InfluencerPlatformId");
        }
    }
}
