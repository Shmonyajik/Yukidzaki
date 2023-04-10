using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Babadzaki.Migrations
{
    /// <inheritdoc />
    public partial class AddClear : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsChecked",
                table: "TokensFilters");

            migrationBuilder.DropColumn(
                name: "TotalTokensNum",
                table: "SeasonCollections");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsChecked",
                table: "TokensFilters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<short>(
                name: "TotalTokensNum",
                table: "SeasonCollections",
                type: "smallint",
                nullable: true);
        }
    }
}
