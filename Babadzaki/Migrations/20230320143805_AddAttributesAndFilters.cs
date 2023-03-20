using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Babadzaki.Migrations
{
    /// <inheritdoc />
    public partial class AddAttributesAndFilters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Background",
                table: "Tokens");

            migrationBuilder.CreateTable(
                name: "Filter",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsChecked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filter", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Attribute",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsChecked = table.Column<bool>(type: "bit", nullable: false),
                    FilterId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attribute", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attribute_Filter_FilterId",
                        column: x => x.FilterId,
                        principalTable: "Filter",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TokensAttributes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TokenId = table.Column<int>(type: "int", nullable: false),
                    SeasonCollectionId = table.Column<int>(type: "int", nullable: false),
                    AttributeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TokensAttributes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TokensAttributes_Attribute_AttributeId",
                        column: x => x.AttributeId,
                        principalTable: "Attribute",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TokensAttributes_Tokens_SeasonCollectionId",
                        column: x => x.SeasonCollectionId,
                        principalTable: "Tokens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attribute_FilterId",
                table: "Attribute",
                column: "FilterId");

            migrationBuilder.CreateIndex(
                name: "IX_TokensAttributes_AttributeId",
                table: "TokensAttributes",
                column: "AttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_TokensAttributes_SeasonCollectionId",
                table: "TokensAttributes",
                column: "SeasonCollectionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TokensAttributes");

            migrationBuilder.DropTable(
                name: "Attribute");

            migrationBuilder.DropTable(
                name: "Filter");

            migrationBuilder.AddColumn<string>(
                name: "Background",
                table: "Tokens",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);
        }
    }
}
