using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VakantiePark.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ParkSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ParkSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CleaningFee = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: false),
                    TouristTaxPerPersonPerNight = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: false),
                    ArrivalTime = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    DepartureTime = table.Column<TimeOnly>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParkSettings", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParkSettings");
        }
    }
}
