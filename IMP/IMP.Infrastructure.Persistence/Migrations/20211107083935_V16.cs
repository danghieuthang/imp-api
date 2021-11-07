using Microsoft.EntityFrameworkCore.Migrations;

namespace IMP.Infrastructure.Persistence.Migrations
{
    public partial class V16 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Openning",
                table: "Campaigns",
                newName: "OpenningDate");

            migrationBuilder.RenameColumn(
                name: "Evaluating",
                table: "Campaigns",
                newName: "EvaluatingDate");

            migrationBuilder.RenameColumn(
                name: "Closed",
                table: "Campaigns",
                newName: "ClosedDate");

            migrationBuilder.RenameColumn(
                name: "Applying",
                table: "Campaigns",
                newName: "ApplyingDate");

            migrationBuilder.RenameColumn(
                name: "Announcing",
                table: "Campaigns",
                newName: "AnnouncingDate");

            migrationBuilder.RenameColumn(
                name: "Advertising",
                table: "Campaigns",
                newName: "AdvertisingDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OpenningDate",
                table: "Campaigns",
                newName: "Openning");

            migrationBuilder.RenameColumn(
                name: "EvaluatingDate",
                table: "Campaigns",
                newName: "Evaluating");

            migrationBuilder.RenameColumn(
                name: "ClosedDate",
                table: "Campaigns",
                newName: "Closed");

            migrationBuilder.RenameColumn(
                name: "ApplyingDate",
                table: "Campaigns",
                newName: "Applying");

            migrationBuilder.RenameColumn(
                name: "AnnouncingDate",
                table: "Campaigns",
                newName: "Announcing");

            migrationBuilder.RenameColumn(
                name: "AdvertisingDate",
                table: "Campaigns",
                newName: "Advertising");
        }
    }
}
