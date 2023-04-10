using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Babadzaki.Migrations
{
    /// <inheritdoc />
    public partial class AddDnaIdenyityLong : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "dna",
                table: "Tokens",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "dna",
                table: "Tokens",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
