using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blockchain_Indexer.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTransactionClass1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxFeePerGas",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "MaxPriorityFeePerGas",
                table: "Transactions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "MaxFeePerGas",
                table: "Transactions",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MaxPriorityFeePerGas",
                table: "Transactions",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }
    }
}