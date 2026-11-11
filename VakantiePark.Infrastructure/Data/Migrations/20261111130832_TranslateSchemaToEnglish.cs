using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VakantiePark.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class TranslateSchemaToEnglish : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccommodatieVoorzieningen");

            migrationBuilder.DropTable(
                name: "AfbeeldingBestanden");

            migrationBuilder.DropTable(
                name: "Accommodaties");

            migrationBuilder.DropTable(
                name: "Voorzieningen");

            migrationBuilder.DropTable(
                name: "Afbeeldingen");

            migrationBuilder.DropTable(
                name: "AccommodatieTypes");

            migrationBuilder.CreateTable(
                name: "AccommodationTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Slug = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Category = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    PricePerNight = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccommodationTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Amenities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Code = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Amenities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Accommodations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Code = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    MaxPersons = table.Column<int>(type: "INTEGER", nullable: false),
                    Zone = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    MaxPets = table.Column<int>(type: "INTEGER", nullable: false),
                    Bedrooms = table.Column<int>(type: "INTEGER", nullable: true),
                    Bathrooms = table.Column<int>(type: "INTEGER", nullable: true),
                    Beds = table.Column<int>(type: "INTEGER", nullable: true),
                    ElectricityAmps = table.Column<int>(type: "INTEGER", nullable: true),
                    MaxVehicleLengthMeters = table.Column<decimal>(type: "TEXT", precision: 4, scale: 1, nullable: true),
                    MaxCaravanLengthMeters = table.Column<decimal>(type: "TEXT", precision: 4, scale: 1, nullable: true),
                    Surface = table.Column<string>(type: "TEXT", maxLength: 30, nullable: true),
                    Shade = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    WifiCoverage = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    AccommodationTypeId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accommodations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accommodations_AccommodationTypes_AccommodationTypeId",
                        column: x => x.AccommodationTypeId,
                        principalTable: "AccommodationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Photos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FileName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    ContentType = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    AccommodationTypeId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Photos_AccommodationTypes_AccommodationTypeId",
                        column: x => x.AccommodationTypeId,
                        principalTable: "AccommodationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccommodationAmenities",
                columns: table => new
                {
                    AccommodationsId = table.Column<int>(type: "INTEGER", nullable: false),
                    AmenitiesId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccommodationAmenities", x => new { x.AccommodationsId, x.AmenitiesId });
                    table.ForeignKey(
                        name: "FK_AccommodationAmenities_Accommodations_AccommodationsId",
                        column: x => x.AccommodationsId,
                        principalTable: "Accommodations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccommodationAmenities_Amenities_AmenitiesId",
                        column: x => x.AmenitiesId,
                        principalTable: "Amenities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PhotoFiles",
                columns: table => new
                {
                    PhotoId = table.Column<int>(type: "INTEGER", nullable: false),
                    Data = table.Column<byte[]>(type: "BLOB", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhotoFiles", x => x.PhotoId);
                    table.ForeignKey(
                        name: "FK_PhotoFiles_Photos_PhotoId",
                        column: x => x.PhotoId,
                        principalTable: "Photos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccommodationAmenities_AmenitiesId",
                table: "AccommodationAmenities",
                column: "AmenitiesId");

            migrationBuilder.CreateIndex(
                name: "IX_Accommodations_AccommodationTypeId",
                table: "Accommodations",
                column: "AccommodationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Accommodations_Code",
                table: "Accommodations",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccommodationTypes_Name",
                table: "AccommodationTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccommodationTypes_Slug",
                table: "AccommodationTypes",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Amenities_Code",
                table: "Amenities",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Photos_AccommodationTypeId_SortOrder",
                table: "Photos",
                columns: new[] { "AccommodationTypeId", "SortOrder" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccommodationAmenities");

            migrationBuilder.DropTable(
                name: "PhotoFiles");

            migrationBuilder.DropTable(
                name: "Accommodations");

            migrationBuilder.DropTable(
                name: "Amenities");

            migrationBuilder.DropTable(
                name: "Photos");

            migrationBuilder.DropTable(
                name: "AccommodationTypes");

            migrationBuilder.CreateTable(
                name: "AccommodatieTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Categorie = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Naam = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Omschrijving = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    PrijsPerNacht = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: false),
                    Slug = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
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
                    AccommodatieTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    Badkamers = table.Column<int>(type: "INTEGER", nullable: true),
                    Bedden = table.Column<int>(type: "INTEGER", nullable: true),
                    Code = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    ElektriciteitAmpere = table.Column<int>(type: "INTEGER", nullable: true),
                    MaxCaravanLengteMeters = table.Column<decimal>(type: "TEXT", precision: 4, scale: 1, nullable: true),
                    MaxHuisdieren = table.Column<int>(type: "INTEGER", nullable: false),
                    MaxPersonen = table.Column<int>(type: "INTEGER", nullable: false),
                    MaxVoertuigLengteMeters = table.Column<decimal>(type: "TEXT", precision: 4, scale: 1, nullable: true),
                    Naam = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Ondergrond = table.Column<string>(type: "TEXT", maxLength: 30, nullable: true),
                    Schaduw = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    Slaapkamers = table.Column<int>(type: "INTEGER", nullable: true),
                    WifiDekking = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    Zone = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
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
                name: "Afbeeldingen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AccommodatieTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    Bestandsnaam = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    ContentType = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Volgorde = table.Column<int>(type: "INTEGER", nullable: false)
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
                name: "IX_Afbeeldingen_AccommodatieTypeId_Volgorde",
                table: "Afbeeldingen",
                columns: new[] { "AccommodatieTypeId", "Volgorde" });

            migrationBuilder.CreateIndex(
                name: "IX_Voorzieningen_Code",
                table: "Voorzieningen",
                column: "Code",
                unique: true);
        }
    }
}
