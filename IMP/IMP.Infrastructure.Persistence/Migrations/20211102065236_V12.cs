using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IMP.Infrastructure.Persistence.Migrations
{
    public partial class V12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Voucher_Campaigns_CampaignId",
                table: "Voucher");

            migrationBuilder.DropTable(
                name: "CampaignConditions");

            migrationBuilder.DropIndex(
                name: "IX_Voucher_CampaignId",
                table: "Voucher");

            migrationBuilder.DropColumn(
                name: "CampaignId",
                table: "Voucher");

            migrationBuilder.DropColumn(
                name: "IsPrizePerVoucher",
                table: "Campaigns");

            migrationBuilder.DropColumn(
                name: "MaxInfluencer",
                table: "Campaigns");

            migrationBuilder.DropColumn(
                name: "PrizeMoney",
                table: "Campaigns");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "CampaignActivities");

            migrationBuilder.RenameColumn(
                name: "QuantityUsed",
                table: "Voucher",
                newName: "BrandId");

            migrationBuilder.RenameColumn(
                name: "Reward",
                table: "Campaigns",
                newName: "SampleContent");

            migrationBuilder.RenameColumn(
                name: "ReferralWebsite",
                table: "Campaigns",
                newName: "Website");

            migrationBuilder.RenameColumn(
                name: "Closeing",
                table: "Campaigns",
                newName: "Closed");

            migrationBuilder.AddColumn<int>(
                name: "CampaignId",
                table: "VoucherCodes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Score",
                table: "RankLevels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Score",
                table: "Rankings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "PlatformId",
                table: "Campaigns",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Keywords",
                table: "Campaigns",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CampaignTypeId",
                table: "Campaigns",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "Advertising",
                table: "Campaigns",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Fanpage",
                table: "Campaigns",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Hashtags",
                table: "Campaigns",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QA",
                table: "Campaigns",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "CampaignActivities",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HowToDo",
                table: "CampaignActivities",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ActivityResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    CampaignActivityId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityResults_CampaignActivities_CampaignActivityId",
                        column: x => x.CampaignActivityId,
                        principalTable: "CampaignActivities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CampaignVouchers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CampaignId = table.Column<int>(type: "int", nullable: false),
                    VoucherId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    QuantityUsed = table.Column<int>(type: "int", nullable: false),
                    QuantityForInfluencer = table.Column<int>(type: "int", nullable: true),
                    PercentForInfluencer = table.Column<int>(type: "int", nullable: false),
                    PercentForIMP = table.Column<int>(type: "int", nullable: false),
                    IsDefaultReward = table.Column<bool>(type: "bit", nullable: false),
                    IsBestInfluencerReward = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampaignVouchers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CampaignVouchers_Campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaigns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CampaignVouchers_Voucher_VoucherId",
                        column: x => x.VoucherId,
                        principalTable: "Voucher",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InfluencerConfigurations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlatformId = table.Column<int>(type: "int", nullable: false),
                    NumberOfInfluencer = table.Column<int>(type: "int", nullable: false),
                    LevelId = table.Column<int>(type: "int", nullable: false),
                    AgeFrom = table.Column<int>(type: "int", nullable: true),
                    AgeTo = table.Column<int>(type: "int", nullable: true),
                    UnlimitedAge = table.Column<bool>(type: "bit", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Interests = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Jobs = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ChildStatus = table.Column<bool>(type: "bit", nullable: true),
                    MaritalStatus = table.Column<bool>(type: "bit", nullable: true),
                    Pregnant = table.Column<bool>(type: "bit", nullable: true),
                    OtherCondition = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CampaignId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InfluencerConfigurations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InfluencerConfigurations_Campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaigns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InfluencerConfigurations_Platforms_PlatformId",
                        column: x => x.PlatformId,
                        principalTable: "Platforms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InfluencerConfigurations_RankLevels_LevelId",
                        column: x => x.LevelId,
                        principalTable: "RankLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    IsReward = table.Column<bool>(type: "bit", nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CampaignId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaigns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TargetCustomerConfigurations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AgeFrom = table.Column<int>(type: "int", nullable: true),
                    AgeTo = table.Column<int>(type: "int", nullable: true),
                    UnlimitedAge = table.Column<bool>(type: "bit", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Interests = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Jobs = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ChildStatus = table.Column<bool>(type: "bit", nullable: true),
                    MaritalStatus = table.Column<bool>(type: "bit", nullable: true),
                    Pregnant = table.Column<bool>(type: "bit", nullable: true),
                    Purpose = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CampaignId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TargetCustomerConfigurations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TargetCustomerConfigurations_Campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaigns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CampaignRewards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CampaignId = table.Column<int>(type: "int", nullable: false),
                    IsDefaultReward = table.Column<bool>(type: "bit", nullable: false),
                    CampaignVoucherId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampaignRewards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CampaignRewards_Campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaigns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CampaignRewards_CampaignVouchers_CampaignVoucherId",
                        column: x => x.CampaignVoucherId,
                        principalTable: "CampaignVouchers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InfluencerConfigurationLocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InfluencerConfigurationId = table.Column<int>(type: "int", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InfluencerConfigurationLocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InfluencerConfigurationLocations_InfluencerConfigurations_InfluencerConfigurationId",
                        column: x => x.InfluencerConfigurationId,
                        principalTable: "InfluencerConfigurations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InfluencerConfigurationLocations_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TargetConfigurationLocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TargetConfigurationId = table.Column<int>(type: "int", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TargetConfigurationLocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TargetConfigurationLocations_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TargetConfigurationLocations_TargetCustomerConfigurations_TargetConfigurationId",
                        column: x => x.TargetConfigurationId,
                        principalTable: "TargetCustomerConfigurations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VoucherCodes_CampaignId",
                table: "VoucherCodes",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_Voucher_BrandId",
                table: "Voucher",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityResults_CampaignActivityId",
                table: "ActivityResults",
                column: "CampaignActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignRewards_CampaignId",
                table: "CampaignRewards",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignRewards_CampaignVoucherId",
                table: "CampaignRewards",
                column: "CampaignVoucherId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignVouchers_CampaignId",
                table: "CampaignVouchers",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignVouchers_VoucherId",
                table: "CampaignVouchers",
                column: "VoucherId");

            migrationBuilder.CreateIndex(
                name: "IX_InfluencerConfigurationLocations_InfluencerConfigurationId",
                table: "InfluencerConfigurationLocations",
                column: "InfluencerConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_InfluencerConfigurationLocations_LocationId",
                table: "InfluencerConfigurationLocations",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_InfluencerConfigurations_CampaignId",
                table: "InfluencerConfigurations",
                column: "CampaignId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InfluencerConfigurations_LevelId",
                table: "InfluencerConfigurations",
                column: "LevelId");

            migrationBuilder.CreateIndex(
                name: "IX_InfluencerConfigurations_PlatformId",
                table: "InfluencerConfigurations",
                column: "PlatformId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CampaignId",
                table: "Products",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_TargetConfigurationLocations_LocationId",
                table: "TargetConfigurationLocations",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_TargetConfigurationLocations_TargetConfigurationId",
                table: "TargetConfigurationLocations",
                column: "TargetConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_TargetCustomerConfigurations_CampaignId",
                table: "TargetCustomerConfigurations",
                column: "CampaignId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Voucher_Brands_BrandId",
                table: "Voucher",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VoucherCodes_Campaigns_CampaignId",
                table: "VoucherCodes",
                column: "CampaignId",
                principalTable: "Campaigns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Voucher_Brands_BrandId",
                table: "Voucher");

            migrationBuilder.DropForeignKey(
                name: "FK_VoucherCodes_Campaigns_CampaignId",
                table: "VoucherCodes");

            migrationBuilder.DropTable(
                name: "ActivityResults");

            migrationBuilder.DropTable(
                name: "CampaignRewards");

            migrationBuilder.DropTable(
                name: "InfluencerConfigurationLocations");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "TargetConfigurationLocations");

            migrationBuilder.DropTable(
                name: "CampaignVouchers");

            migrationBuilder.DropTable(
                name: "InfluencerConfigurations");

            migrationBuilder.DropTable(
                name: "TargetCustomerConfigurations");

            migrationBuilder.DropIndex(
                name: "IX_VoucherCodes_CampaignId",
                table: "VoucherCodes");

            migrationBuilder.DropIndex(
                name: "IX_Voucher_BrandId",
                table: "Voucher");

            migrationBuilder.DropColumn(
                name: "CampaignId",
                table: "VoucherCodes");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "RankLevels");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "Rankings");

            migrationBuilder.DropColumn(
                name: "Advertising",
                table: "Campaigns");

            migrationBuilder.DropColumn(
                name: "Fanpage",
                table: "Campaigns");

            migrationBuilder.DropColumn(
                name: "Hashtags",
                table: "Campaigns");

            migrationBuilder.DropColumn(
                name: "QA",
                table: "Campaigns");

            migrationBuilder.DropColumn(
                name: "HowToDo",
                table: "CampaignActivities");

            migrationBuilder.RenameColumn(
                name: "BrandId",
                table: "Voucher",
                newName: "QuantityUsed");

            migrationBuilder.RenameColumn(
                name: "Website",
                table: "Campaigns",
                newName: "ReferralWebsite");

            migrationBuilder.RenameColumn(
                name: "SampleContent",
                table: "Campaigns",
                newName: "Reward");

            migrationBuilder.RenameColumn(
                name: "Closed",
                table: "Campaigns",
                newName: "Closeing");

            migrationBuilder.AddColumn<int>(
                name: "CampaignId",
                table: "Voucher",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "PlatformId",
                table: "Campaigns",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Keywords",
                table: "Campaigns",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CampaignTypeId",
                table: "Campaigns",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPrizePerVoucher",
                table: "Campaigns",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "MaxInfluencer",
                table: "Campaigns",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "PrizeMoney",
                table: "Campaigns",
                type: "decimal(18,6)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "CampaignActivities",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Position",
                table: "CampaignActivities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CampaignConditions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AgeFrom = table.Column<int>(type: "int", nullable: true),
                    AgeTo = table.Column<int>(type: "int", nullable: true),
                    CampaignId = table.Column<int>(type: "int", nullable: false),
                    ChildStatus = table.Column<bool>(type: "bit", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: true),
                    Interests = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Jobs = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    MaritalStatus = table.Column<bool>(type: "bit", nullable: true),
                    UnlimitedAge = table.Column<bool>(type: "bit", nullable: false)
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
                name: "IX_Voucher_CampaignId",
                table: "Voucher",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignConditions_CampaignId",
                table: "CampaignConditions",
                column: "CampaignId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Voucher_Campaigns_CampaignId",
                table: "Voucher",
                column: "CampaignId",
                principalTable: "Campaigns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
