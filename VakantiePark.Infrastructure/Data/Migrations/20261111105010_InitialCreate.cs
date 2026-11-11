using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VakantiePark.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccommodatieTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Naam = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Slug = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Categorie = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Omschrijving = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    PrijsPerNacht = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: false),
                    Afbeeldingen = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccommodatieTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Voorzieningen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Code = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Naam = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Voorzieningen", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Accommodaties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Code = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Naam = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    MaxPersonen = table.Column<int>(type: "INTEGER", nullable: false),
                    Zone = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    MaxHuisdieren = table.Column<int>(type: "INTEGER", nullable: false),
                    Slaapkamers = table.Column<int>(type: "INTEGER", nullable: true),
                    Badkamers = table.Column<int>(type: "INTEGER", nullable: true),
                    Bedden = table.Column<int>(type: "INTEGER", nullable: true),
                    ElektriciteitAmpere = table.Column<int>(type: "INTEGER", nullable: true),
                    MaxVoertuigLengteMeters = table.Column<decimal>(type: "TEXT", precision: 4, scale: 1, nullable: true),
                    MaxCaravanLengteMeters = table.Column<decimal>(type: "TEXT", precision: 4, scale: 1, nullable: true),
                    Ondergrond = table.Column<string>(type: "TEXT", maxLength: 30, nullable: true),
                    Schaduw = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    WifiDekking = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    AccommodatieTypeId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accommodaties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accommodaties_AccommodatieTypes_AccommodatieTypeId",
                        column: x => x.AccommodatieTypeId,
                        principalTable: "AccommodatieTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AccommodatieVoorzieningen",
                columns: table => new
                {
                    AccommodatiesId = table.Column<int>(type: "INTEGER", nullable: false),
                    VoorzieningenId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccommodatieVoorzieningen", x => new { x.AccommodatiesId, x.VoorzieningenId });
                    table.ForeignKey(
                        name: "FK_AccommodatieVoorzieningen_Accommodaties_AccommodatiesId",
                        column: x => x.AccommodatiesId,
                        principalTable: "Accommodaties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccommodatieVoorzieningen_Voorzieningen_VoorzieningenId",
                        column: x => x.VoorzieningenId,
                        principalTable: "Voorzieningen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accommodaties_AccommodatieTypeId",
                table: "Accommodaties",
                column: "AccommodatieTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Accommodaties_Code",
                table: "Accommodaties",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccommodatieTypes_Naam",
                table: "AccommodatieTypes",
                column: "Naam",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccommodatieTypes_Slug",
                table: "AccommodatieTypes",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccommodatieVoorzieningen_VoorzieningenId",
                table: "AccommodatieVoorzieningen",
                column: "VoorzieningenId");

            migrationBuilder.CreateIndex(
                name: "IX_Voorzieningen_Code",
                table: "Voorzieningen",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccommodatieVoorzieningen");

            migrationBuilder.DropTable(
                name: "Accommodaties");

            migrationBuilder.DropTable(
                name: "Voorzieningen");

            migrationBuilder.DropTable(
                name: "AccommodatieTypes");
        }
    }
}
