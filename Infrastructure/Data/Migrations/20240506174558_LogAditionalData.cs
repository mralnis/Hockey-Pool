using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HockeyPool.Infrastructure.Migrations;

/// <inheritdoc />
public partial class LogAditionalData : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "BrowserVersion",
            table: "PredictionLogs",
            type: "TEXT",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<string>(
            name: "IpAddress",
            table: "PredictionLogs",
            type: "TEXT",
            nullable: false,
            defaultValue: "");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "BrowserVersion",
            table: "PredictionLogs");

        migrationBuilder.DropColumn(
            name: "IpAddress",
            table: "PredictionLogs");
    }
}
