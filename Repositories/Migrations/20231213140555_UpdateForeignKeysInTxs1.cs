using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blockchain_Indexer.Migrations
{
    /// <inheritdoc />
    public partial class UpdateForeignKeysInTxs1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WalletAddress",
                table: "Wallets",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_WalletAddress",
                table: "Wallets",
                column: "WalletAddress");

            migrationBuilder.AddForeignKey(
                name: "FK_Wallets_Wallets_WalletAddress",
                table: "Wallets",
                column: "WalletAddress",
                principalTable: "Wallets",
                principalColumn: "Address");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wallets_Wallets_WalletAddress",
                table: "Wallets");

            migrationBuilder.DropIndex(
                name: "IX_Wallets_WalletAddress",
                table: "Wallets");

            migrationBuilder.DropColumn(
                name: "WalletAddress",
                table: "Wallets");
        }
    }
}