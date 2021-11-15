using Microsoft.EntityFrameworkCore.Migrations;

namespace IMP.Infrastructure.Persistence.Migrations
{
    public partial class V28 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Evidences_MemberActivityId",
                table: "Evidences");

            migrationBuilder.AddColumn<int>(
                name: "ApprovelById",
                table: "Campaigns",
                type: "int",
                maxLength: 50,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Campaigns",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Evidences_MemberActivityId",
                table: "Evidences",
                column: "MemberActivityId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Evidences_MemberActivityId",
                table: "Evidences");

            migrationBuilder.DropColumn(
                name: "ApprovelById",
                table: "Campaigns");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "Campaigns");

            migrationBuilder.CreateIndex(
                name: "IX_Evidences_MemberActivityId",
                table: "Evidences",
                column: "MemberActivityId");
        }
    }
}
