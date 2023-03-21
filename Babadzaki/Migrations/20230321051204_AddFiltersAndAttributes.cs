using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Babadzaki.Migrations
{
    /// <inheritdoc />
    public partial class AddFiltersAndAttributes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attribute_Filter_FilterId",
                table: "Attribute");

            migrationBuilder.DropForeignKey(
                name: "FK_TokensAttributes_Attribute_AttributeId",
                table: "TokensAttributes");

            migrationBuilder.DropForeignKey(
                name: "FK_TokensAttributes_Tokens_SeasonCollectionId",
                table: "TokensAttributes");

            migrationBuilder.DropIndex(
                name: "IX_TokensAttributes_SeasonCollectionId",
                table: "TokensAttributes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Filter",
                table: "Filter");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Attribute",
                table: "Attribute");

            migrationBuilder.DropColumn(
                name: "SeasonCollectionId",
                table: "TokensAttributes");

            migrationBuilder.RenameTable(
                name: "Filter",
                newName: "Filters");

            migrationBuilder.RenameTable(
                name: "Attribute",
                newName: "Attributes");

            migrationBuilder.RenameIndex(
                name: "IX_Attribute_FilterId",
                table: "Attributes",
                newName: "IX_Attributes_FilterId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Filters",
                table: "Filters",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Attributes",
                table: "Attributes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TokensAttributes_TokenId",
                table: "TokensAttributes",
                column: "TokenId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attributes_Filters_FilterId",
                table: "Attributes",
                column: "FilterId",
                principalTable: "Filters",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TokensAttributes_Attributes_AttributeId",
                table: "TokensAttributes",
                column: "AttributeId",
                principalTable: "Attributes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TokensAttributes_Tokens_TokenId",
                table: "TokensAttributes",
                column: "TokenId",
                principalTable: "Tokens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attributes_Filters_FilterId",
                table: "Attributes");

            migrationBuilder.DropForeignKey(
                name: "FK_TokensAttributes_Attributes_AttributeId",
                table: "TokensAttributes");

            migrationBuilder.DropForeignKey(
                name: "FK_TokensAttributes_Tokens_TokenId",
                table: "TokensAttributes");

            migrationBuilder.DropIndex(
                name: "IX_TokensAttributes_TokenId",
                table: "TokensAttributes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Filters",
                table: "Filters");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Attributes",
                table: "Attributes");

            migrationBuilder.RenameTable(
                name: "Filters",
                newName: "Filter");

            migrationBuilder.RenameTable(
                name: "Attributes",
                newName: "Attribute");

            migrationBuilder.RenameIndex(
                name: "IX_Attributes_FilterId",
                table: "Attribute",
                newName: "IX_Attribute_FilterId");

            migrationBuilder.AddColumn<int>(
                name: "SeasonCollectionId",
                table: "TokensAttributes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Filter",
                table: "Filter",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Attribute",
                table: "Attribute",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TokensAttributes_SeasonCollectionId",
                table: "TokensAttributes",
                column: "SeasonCollectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attribute_Filter_FilterId",
                table: "Attribute",
                column: "FilterId",
                principalTable: "Filter",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TokensAttributes_Attribute_AttributeId",
                table: "TokensAttributes",
                column: "AttributeId",
                principalTable: "Attribute",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TokensAttributes_Tokens_SeasonCollectionId",
                table: "TokensAttributes",
                column: "SeasonCollectionId",
                principalTable: "Tokens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
