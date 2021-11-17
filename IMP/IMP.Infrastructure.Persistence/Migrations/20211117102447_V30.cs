using Microsoft.EntityFrameworkCore.Migrations;

namespace IMP.Infrastructure.Persistence.Migrations
{
    public partial class V30 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Evidences_MemberActivityId",
                table: "Evidences");

            migrationBuilder.CreateIndex(
                name: "IX_Evidences_MemberActivityId",
                table: "Evidences",
                column: "MemberActivityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Evidences_MemberActivityId",
                table: "Evidences");

            migrationBuilder.CreateIndex(
                name: "IX_Evidences_MemberActivityId",
                table: "Evidences",
                column: "MemberActivityId",
                unique: true);
        }
    }
}
