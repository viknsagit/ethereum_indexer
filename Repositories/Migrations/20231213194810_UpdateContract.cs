using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blockchain_Indexer.Migrations
{
    /// <inheritdoc />
    public partial class UpdateContract : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Hash",
                table: "Contracts",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hash",
                table: "Contracts");
        }
    }
}