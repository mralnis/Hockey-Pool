using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HockeyPool.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTournamentWinner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TournamentWinners",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TournamentId = table.Column<int>(type: "INTEGER", nullable: false),
                    AspNetUserId = table.Column<string>(type: "TEXT", nullable: false),
                    Place = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentWinners", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TournamentWinners");
        }
    }
}
