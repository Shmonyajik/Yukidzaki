using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Babadzaki.Migrations
{
    /// <inheritdoc />
    public partial class AddBackgroundField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Background",
                table: "Tokens",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Background",
                table: "Tokens");
        }
    }
}
