using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VakantiePark.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class BookingSequence : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BookingSequence",
                table: "Reservations",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_BookingSequence",
                table: "Reservations",
                column: "BookingSequence",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reservations_BookingSequence",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "BookingSequence",
                table: "Reservations");
        }
    }
}
