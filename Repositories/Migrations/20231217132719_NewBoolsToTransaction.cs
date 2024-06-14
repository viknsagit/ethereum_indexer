using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blockchain_Indexer.Migrations
{
    /// <inheritdoc />
    public partial class NewBoolsToTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsContractDeployed",
                table: "Transaction",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsContractInteraction",
                table: "Transaction",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "Timestamp",
                table: "Blocks",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsContractDeployed",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "IsContractInteraction",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "Blocks");
        }
    }
}