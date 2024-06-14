using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blockchain_Indexer.Migrations
{
    /// <inheritdoc />
    public partial class addTokenHolders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TokenHolders",
                columns: table => new
                {
                    Owner = table.Column<string>(type: "text", nullable: false),
                    TokenAddress = table.Column<string>(type: "text", nullable: false),
                    TokenValue = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TokenHolders", x => x.Owner);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TokenHolders_TokenAddress",
                table: "TokenHolders",
                column: "TokenAddress");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TokenHolders");
        }
    }
}
