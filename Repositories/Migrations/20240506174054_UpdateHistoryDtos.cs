using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blockchain_Indexer.Migrations
{
    /// <inheritdoc />
    public partial class UpdateHistoryDtos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DateNameId",
                table: "TransactionsHistory",
                type: "character varying(11)",
                maxLength: 11,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionsHistory_DateNameId",
                table: "TransactionsHistory",
                column: "DateNameId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TransactionsHistory_DateNameId",
                table: "TransactionsHistory");

            migrationBuilder.AlterColumn<string>(
                name: "DateNameId",
                table: "TransactionsHistory",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(11)",
                oldMaxLength: 11);
        }
    }
}
