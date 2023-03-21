using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Babadzaki.Migrations
{
    /// <inheritdoc />
    public partial class IsCheckedFieldTokensFilters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsChecked",
                table: "Filters");

            migrationBuilder.AddColumn<bool>(
                name: "IsChecked",
                table: "TokensFilters",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsChecked",
                table: "TokensFilters");

            migrationBuilder.AddColumn<bool>(
                name: "IsChecked",
                table: "Filters",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
