using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blockchain_Indexer.Migrations
{
    /// <inheritdoc />
    public partial class RemoveFKFromTransactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Addresses_FromAddress",
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Addresses_ToAddress",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_FromAddress",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_ToAddress",
                table: "Transaction");

            migrationBuilder.AddColumn<bool>(
                name: "IsReverted",
                table: "Transaction",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReverted",
                table: "Transaction");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_FromAddress",
                table: "Transaction",
                column: "FromAddress");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_ToAddress",
                table: "Transaction",
                column: "ToAddress");

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
    }
}