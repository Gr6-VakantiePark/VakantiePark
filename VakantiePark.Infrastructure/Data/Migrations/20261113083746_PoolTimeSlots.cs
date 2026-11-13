using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VakantiePark.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class PoolTimeSlots : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PoolTimeSlots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    DayOfWeek = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    Capacity = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PoolTimeSlots", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PoolReservations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PoolTimeSlotId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Date = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    Persons = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PoolReservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PoolReservations_PoolTimeSlots_PoolTimeSlotId",
                        column: x => x.PoolTimeSlotId,
                        principalTable: "PoolTimeSlots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PoolReservations_PoolTimeSlotId_Date",
                table: "PoolReservations",
                columns: new[] { "PoolTimeSlotId", "Date" });

            migrationBuilder.CreateIndex(
                name: "IX_PoolTimeSlots_DayOfWeek_StartTime",
                table: "PoolTimeSlots",
                columns: new[] { "DayOfWeek", "StartTime" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PoolReservations");

            migrationBuilder.DropTable(
                name: "PoolTimeSlots");
        }
    }
}
