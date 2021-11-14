using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IMP.Infrastructure.Persistence.Migrations
{
    public partial class V27 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MemberActivities_ActivityTypes_ActivityTypeId",
                table: "MemberActivities");

            migrationBuilder.DropForeignKey(
                name: "FK_MemberActivities_ApplicationUsers_InfluencerId",
                table: "MemberActivities");

            migrationBuilder.DropTable(
                name: "ActivityResults");

            migrationBuilder.DropTable(
                name: "Profile");

            migrationBuilder.DropIndex(
                name: "IX_MemberActivities_ActivityTypeId",
                table: "MemberActivities");

            migrationBuilder.DropIndex(
                name: "IX_MemberActivities_InfluencerId",
                table: "MemberActivities");

            migrationBuilder.DropColumn(
                name: "ActivityTypeId",
                table: "MemberActivities");

            migrationBuilder.DropColumn(
                name: "InfluencerId",
                table: "MemberActivities");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "MemberActivities");

            migrationBuilder.DropColumn(
                name: "Progress",
                table: "MemberActivities");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Evidences");

            migrationBuilder.DropColumn(
                name: "LinkUrl",
                table: "Evidences");

            migrationBuilder.DropColumn(
                name: "VideoUrl",
                table: "Evidences");

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "MemberActivities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "EvidenceTypeId",
                table: "Evidences",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Evidences",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ActivityProgess",
                table: "CampaignMembers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ApproveById",
                table: "CampaignMembers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ApprovedById",
                table: "CampaignMembers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedDate",
                table: "CampaignMembers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedDate",
                table: "CampaignMembers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "Money",
                table: "CampaignMembers",
                type: "decimal(18,6)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "EvidenceTypeId",
                table: "CampaignActivities",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResultName",
                table: "CampaignActivities",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ActivityComments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberActivityId = table.Column<int>(type: "int", nullable: false),
                    ApplicationUserId = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityComments_MemberActivities_MemberActivityId",
                        column: x => x.MemberActivityId,
                        principalTable: "MemberActivities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EvidenceTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvidenceTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Evidences_EvidenceTypeId",
                table: "Evidences",
                column: "EvidenceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignMembers_ApprovedById",
                table: "CampaignMembers",
                column: "ApprovedById");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignActivities_EvidenceTypeId",
                table: "CampaignActivities",
                column: "EvidenceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityComments_MemberActivityId",
                table: "ActivityComments",
                column: "MemberActivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_CampaignActivities_EvidenceTypes_EvidenceTypeId",
                table: "CampaignActivities",
                column: "EvidenceTypeId",
                principalTable: "EvidenceTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CampaignMembers_ApplicationUsers_ApprovedById",
                table: "CampaignMembers",
                column: "ApprovedById",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Evidences_EvidenceTypes_EvidenceTypeId",
                table: "Evidences",
                column: "EvidenceTypeId",
                principalTable: "EvidenceTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CampaignActivities_EvidenceTypes_EvidenceTypeId",
                table: "CampaignActivities");

            migrationBuilder.DropForeignKey(
                name: "FK_CampaignMembers_ApplicationUsers_ApprovedById",
                table: "CampaignMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_Evidences_EvidenceTypes_EvidenceTypeId",
                table: "Evidences");

            migrationBuilder.DropTable(
                name: "ActivityComments");

            migrationBuilder.DropTable(
                name: "EvidenceTypes");

            migrationBuilder.DropIndex(
                name: "IX_Evidences_EvidenceTypeId",
                table: "Evidences");

            migrationBuilder.DropIndex(
                name: "IX_CampaignMembers_ApprovedById",
                table: "CampaignMembers");

            migrationBuilder.DropIndex(
                name: "IX_CampaignActivities_EvidenceTypeId",
                table: "CampaignActivities");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "MemberActivities");

            migrationBuilder.DropColumn(
                name: "EvidenceTypeId",
                table: "Evidences");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "Evidences");

            migrationBuilder.DropColumn(
                name: "ActivityProgess",
                table: "CampaignMembers");

            migrationBuilder.DropColumn(
                name: "ApproveById",
                table: "CampaignMembers");

            migrationBuilder.DropColumn(
                name: "ApprovedById",
                table: "CampaignMembers");

            migrationBuilder.DropColumn(
                name: "ApprovedDate",
                table: "CampaignMembers");

            migrationBuilder.DropColumn(
                name: "CompletedDate",
                table: "CampaignMembers");

            migrationBuilder.DropColumn(
                name: "Money",
                table: "CampaignMembers");

            migrationBuilder.DropColumn(
                name: "EvidenceTypeId",
                table: "CampaignActivities");

            migrationBuilder.DropColumn(
                name: "ResultName",
                table: "CampaignActivities");

            migrationBuilder.AddColumn<int>(
                name: "ActivityTypeId",
                table: "MemberActivities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "InfluencerId",
                table: "MemberActivities",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Position",
                table: "MemberActivities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<float>(
                name: "Progress",
                table: "MemberActivities",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Evidences",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkUrl",
                table: "Evidences",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VideoUrl",
                table: "Evidences",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ActivityResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CampaignActivityId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false)
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
                name: "Profile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    AvatarUrl = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NickName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Profile_ApplicationUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MemberActivities_ActivityTypeId",
                table: "MemberActivities",
                column: "ActivityTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberActivities_InfluencerId",
                table: "MemberActivities",
                column: "InfluencerId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityResults_CampaignActivityId",
                table: "ActivityResults",
                column: "CampaignActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_Profile_UserId",
                table: "Profile",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_MemberActivities_ActivityTypes_ActivityTypeId",
                table: "MemberActivities",
                column: "ActivityTypeId",
                principalTable: "ActivityTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MemberActivities_ApplicationUsers_InfluencerId",
                table: "MemberActivities",
                column: "InfluencerId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
