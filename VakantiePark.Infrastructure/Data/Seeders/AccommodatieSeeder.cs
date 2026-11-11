using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using VakantiePark.Domain.Entities;
using VakantiePark.Domain.Enums;

namespace VakantiePark.Infrastructure.Data.Seeders
{
    public static class AccommodatieSeeder
    {
        private static readonly string SeedFolder = Path.Combine(AppContext.BaseDirectory, "Data", "Seed");

        public static void UseAccommodatieSeeder(this DbContext context)
        {
            if (!context.Set<Accommodatie>().Any())
            {
                var types = SeedTypes(context);
                var voorzieningen = SeedVoorzieningen(context, Lees<Dictionary<string, string>>("voorzieningen.json"));
                SeedAccommodaties(context, types, voorzieningen, Lees<Dictionary<string, string>>("waarden.json"));
            }

            if (!context.Set<Afbeelding>().Any())
                SeedAfbeeldingen(context, Lees<Dictionary<string, List<string>>>("afbeeldingen.json"));
        }

        private static T Lees<T>(string bestand) =>
            JsonSerializer.Deserialize<T>(File.ReadAllText(Path.Combine(SeedFolder, bestand)))
            ?? throw new InvalidOperationException($"{bestand} is leeg of ongeldig.");

        private static Dictionary<string, AccommodatieType> SeedTypes(DbContext context)
        {
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
                    PrijsPerNacht = 0m,
                };
                context.Add(nieuw);
                types[nieuw.Naam] = nieuw;
            }

            return types;
        }

        private static Dictionary<string, Voorziening> SeedVoorzieningen(
            DbContext context,
            Dictionary<string, string> namen)
        {
            var voorzieningen = context.Set<Voorziening>().ToDictionary(v => v.Code);

            foreach (var (code, naam) in namen)
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
            Dictionary<string, Voorziening> voorzieningen,
            Dictionary<string, string> waarden)
        {
            string? Vertaal(JsonElement e) =>
                e.GetString() is { } w ? waarden.GetValueOrDefault(w, w) : null;

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
                        case "surface": accommodatie.Ondergrond = Vertaal(prop.Value); break;
                        case "shade": accommodatie.Schaduw = Vertaal(prop.Value); break;
                        case "wifiCoverage": accommodatie.WifiDekking = Vertaal(prop.Value); break;

                        default:
                            if (prop.Value.ValueKind == JsonValueKind.True)
                            {
                                if (!voorzieningen.TryGetValue(prop.Name, out var voorziening))
                                    throw new InvalidOperationException(
                                        $"Onbekende voorziening '{prop.Name}' in accommodaties.json. Voeg hem toe aan voorzieningen.json.");
                                accommodatie.Voorzieningen.Add(voorziening);
                            }
                            break;
                    }
                }

                context.Add(accommodatie);
            }

            context.SaveChanges();
        }

        private static void SeedAfbeeldingen(DbContext context, Dictionary<string, List<string>> afbeeldingenPerType)
        {
            var types = context.Set<AccommodatieType>().ToDictionary(t => t.Slug);
            string map = Path.Combine(SeedFolder, "afbeeldingen");

            foreach (var (slug, bestanden) in afbeeldingenPerType)
            {
                if (!types.TryGetValue(slug, out var type))
                    throw new InvalidOperationException($"Onbekend type '{slug}' in afbeeldingen.json.");

                for (int i = 0; i < bestanden.Count; i++)
                {
                    string pad = Path.Combine(map, bestanden[i]);
                    if (!File.Exists(pad))
                        throw new InvalidOperationException($"Foto '{bestanden[i]}' uit afbeeldingen.json staat niet in Data/Seed/afbeeldingen.");

                    context.Add(new Afbeelding
                    {
                        Bestandsnaam = bestanden[i],
                        ContentType = ContentType(pad),
                        Volgorde = i,
                        AccommodatieType = type,
                        Bestand = new AfbeeldingBestand { Data = File.ReadAllBytes(pad) },
                    });
                }
            }

            context.SaveChanges();
        }

        private static string ContentType(string pad) => Path.GetExtension(pad).ToLowerInvariant() switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".webp" => "image/webp",
            _ => throw new InvalidOperationException($"Onbekend afbeeldingsformaat: {Path.GetFileName(pad)}"),
        };

        private static AccommodatieCategorie ParseCategorie(string? category) => category switch
        {
            "holiday_home" => AccommodatieCategorie.Huisje,
            "camping_pitch" => AccommodatieCategorie.Campingplaats,
            _ => throw new InvalidOperationException($"Onbekende categorie '{category}' in types.json."),
        };
    }
}
