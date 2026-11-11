using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using VakantiePark.Domain.Entities;
using VakantiePark.Domain.Enums;

namespace VakantiePark.Infrastructure.Data.Seeders
{
    /// <summary>
    /// Leest Data/Seed/accommodaties.json en types.json in en vult de tabellen
    /// AccommodatieTypes, Voorzieningen en Accommodaties. Doet niets als er al accommodaties zijn.
    /// </summary>
    public static class AccommodatieSeeder
    {
        private static readonly string SeedFolder = Path.Combine(AppContext.BaseDirectory, "Data", "Seed");

        /// <summary>Nederlandse namen voor de ja/nee-velden uit "properties".</summary>
        private static readonly Dictionary<string, string> VoorzieningNamen = new()
        {
            // Huisjes
            ["petsAllowed"] = "Huisdieren toegestaan",
            ["wheelchairAccessible"] = "Rolstoeltoegankelijk",
            ["sauna"] = "Sauna",
            ["hotTub"] = "Hottub",
            ["dishwasher"] = "Vaatwasser",
            ["washingMachine"] = "Wasmachine",
            ["airConditioning"] = "Airconditioning",
            ["fireplace"] = "Open haard",
            ["coveredTerrace"] = "Overdekt terras",
            ["chargingPoint"] = "Laadpaal",
            ["waterfront"] = "Aan het water",
            ["nearPool"] = "Dicht bij het zwembad",
            ["childFriendly"] = "Kindvriendelijk",
            ["smokeFree"] = "Rookvrij",
            ["levelAccess"] = "Drempelvrij",
            ["adaptedBathroom"] = "Aangepaste badkamer",
            ["widenedDoors"] = "Verbrede deuren",
            ["adjustableBed"] = "Hoog-laagbed",
            ["extraParkingSpace"] = "Extra parkeerplaats",
            // Campingplaatsen
            ["electricity"] = "Elektriciteit",
            ["waterConnection"] = "Wateraansluiting",
            ["drainConnection"] = "Afvoeraansluiting",
            ["privateSanitary"] = "Eigen sanitair",
            ["nearSanitary"] = "Dicht bij sanitairgebouw",
            ["carAtPitch"] = "Auto bij de plaats",
            ["extraWide"] = "Extra brede plaats",
        };

        /// <summary>Afbeeldingen per type (slug), volgens de bestanden in de assets-map. Eerste = hoofdfoto.</summary>
        private static readonly Dictionary<string, List<string>> AfbeeldingenPerType = new()
        {
            ["berk-4"] = ["berk4.png", "berk4-1.png", "berk4-2.png", "berk4-4.png"],
            ["berk-6"] = ["berk6.png", "berk6-1.png", "berk6-2.png", "berk6-3.png"],
            ["eik-6"] = ["eik6.png", "eik6-1.png", "eik6-2.png", "eik6-3.png"],
            ["eik-luxe-6"] = ["eikluxe6.png", "eikluxe6-1.png", "eikluxe6-2.png", "eikluxe6-3.png"],
            ["beuk-8"] = ["beuk8.png", "beuk8-1.png", "beuk8-2.png", "beuk8-3.png"],
            ["wilg-4-mindervalide"] = ["wilg4.png", "wilg4-1.png", "wilg4-2.png", "wilg4-3.png"],
            ["bosvilla-10"] = ["bosvilla10.png", "bosvilla10-1.png", "bosvilla10-2.png", "bosvilla10-3.png"],
            ["tentplaats-basis"] = ["tentbasis.png", "tentbasis1.png"],
            ["tentplaats-comfort"] = ["tentcomfort.png", "tentcomfort1.png"],
            ["caravanplaats"] = ["caravan.png", "caravan1.png"],
            ["camperplaats"] = ["camper.png", "camper1.png"],
            ["camperplaats-xl"] = ["camperXL.png", "camperXL1.png"],
        };

        public static void UseAccommodatieSeeder(this DbContext context)
        {
            // Alleen seeden als de tabel nog leeg is
            if (context.Set<Accommodatie>().Any())
                return;

            var types = SeedTypes(context);
            var voorzieningen = SeedVoorzieningen(context);
            SeedAccommodaties(context, types, voorzieningen);
        }

        private static Dictionary<string, AccommodatieType> SeedTypes(DbContext context)
        {
            // Bestaande types hergebruiken, zodat de seeder ook werkt als alleen Accommodaties leeg is
            var types = context.Set<AccommodatieType>().ToDictionary(t => t.Naam);

            using var doc = JsonDocument.Parse(File.ReadAllText(Path.Combine(SeedFolder, "types.json")));
            foreach (var type in doc.RootElement.EnumerateObject())
            {
                if (types.ContainsKey(type.Name))
                    continue;

                string slug = type.Value.GetProperty("slug").GetString()!;
                var nieuw = new AccommodatieType
                {
                    Naam = type.Name,
                    Slug = slug,
                    Categorie = ParseCategorie(type.Value.GetProperty("category").GetString()),
                    PrijsPerNacht = 0m, // staat niet in de JSON
                    Afbeeldingen = AfbeeldingenPerType.GetValueOrDefault(slug, []),
                };
                context.Add(nieuw);
                types[nieuw.Naam] = nieuw;
            }

            return types;
        }

        private static Dictionary<string, Voorziening> SeedVoorzieningen(DbContext context)
        {
            var voorzieningen = context.Set<Voorziening>().ToDictionary(v => v.Code);

            foreach (var (code, naam) in VoorzieningNamen)
            {
                if (voorzieningen.ContainsKey(code))
                    continue;

                var nieuw = new Voorziening { Code = code, Naam = naam };
                context.Add(nieuw);
                voorzieningen[code] = nieuw;
            }

            return voorzieningen;
        }

        private static void SeedAccommodaties(
            DbContext context,
            Dictionary<string, AccommodatieType> types,
            Dictionary<string, Voorziening> voorzieningen)
        {
            using var doc = JsonDocument.Parse(File.ReadAllText(Path.Combine(SeedFolder, "accommodaties.json")));

            foreach (var item in doc.RootElement.GetProperty("accommodations").EnumerateArray())
            {
                string typeNaam = item.GetProperty("type").GetString()!;
                if (!types.TryGetValue(typeNaam, out var type))
                    throw new InvalidOperationException($"Onbekend accommodatietype '{typeNaam}' in accommodaties.json.");

                var accommodatie = new Accommodatie
                {
                    Code = item.GetProperty("code").GetString()!,
                    Naam = item.GetProperty("name").GetString()!,
                    MaxPersonen = item.GetProperty("maxPersons").GetInt32(),
                    Zone = item.GetProperty("zone").GetString()!,
                    AccommodatieType = type,
                };

                foreach (var prop in item.GetProperty("properties").EnumerateObject())
                {
                    switch (prop.Name)
                    {
                        case "maxPets": accommodatie.MaxHuisdieren = prop.Value.GetInt32(); break;
                        case "bedrooms": accommodatie.Slaapkamers = prop.Value.GetInt32(); break;
                        case "bathrooms": accommodatie.Badkamers = prop.Value.GetInt32(); break;
                        case "beds": accommodatie.Bedden = prop.Value.GetInt32(); break;
                        case "electricityAmps": accommodatie.ElektriciteitAmpere = prop.Value.GetInt32(); break;
                        case "maxVehicleLengthMeters": accommodatie.MaxVoertuigLengteMeters = prop.Value.GetDecimal(); break;
                        case "maxCaravanLengthMeters": accommodatie.MaxCaravanLengteMeters = prop.Value.GetDecimal(); break;
                        case "surface": accommodatie.Ondergrond = prop.Value.GetString(); break;
                        case "shade": accommodatie.Schaduw = prop.Value.GetString(); break;
                        case "wifiCoverage": accommodatie.WifiDekking = prop.Value.GetString(); break;

                        default:
                            // Ja/nee-velden worden voorzieningen; alleen koppelen als de waarde true is
                            if (prop.Value.ValueKind == JsonValueKind.True)
                            {
                                if (!voorzieningen.TryGetValue(prop.Name, out var voorziening))
                                    throw new InvalidOperationException($"Onbekende voorziening '{prop.Name}' in accommodaties.json.");
                                accommodatie.Voorzieningen.Add(voorziening);
                            }
                            break;
                    }
                }

                context.Add(accommodatie);
            }

            context.SaveChanges();
        }

        private static AccommodatieCategorie ParseCategorie(string? category) => category switch
        {
            "holiday_home" => AccommodatieCategorie.Huisje,
            "camping_pitch" => AccommodatieCategorie.Campingplaats,
            _ => throw new InvalidOperationException($"Onbekende categorie '{category}' in types.json."),
        };
    }
}
