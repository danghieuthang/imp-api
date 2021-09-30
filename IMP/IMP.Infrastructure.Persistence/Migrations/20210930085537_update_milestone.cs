using Microsoft.EntityFrameworkCore.Migrations;

namespace IMP.Infrastructure.Persistence.Migrations
{
    public partial class update_milestone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Milestones",
                newName: "NameVi");

            migrationBuilder.AddColumn<string>(
                name: "NameEn",
                table: "Milestones",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameEn",
                table: "Milestones");

            migrationBuilder.RenameColumn(
                name: "NameVi",
                table: "Milestones",
                newName: "Name");
        }
    }
}
