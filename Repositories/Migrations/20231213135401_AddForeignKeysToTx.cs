using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blockchain_Indexer.Migrations
{
    /// <inheritdoc />
    public partial class AddForeignKeysToTx : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Wallets_FromAddress",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Wallets_ToAddress",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_FromAddress",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_ToAddress",
                table: "Transactions");

            migrationBuilder.AddColumn<string>(
                name: "FromWalletAddress",
                table: "Transactions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ToWalletAddress",
                table: "Transactions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_FromWalletAddress",
                table: "Transactions",
                column: "FromWalletAddress");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_ToWalletAddress",
                table: "Transactions",
                column: "ToWalletAddress");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Wallets_FromWalletAddress",
                table: "Transactions",
                column: "FromWalletAddress",
                principalTable: "Wallets",
                principalColumn: "Address",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Wallets_ToWalletAddress",
                table: "Transactions",
                column: "ToWalletAddress",
                principalTable: "Wallets",
                principalColumn: "Address",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Wallets_FromWalletAddress",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Wallets_ToWalletAddress",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_FromWalletAddress",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_ToWalletAddress",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "FromWalletAddress",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "ToWalletAddress",
                table: "Transactions");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_FromAddress",
                table: "Transactions",
                column: "FromAddress");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_ToAddress",
                table: "Transactions",
                column: "ToAddress");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Wallets_FromAddress",
                table: "Transactions",
                column: "FromAddress",
                principalTable: "Wallets",
                principalColumn: "Address",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Wallets_ToAddress",
                table: "Transactions",
                column: "ToAddress",
                principalTable: "Wallets",
                principalColumn: "Address",
                onDelete: ReferentialAction.Cascade);
        }
    }
}