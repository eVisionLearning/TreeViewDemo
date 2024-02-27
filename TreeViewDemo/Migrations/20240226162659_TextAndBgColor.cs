using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TreeViewDemo.Migrations
{
    /// <inheritdoc />
    public partial class TextAndBgColor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ColorCode",
                table: "Categories",
                newName: "TextColor");

            migrationBuilder.AddColumn<string>(
                name: "BgColor",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BgColor",
                table: "Categories");

            migrationBuilder.RenameColumn(
                name: "TextColor",
                table: "Categories",
                newName: "ColorCode");
        }
    }
}
