using Microsoft.EntityFrameworkCore.Migrations;

namespace IMP.Infrastructure.Persistence.Migrations
{
    public partial class V33 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "Evidences",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Evidences",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ActivityComments_ApplicationUserId",
                table: "ActivityComments",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityComments_ApplicationUsers_ApplicationUserId",
                table: "ActivityComments",
                column: "ApplicationUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityComments_ApplicationUsers_ApplicationUserId",
                table: "ActivityComments");

            migrationBuilder.DropIndex(
                name: "IX_ActivityComments_ApplicationUserId",
                table: "ActivityComments");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Evidences");

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "Evidences",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);
        }
    }
}
