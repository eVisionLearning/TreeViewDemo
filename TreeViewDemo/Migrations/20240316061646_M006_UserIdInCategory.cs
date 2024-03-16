using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TreeViewDemo.Migrations
{
    /// <inheritdoc />
    public partial class M006_UserIdInCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_AppUsers_UserId",
                table: "Categories");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_AppUsers_UserId",
                table: "Categories",
                column: "UserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_AppUsers_UserId",
                table: "Categories");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Categories",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_AppUsers_UserId",
                table: "Categories",
                column: "UserId",
                principalTable: "AppUsers",
                principalColumn: "Id");
        }
    }
}
