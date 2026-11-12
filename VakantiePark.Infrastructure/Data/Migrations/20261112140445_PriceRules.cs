using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VakantiePark.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class PriceRules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PetFeePerNight",
                table: "ParkSettings",
                type: "TEXT",
                precision: 10,
                scale: 2,
                nullable: false,
                defaultValue: 10m);

            migrationBuilder.AddColumn<int>(
                name: "TouristTaxMinimumAge",
                table: "ParkSettings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 3);

            migrationBuilder.AddColumn<decimal>(
                name: "WeekendSurchargePercentage",
                table: "ParkSettings",
                type: "TEXT",
                precision: 5,
                scale: 2,
                nullable: false,
                defaultValue: 10m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PetFeePerNight",
                table: "ParkSettings");

            migrationBuilder.DropColumn(
                name: "TouristTaxMinimumAge",
                table: "ParkSettings");

            migrationBuilder.DropColumn(
                name: "WeekendSurchargePercentage",
                table: "ParkSettings");
        }
    }
}
