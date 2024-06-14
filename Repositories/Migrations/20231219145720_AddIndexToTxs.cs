using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blockchain_Indexer.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexToTxs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Transactions_Block",
                table: "Transactions",
                column: "Block");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Transactions_Block",
                table: "Transactions");
        }
    }
}
