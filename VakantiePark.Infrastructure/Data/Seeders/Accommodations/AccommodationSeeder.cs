using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using VakantiePark.Domain.Entities;
using VakantiePark.Domain.Enums;

namespace VakantiePark.Infrastructure.Data.Seeders
{
    public static class AccommodationSeeder
    {
        private static readonly string SeedFolder = Path.Combine(AppContext.BaseDirectory, "Data", "Seed");

        public static void UseAccommodationSeeder(this DbContext context)
        {
            if (!context.Set<Accommodation>().Any())
            {
                var types = SeedTypes(context);
                var amenities = SeedAmenities(context, Read<Dictionary<string, string>>("amenities.json"));
                SeedAccommodations(context, types, amenities, Read<Dictionary<string, string>>("value-labels.json"));
            }

            if (!context.Set<Photo>().Any())
                SeedPhotos(context, Read<Dictionary<string, List<string>>>("photos.json"));
        }

        private static T Read<T>(string fileName) =>
            JsonSerializer.Deserialize<T>(File.ReadAllText(Path.Combine(SeedFolder, fileName)))
            ?? throw new InvalidOperationException($"{fileName} is leeg of ongeldig.");

        private static Dictionary<string, AccommodationType> SeedTypes(DbContext context)
        {
            var types = context.Set<AccommodationType>().ToDictionary(t => t.Name);

            using var doc = JsonDocument.Parse(File.ReadAllText(Path.Combine(SeedFolder, "types.json")));
            foreach (var type in doc.RootElement.EnumerateObject())
            {
                if (types.ContainsKey(type.Name))
                    continue;

                string slug = type.Value.GetProperty("slug").GetString()!;
                var created = new AccommodationType
                {
                    Id = SeedIds.AccommodationType(slug),
                    Name = type.Name,
                    Slug = slug,
                    Category = ParseCategory(type.Value.GetProperty("category").GetString()),
                    PricePerNight = 0m,
                };
                context.Add(created);
                types[created.Name] = created;
            }

            return types;
        }

        private static Dictionary<string, Amenity> SeedAmenities(
            DbContext context,
            Dictionary<string, string> names)
        {
            var amenities = context.Set<Amenity>().ToDictionary(v => v.Code);

            foreach (var (code, name) in names)
            {
                if (amenities.ContainsKey(code))
                    continue;

                var created = new Amenity { Id = SeedIds.Amenity(code), Code = code, Name = name };
                context.Add(created);
                amenities[code] = created;
            }

            return amenities;
        }

        private static void SeedAccommodations(
            DbContext context,
            Dictionary<string, AccommodationType> types,
            Dictionary<string, Amenity> amenities,
            Dictionary<string, string> valueLabels)
        {
            string? Translate(JsonElement e) =>
                e.GetString() is { } w ? valueLabels.GetValueOrDefault(w, w) : null;

            using var doc = JsonDocument.Parse(File.ReadAllText(Path.Combine(SeedFolder, "accommodations.json")));

            foreach (var item in doc.RootElement.GetProperty("accommodations").EnumerateArray())
            {
                string typeName = item.GetProperty("type").GetString()!;
                if (!types.TryGetValue(typeName, out var type))
                    throw new InvalidOperationException($"Onbekend accommodatietype '{typeName}' in accommodations.json.");

                string code = item.GetProperty("code").GetString()!;
                var accommodation = new Accommodation
                {
                    Id = SeedIds.Accommodation(code),
                    Code = code,
                    Name = item.GetProperty("name").GetString()!,
                    MaxPersons = item.GetProperty("maxPersons").GetInt32(),
                    Zone = item.GetProperty("zone").GetString()!,
                    AccommodationType = type,
                };

                foreach (var prop in item.GetProperty("properties").EnumerateObject())
                {
                    switch (prop.Name)
                    {
                        case "maxPets": accommodation.MaxPets = prop.Value.GetInt32(); break;
                        case "bedrooms": accommodation.Bedrooms = prop.Value.GetInt32(); break;
                        case "bathrooms": accommodation.Bathrooms = prop.Value.GetInt32(); break;
                        case "beds": accommodation.Beds = prop.Value.GetInt32(); break;
                        case "electricityAmps": accommodation.ElectricityAmps = prop.Value.GetInt32(); break;
                        case "maxVehicleLengthMeters": accommodation.MaxVehicleLengthMeters = prop.Value.GetDecimal(); break;
                        case "maxCaravanLengthMeters": accommodation.MaxCaravanLengthMeters = prop.Value.GetDecimal(); break;
                        case "surface": accommodation.Surface = Translate(prop.Value); break;
                        case "shade": accommodation.Shade = Translate(prop.Value); break;
                        case "wifiCoverage": accommodation.WifiCoverage = Translate(prop.Value); break;

                        default:
                            if (prop.Value.ValueKind == JsonValueKind.True)
                            {
                                if (!amenities.TryGetValue(prop.Name, out var amenity))
                                    throw new InvalidOperationException(
                                        $"Onbekende voorziening '{prop.Name}' in accommodations.json. Voeg hem toe aan amenities.json.");
                                accommodation.Amenities.Add(amenity);
                            }
                            break;
                    }
                }

                context.Add(accommodation);
            }

            context.SaveChanges();
        }

        private static void SeedPhotos(DbContext context, Dictionary<string, List<string>> photosPerType)
        {
            var types = context.Set<AccommodationType>().ToDictionary(t => t.Slug);
            string folder = Path.Combine(SeedFolder, "photos");

            foreach (var (slug, files) in photosPerType)
            {
                if (!types.TryGetValue(slug, out var type))
                    throw new InvalidOperationException($"Onbekend type '{slug}' in photos.json.");

                for (int i = 0; i < files.Count; i++)
                {
                    string path = Path.Combine(folder, files[i]);
                    if (!File.Exists(path))
                        throw new InvalidOperationException($"Foto '{files[i]}' uit photos.json staat niet in Data/Seed/photos.");

                    var photoId = SeedIds.Photo(slug, files[i]);
                    context.Add(new Photo
                    {
                        Id = photoId,
                        FileName = files[i],
                        ContentType = GetContentType(path),
                        SortOrder = i,
                        AccommodationType = type,
                        File = new PhotoFile { PhotoId = photoId, Data = File.ReadAllBytes(path) },
                    });
                }
            }

            context.SaveChanges();
        }

        private static string GetContentType(string path) => Path.GetExtension(path).ToLowerInvariant() switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".webp" => "image/webp",
            _ => throw new InvalidOperationException($"Onbekend afbeeldingsformaat: {Path.GetFileName(path)}"),
        };

        private static AccommodationCategory ParseCategory(string? category) => category switch
        {
            "holiday_home" => AccommodationCategory.HolidayHome,
            "camping_pitch" => AccommodationCategory.CampingPitch,
            _ => throw new InvalidOperationException($"Onbekende categorie '{category}' in types.json."),
        };
    }
}
