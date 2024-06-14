using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blockchain_Indexer.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTransactionClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Input",
                table: "Transactions",
                type: "text",
                nullable: false,
                defaultValue: "");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Input",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "MaxFeePerGas",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "MaxPriorityFeePerGas",
                table: "Transactions");
        }
    }
}