using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TreeViewDemo.Migrations
{
    /// <inheritdoc />
    public partial class AttributesAndColor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Attribute1",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Attribute2",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Attribute3",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Attribute4",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ColorCode",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Attribute1",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "Attribute2",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "Attribute3",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "Attribute4",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "ColorCode",
                table: "Categories");
        }
    }
}
