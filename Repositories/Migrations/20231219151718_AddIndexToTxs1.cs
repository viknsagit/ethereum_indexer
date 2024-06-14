using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blockchain_Indexer.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexToTxs1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Transactions_Block",
                table: "Transactions");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_Block_Number",
                table: "Transactions",
                columns: new[] { "Block", "Number" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Transactions_Block_Number",
                table: "Transactions");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_Block",
                table: "Transactions",
                column: "Block");
        }
    }
}
