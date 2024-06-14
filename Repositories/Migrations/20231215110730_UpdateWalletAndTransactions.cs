using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blockchain_Indexer.Migrations
{
    /// <inheritdoc />
    public partial class UpdateWalletAndTransactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Wallets_CreatorAddress",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Wallets_FromAddress",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Wallets_ToAddress",
                table: "Transactions");

            migrationBuilder.DropTable(
                name: "Tokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Wallets",
                table: "Wallets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "TokenTransfersCount",
                table: "Wallets");

            migrationBuilder.RenameTable(
                name: "Wallets",
                newName: "Addresses");

            migrationBuilder.RenameTable(
                name: "Transactions",
                newName: "Transaction");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_ToAddress",
                table: "Transaction",
                newName: "IX_Transaction_ToAddress");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_FromAddress",
                table: "Transaction",
                newName: "IX_Transaction_FromAddress");

            migrationBuilder.AddColumn<decimal>(
                name: "Gas",
                table: "Transaction",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "GasPrice",
                table: "Transaction",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Addresses",
                table: "Addresses",
                column: "Address");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transaction",
                table: "Transaction",
                column: "Hash");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Addresses_CreatorAddress",
                table: "Contracts",
                column: "CreatorAddress",
                principalTable: "Addresses",
                principalColumn: "Address",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Addresses_FromAddress",
                table: "Transaction",
                column: "FromAddress",
                principalTable: "Addresses",
                principalColumn: "Address",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Addresses_ToAddress",
                table: "Transaction",
                column: "ToAddress",
                principalTable: "Addresses",
                principalColumn: "Address",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Addresses_CreatorAddress",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Addresses_FromAddress",
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Addresses_ToAddress",
                table: "Transaction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transaction",
                table: "Transaction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Addresses",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "Gas",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "GasPrice",
                table: "Transaction");

            migrationBuilder.RenameTable(
                name: "Transaction",
                newName: "Transactions");

            migrationBuilder.RenameTable(
                name: "Addresses",
                newName: "Wallets");

            migrationBuilder.RenameIndex(
                name: "IX_Transaction_ToAddress",
                table: "Transactions",
                newName: "IX_Transactions_ToAddress");

            migrationBuilder.RenameIndex(
                name: "IX_Transaction_FromAddress",
                table: "Transactions",
                newName: "IX_Transactions_FromAddress");

            migrationBuilder.AddColumn<int>(
                name: "TokenTransfersCount",
                table: "Wallets",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions",
                column: "Hash");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Wallets",
                table: "Wallets",
                column: "Address");

            migrationBuilder.CreateTable(
                name: "Tokens",
                columns: table => new
                {
                    Address = table.Column<string>(type: "text", nullable: false),
                    Holders = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Supply = table.Column<decimal>(type: "numeric", nullable: false),
                    Symbol = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tokens", x => x.Address);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Wallets_CreatorAddress",
                table: "Contracts",
                column: "CreatorAddress",
                principalTable: "Wallets",
                principalColumn: "Address",
                onDelete: ReferentialAction.Cascade);

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