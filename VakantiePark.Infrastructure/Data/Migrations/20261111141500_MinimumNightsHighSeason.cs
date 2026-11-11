using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VakantiePark.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class MinimumNightsHighSeason : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MinimumNightsHighSeason",
                table: "ParkSettings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 7);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MinimumNightsHighSeason",
                table: "ParkSettings");
        }
    }
}
