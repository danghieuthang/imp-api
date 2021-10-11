using Microsoft.EntityFrameworkCore.Migrations;

namespace IMP.Infrastructure.Persistence.Migrations
{
    public partial class change_block_item_name : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlockItem_Blocks_BlockId",
                table: "BlockItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BlockItem",
                table: "BlockItem");

            migrationBuilder.RenameTable(
                name: "BlockItem",
                newName: "BlockItems");

            migrationBuilder.RenameIndex(
                name: "IX_BlockItem_BlockId",
                table: "BlockItems",
                newName: "IX_BlockItems_BlockId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlockItems",
                table: "BlockItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BlockItems_Blocks_BlockId",
                table: "BlockItems",
                column: "BlockId",
                principalTable: "Blocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlockItems_Blocks_BlockId",
                table: "BlockItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BlockItems",
                table: "BlockItems");

            migrationBuilder.RenameTable(
                name: "BlockItems",
                newName: "BlockItem");

            migrationBuilder.RenameIndex(
                name: "IX_BlockItems_BlockId",
                table: "BlockItem",
                newName: "IX_BlockItem_BlockId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlockItem",
                table: "BlockItem",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BlockItem_Blocks_BlockId",
                table: "BlockItem",
                column: "BlockId",
                principalTable: "Blocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
