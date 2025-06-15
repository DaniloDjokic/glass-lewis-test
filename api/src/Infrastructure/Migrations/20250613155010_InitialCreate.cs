using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Exchange = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Ticker = table.Column<string>(type: "text", nullable: false),
                    Isin = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    WebsiteUrl = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "Exchange", "Isin", "Name", "Ticker", "WebsiteUrl" },
                values: new object[,]
                {
                    { 1, "NASDAQ", "US0378331005", "Apple Inc.", "AAPL", "https://www.apple.com" },
                    { 2, "NASDAQ", "US5949181045", "Microsoft Corporation", "MSFT", "https://www.microsoft.com" },
                    { 3, "NASDAQ", "US02079K3059", "Alphabet Inc.", "GOOGL", "https://www.google.com" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Companies");
        }
    }
}
