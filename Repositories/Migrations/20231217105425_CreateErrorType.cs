using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blockchain_Indexer.Migrations
{
    /// <inheritdoc />
    public partial class CreateErrorType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Message",
                table: "Errors");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Errors",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Errors");

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "Errors",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}