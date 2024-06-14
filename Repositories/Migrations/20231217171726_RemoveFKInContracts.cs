using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blockchain_Indexer.Migrations
{
    /// <inheritdoc />
    public partial class RemoveFKInContracts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Addresses_CreatorAddress",
                table: "Contracts");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_CreatorAddress",
                table: "Contracts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Contracts_CreatorAddress",
                table: "Contracts",
                column: "CreatorAddress");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Addresses_CreatorAddress",
                table: "Contracts",
                column: "CreatorAddress",
                principalTable: "Addresses",
                principalColumn: "Address",
                onDelete: ReferentialAction.Cascade);
        }
    }
}