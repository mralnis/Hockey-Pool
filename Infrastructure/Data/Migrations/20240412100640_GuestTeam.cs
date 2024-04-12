using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HockeyPool.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class GuestTeam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EnemyTeamScore",
                table: "Predictions",
                newName: "GuestTeamScore");

            migrationBuilder.RenameColumn(
                name: "EnemyTeamScore",
                table: "Matchups",
                newName: "GuestTeamScore");

            migrationBuilder.RenameColumn(
                name: "CountryId",
                table: "Matchups",
                newName: "HomeTeamId");

            migrationBuilder.AddColumn<int>(
                name: "GuestTeamId",
                table: "Matchups",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GuestTeamId",
                table: "Matchups");

            migrationBuilder.RenameColumn(
                name: "GuestTeamScore",
                table: "Predictions",
                newName: "EnemyTeamScore");

            migrationBuilder.RenameColumn(
                name: "HomeTeamId",
                table: "Matchups",
                newName: "CountryId");

            migrationBuilder.RenameColumn(
                name: "GuestTeamScore",
                table: "Matchups",
                newName: "EnemyTeamScore");
        }
    }
}
