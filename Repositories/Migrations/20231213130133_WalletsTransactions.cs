using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blockchain_Indexer.Migrations
{
    /// <inheritdoc />
    public partial class WalletsTransactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Wallets",
                columns: table => new
                {
                    Address = table.Column<string>(type: "text", nullable: false),
                    TransactionsCount = table.Column<int>(type: "integer", nullable: false),
                    TokenTransfersCount = table.Column<int>(type: "integer", nullable: false),
                    GasUsed = table.Column<decimal>(type: "numeric", nullable: false),
                    LastBalanceUpdate = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallets", x => x.Address);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Hash = table.Column<string>(type: "text", nullable: false),
                    Block = table.Column<int>(type: "integer", nullable: false),
                    Timestamp = table.Column<long>(type: "bigint", nullable: false),
                    FromAddress = table.Column<string>(type: "text", nullable: false),
                    ToAddress = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Hash);
                    table.ForeignKey(
                        name: "FK_Transactions_Wallets_FromAddress",
                        column: x => x.FromAddress,
                        principalTable: "Wallets",
                        principalColumn: "Address",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transactions_Wallets_ToAddress",
                        column: x => x.ToAddress,
                        principalTable: "Wallets",
                        principalColumn: "Address",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_FromAddress",
                table: "Transactions",
                column: "FromAddress");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_ToAddress",
                table: "Transactions",
                column: "ToAddress");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Wallets");
        }
    }
}