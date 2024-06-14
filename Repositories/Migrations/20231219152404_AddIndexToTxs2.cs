using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blockchain_Indexer.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexToTxs2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Transactions_Block_Number",
                table: "Transactions");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_Block_Number_Timestamp",
                table: "Transactions",
                columns: new[] { "Block", "Number", "Timestamp" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Transactions_Block_Number_Timestamp",
                table: "Transactions");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_Block_Number",
                table: "Transactions",
                columns: new[] { "Block", "Number" });
        }
    }
}
