using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HockeyPool.Infrastructure.Migrations;

/// <inheritdoc />
public partial class PredictionLog : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "PredictionLogs",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                MatchupId = table.Column<int>(type: "INTEGER", nullable: false),
                TimeStamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                AspNetUserId = table.Column<Guid>(type: "TEXT", nullable: false),
                HomeTeamScore = table.Column<int>(type: "INTEGER", nullable: true),
                GuestTeamScore = table.Column<int>(type: "INTEGER", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PredictionLogs", x => x.Id);
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "PredictionLogs");
    }
}
