using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MigrationBundel.Migrations
{
    /// <inheritdoc />
    public partial class Add_Currency_Tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "application");

            migrationBuilder.CreateTable(
                name: "Currencies",
                schema: "application",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Rate = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Favorites",
                schema: "application",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CurrencyId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favorites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Favorites_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalSchema: "application",
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Histories",
                schema: "application",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FavoriteId = table.Column<Guid>(type: "uuid", nullable: false),
                    Action = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Histories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Histories_Favorites_FavoriteId",
                        column: x => x.FavoriteId,
                        principalSchema: "application",
                        principalTable: "Favorites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_CurrencyId",
                schema: "application",
                table: "Favorites",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Histories_FavoriteId",
                schema: "application",
                table: "Histories",
                column: "FavoriteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Histories",
                schema: "application");

            migrationBuilder.DropTable(
                name: "Favorites",
                schema: "application");

            migrationBuilder.DropTable(
                name: "Currencies",
                schema: "application");
        }
    }
}
