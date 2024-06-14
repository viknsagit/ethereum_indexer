using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blockchain_Indexer.Migrations
{
    /// <inheritdoc />
    public partial class updateTxsCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransactionsCount",
                table: "Tokens");

            migrationBuilder.AddColumn<int>(
                name: "TransactionsCount",
                table: "Contracts",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransactionsCount",
                table: "Contracts");

            migrationBuilder.AddColumn<int>(
                name: "TransactionsCount",
                table: "Tokens",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
