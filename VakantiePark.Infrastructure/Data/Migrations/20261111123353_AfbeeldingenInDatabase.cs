using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VakantiePark.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AfbeeldingenInDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Afbeeldingen",
                table: "AccommodatieTypes");

            migrationBuilder.CreateTable(
                name: "Afbeeldingen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Bestandsnaam = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    ContentType = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Volgorde = table.Column<int>(type: "INTEGER", nullable: false),
                    AccommodatieTypeId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Afbeeldingen", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Afbeeldingen_AccommodatieTypes_AccommodatieTypeId",
                        column: x => x.AccommodatieTypeId,
                        principalTable: "AccommodatieTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AfbeeldingBestanden",
                columns: table => new
                {
                    AfbeeldingId = table.Column<int>(type: "INTEGER", nullable: false),
                    Data = table.Column<byte[]>(type: "BLOB", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AfbeeldingBestanden", x => x.AfbeeldingId);
                    table.ForeignKey(
                        name: "FK_AfbeeldingBestanden_Afbeeldingen_AfbeeldingId",
                        column: x => x.AfbeeldingId,
                        principalTable: "Afbeeldingen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Afbeeldingen_AccommodatieTypeId_Volgorde",
                table: "Afbeeldingen",
                columns: new[] { "AccommodatieTypeId", "Volgorde" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AfbeeldingBestanden");

            migrationBuilder.DropTable(
                name: "Afbeeldingen");

            migrationBuilder.AddColumn<string>(
                name: "Afbeeldingen",
                table: "AccommodatieTypes",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
