using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TreeViewDemo.Migrations
{
    /// <inheritdoc />
    public partial class M005_Misc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUsers_Categories_TreeId",
                table: "AppUsers");

            migrationBuilder.DropIndex(
                name: "IX_AppUsers_TreeId",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "TreeId",
                table: "AppUsers");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Categories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TreeName",
                table: "AppUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_UserId",
                table: "Categories",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_AppUsers_UserId",
                table: "Categories",
                column: "UserId",
                principalTable: "AppUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_AppUsers_UserId",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_UserId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "TreeName",
                table: "AppUsers");

            migrationBuilder.AddColumn<int>(
                name: "TreeId",
                table: "AppUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_TreeId",
                table: "AppUsers",
                column: "TreeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUsers_Categories_TreeId",
                table: "AppUsers",
                column: "TreeId",
                principalTable: "Categories",
                principalColumn: "Id");
        }
    }
}
