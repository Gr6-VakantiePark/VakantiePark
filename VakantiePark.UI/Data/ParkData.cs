namespace VakantiePark.UI.Data;

public static class ParkData
{
    public static IReadOnlyList<AccommodationType> AccommodationTypes { get; } =
    [
        new(
            Slug: "berk-4",
            Name: "Berk 4",
            Category: AccommodationCategory.HolidayHome,
            Tagline: "Twee slaapkamers",
            Description: "Compact vakantiehuis voor vier personen, met twee slaapkamers. Fijn voor een stel of een klein gezin.",
            Persons: "4",
            UnitCount: 32,
            KeyFacts:
            [
                new("Personen", "4"),
                new("Slaapkamers", "2"),
                new("Badkamers", "1"),
                new("Bedden", "4"),
                new("Huisdieren", "max. 2")
            ],
            Amenities:
            [
                "Rookvrij"
            ],
            PartialAmenities:
            [
                new("Huisdieren toegestaan", 20),
                new("Rolstoeltoegankelijk", 2),
                new("Openhaard", 8),
                new("Vaatwasser", 23),
                new("Wasmachine", 10),
                new("Airconditioning", 7),
                new("Overdekt terras", 13),
                new("Laadpaal", 6),
                new("Aan het water", 10),
                new("Dicht bij het zwembad", 13),
                new("Kindvriendelijk", 20)
            ],
            Zones:
            [
                new("Parkcentrum", 10),
                new("Heideveld", 8),
                new("Rustweide", 6),
                new("Waterkant", 5),
                new("Bosrand", 3)
            ]),
        new(
            Slug: "wilg-4-mindervalide",
            Name: "Wilg 4 Mindervalide",
            Category: AccommodationCategory.HolidayHome,
            Tagline: "Rolstoeltoegankelijk",
            Description: "Gelijkvloers huis voor vier personen, drempelvrij, met verbrede deuren en een aangepaste badkamer.",
            Persons: "4",
            UnitCount: 12,
            KeyFacts:
            [
                new("Personen", "4"),
                new("Slaapkamers", "2"),
                new("Badkamers", "1"),
                new("Bedden", "4"),
                new("Huisdieren", "max. 2")
            ],
            Amenities:
            [
                "Rolstoeltoegankelijk",
                "Drempelvrij",
                "Verbrede deuren",
                "Aangepaste badkamer",
                "Vaatwasser",
                "Kindvriendelijk",
                "Rookvrij"
            ],
            PartialAmenities:
            [
                new("Huisdieren toegestaan", 5),
                new("Verstelbaar bed", 10),
                new("Wasmachine", 6),
                new("Airconditioning", 8),
                new("Overdekt terras", 10),
                new("Laadpaal", 2),
                new("Aan het water", 1),
                new("Dicht bij het zwembad", 8)
            ],
            Zones:
            [
                new("Bosrand", 4),
                new("Rustweide", 4),
                new("Waterkant", 2),
                new("Parkcentrum", 2)
            ]),
        new(
            Slug: "berk-6",
            Name: "Berk 6",
            Category: AccommodationCategory.HolidayHome,
            Tagline: "Drie slaapkamers",
            Description: "Ruim huis voor zes personen met drie slaapkamers en een vaatwasser in elk huis.",
            Persons: "6",
            UnitCount: 26,
            KeyFacts:
            [
                new("Personen", "6"),
                new("Slaapkamers", "3"),
                new("Badkamers", "1"),
                new("Bedden", "6"),
                new("Huisdieren", "max. 2")
            ],
            Amenities:
            [
                "Vaatwasser",
                "Rookvrij"
            ],
            PartialAmenities:
            [
                new("Huisdieren toegestaan", 12),
                new("Openhaard", 8),
                new("Wasmachine", 16),
                new("Airconditioning", 4),
                new("Overdekt terras", 13),
                new("Laadpaal", 7),
                new("Aan het water", 7),
                new("Dicht bij het zwembad", 8),
                new("Kindvriendelijk", 22)
            ],
            Zones:
            [
                new("Heideveld", 8),
                new("Waterkant", 6),
                new("Parkcentrum", 5),
                new("Rustweide", 4),
                new("Bosrand", 3)
            ]),
        new(
            Slug: "eik-6",
            Name: "Eik 6",
            Category: AccommodationCategory.HolidayHome,
            Tagline: "Drie slaapkamers, soms sauna",
            Description: "Huis voor zes personen met drie slaapkamers. Een deel van de huizen heeft twee badkamers of een sauna.",
            Persons: "6",
            UnitCount: 24,
            KeyFacts:
            [
                new("Personen", "6"),
                new("Slaapkamers", "3"),
                new("Badkamers", "1 – 2"),
                new("Bedden", "6"),
                new("Huisdieren", "max. 2")
            ],
            Amenities:
            [
                "Vaatwasser",
                "Rookvrij"
            ],
            PartialAmenities:
            [
                new("Huisdieren toegestaan", 14),
                new("Sauna", 7),
                new("Openhaard", 9),
                new("Wasmachine", 15),
                new("Airconditioning", 7),
                new("Overdekt terras", 11),
                new("Laadpaal", 9),
                new("Aan het water", 5),
                new("Dicht bij het zwembad", 13),
                new("Kindvriendelijk", 18)
            ],
            Zones:
            [
                new("Rustweide", 10),
                new("Heideveld", 4),
                new("Bosrand", 4),
                new("Waterkant", 3),
                new("Parkcentrum", 3)
            ]),
        new(
            Slug: "eik-luxe-6",
            Name: "Eik Luxe 6",
            Category: AccommodationCategory.HolidayHome,
            Tagline: "Sauna en twee badkamers",
            Description: "Luxe huis voor zes personen met twee badkamers, overdekt terras en bijna altijd een sauna.",
            Persons: "6",
            UnitCount: 18,
            KeyFacts:
            [
                new("Personen", "6"),
                new("Slaapkamers", "3"),
                new("Badkamers", "2"),
                new("Bedden", "6"),
                new("Huisdieren", "max. 2")
            ],
            Amenities:
            [
                "Vaatwasser",
                "Wasmachine",
                "Overdekt terras",
                "Rookvrij"
            ],
            PartialAmenities:
            [
                new("Huisdieren toegestaan", 8),
                new("Rolstoeltoegankelijk", 1),
                new("Sauna", 16),
                new("Hottub", 7),
                new("Openhaard", 14),
                new("Airconditioning", 15),
                new("Laadpaal", 12),
                new("Aan het water", 8),
                new("Dicht bij het zwembad", 7),
                new("Kindvriendelijk", 13)
            ],
            Zones:
            [
                new("Rustweide", 5),
                new("Waterkant", 5),
                new("Heideveld", 3),
                new("Parkcentrum", 3),
                new("Bosrand", 2)
            ]),
        new(
            Slug: "beuk-8",
            Name: "Beuk 8",
            Category: AccommodationCategory.HolidayHome,
            Tagline: "Vier slaapkamers",
            Description: "Groot huis voor acht personen met vier slaapkamers en twee of drie badkamers.",
            Persons: "8",
            UnitCount: 20,
            KeyFacts:
            [
                new("Personen", "8"),
                new("Slaapkamers", "4"),
                new("Badkamers", "2 – 3"),
                new("Bedden", "8"),
                new("Huisdieren", "max. 2")
            ],
            Amenities:
            [
                "Vaatwasser",
                "Rookvrij"
            ],
            PartialAmenities:
            [
                new("Huisdieren toegestaan", 6),
                new("Rolstoeltoegankelijk", 2),
                new("Sauna", 4),
                new("Openhaard", 10),
                new("Wasmachine", 17),
                new("Airconditioning", 10),
                new("Overdekt terras", 9),
                new("Laadpaal", 7),
                new("Aan het water", 6),
                new("Dicht bij het zwembad", 5),
                new("Kindvriendelijk", 16)
            ],
            Zones:
            [
                new("Bosrand", 6),
                new("Heideveld", 6),
                new("Parkcentrum", 4),
                new("Waterkant", 2),
                new("Rustweide", 2)
            ]),
        new(
            Slug: "bosvilla-10",
            Name: "Bosvilla 10",
            Category: AccommodationCategory.HolidayHome,
            Tagline: "Vijf slaapkamers en openhaard",
            Description: "Villa voor tien personen met vijf slaapkamers, openhaard en in de meeste gevallen een sauna.",
            Persons: "10",
            UnitCount: 18,
            KeyFacts:
            [
                new("Personen", "10"),
                new("Slaapkamers", "5"),
                new("Badkamers", "2 – 3"),
                new("Bedden", "10"),
                new("Huisdieren", "max. 2")
            ],
            Amenities:
            [
                "Vaatwasser",
                "Wasmachine",
                "Kindvriendelijk",
                "Rookvrij"
            ],
            PartialAmenities:
            [
                new("Huisdieren toegestaan", 12),
                new("Sauna", 14),
                new("Hottub", 4),
                new("Openhaard", 16),
                new("Airconditioning", 13),
                new("Overdekt terras", 16),
                new("Laadpaal", 9),
                new("Extra parkeerplaats", 14),
                new("Aan het water", 8),
                new("Dicht bij het zwembad", 7)
            ],
            Zones:
            [
                new("Rustweide", 8),
                new("Bosrand", 5),
                new("Heideveld", 2),
                new("Parkcentrum", 2),
                new("Waterkant", 1)
            ]),
        new(
            Slug: "tentplaats-basis",
            Name: "Tentplaats Basis",
            Category: AccommodationCategory.CampingPitch,
            Tagline: "Gras, zonder aansluitingen",
            Description: "Grasveld voor je tent, zonder aansluitingen. Een deel van de plaatsen heeft stroom.",
            Persons: "4 – 6",
            UnitCount: 12,
            KeyFacts:
            [
                new("Personen", "4 – 6"),
                new("Ondergrond", "Gras"),
                new("Stroom", "6 A – 10 A"),
                new("Huisdieren", "max. 2")
            ],
            Amenities:
            [
                "Basisuitrusting"
            ],
            PartialAmenities:
            [
                new("Huisdieren toegestaan", 11),
                new("Stroomaansluiting", 6),
                new("Auto bij de plaats", 5),
                new("Dicht bij sanitair", 4)
            ],
            Zones:
            [
                new("Waterweide", 4),
                new("Bosveld", 3),
                new("Kampeerveld Noord", 3),
                new("Kampeerveld Zuid", 2)
            ]),
        new(
            Slug: "tentplaats-comfort",
            Name: "Tentplaats Comfort",
            Category: AccommodationCategory.CampingPitch,
            Tagline: "Met stroom",
            Description: "Tentplaats met stroom en meestal een wateraansluiting. Enkele plaatsen hebben eigen sanitair.",
            Persons: "4 – 6",
            UnitCount: 10,
            KeyFacts:
            [
                new("Personen", "4 – 6"),
                new("Ondergrond", "Gras"),
                new("Stroom", "10 A – 16 A"),
                new("Huisdieren", "max. 2")
            ],
            Amenities:
            [
                "Stroomaansluiting"
            ],
            PartialAmenities:
            [
                new("Huisdieren toegestaan", 4),
                new("Wateraansluiting", 8),
                new("Eigen sanitair", 3),
                new("Auto bij de plaats", 8),
                new("Dicht bij sanitair", 5)
            ],
            Zones:
            [
                new("Kampeerveld Noord", 4),
                new("Waterweide", 4),
                new("Bosveld", 2)
            ]),
        new(
            Slug: "caravanplaats",
            Name: "Caravanplaats",
            Category: AccommodationCategory.CampingPitch,
            Tagline: "Stroom, meestal water",
            Description: "Plaats voor caravan en auto, met stroom. De meeste plaatsen hebben water en afvoer.",
            Persons: "6",
            UnitCount: 12,
            KeyFacts:
            [
                new("Personen", "6"),
                new("Ondergrond", "Gras, Versterkt gras"),
                new("Stroom", "10 A – 16 A"),
                new("Max. lengte", "6 m – 8 m"),
                new("Huisdieren", "max. 2")
            ],
            Amenities:
            [
                "Stroomaansluiting",
                "Auto bij de plaats"
            ],
            PartialAmenities:
            [
                new("Huisdieren toegestaan", 10),
                new("Wateraansluiting", 10),
                new("Afvoer", 7),
                new("Eigen sanitair", 3),
                new("Dicht bij sanitair", 10)
            ],
            Zones:
            [
                new("Bosveld", 4),
                new("Kampeerveld Zuid", 4),
                new("Kampeerveld Noord", 4)
            ]),
        new(
            Slug: "camperplaats",
            Name: "Camperplaats",
            Category: AccommodationCategory.CampingPitch,
            Tagline: "Verhard, water en afvoer",
            Description: "Verharde plaats voor campers tot ongeveer 8,5 meter, met stroom, water en afvoer.",
            Persons: "6",
            UnitCount: 10,
            KeyFacts:
            [
                new("Personen", "6"),
                new("Ondergrond", "Verhard, Versterkt gras"),
                new("Stroom", "10 A – 16 A"),
                new("Max. lengte", "7 m – 8,5 m"),
                new("Huisdieren", "max. 2")
            ],
            Amenities:
            [
                "Stroomaansluiting",
                "Wateraansluiting",
                "Auto bij de plaats"
            ],
            PartialAmenities:
            [
                new("Huisdieren toegestaan", 8),
                new("Afvoer", 9),
                new("Dicht bij sanitair", 2)
            ],
            Zones:
            [
                new("Kampeerveld Noord", 5),
                new("Kampeerveld Zuid", 3),
                new("Bosveld", 2)
            ]),
        new(
            Slug: "camperplaats-xl",
            Name: "Camperplaats XL",
            Category: AccommodationCategory.CampingPitch,
            Tagline: "Extra breed, tot 11 meter",
            Description: "Extra brede, verharde plaats voor grote campers tot 11 meter, met 16 ampère stroom.",
            Persons: "6",
            UnitCount: 6,
            KeyFacts:
            [
                new("Personen", "6"),
                new("Ondergrond", "Verhard"),
                new("Stroom", "16 A"),
                new("Max. lengte", "9 m – 11 m"),
                new("Huisdieren", "max. 2")
            ],
            Amenities:
            [
                "Stroomaansluiting",
                "Wateraansluiting",
                "Afvoer",
                "Auto bij de plaats",
                "Extra brede plaats"
            ],
            PartialAmenities:
            [
                new("Huisdieren toegestaan", 4),
                new("Eigen sanitair", 1),
                new("Dicht bij sanitair", 4)
            ],
            Zones:
            [
                new("Kampeerveld Noord", 3),
                new("Bosveld", 1),
                new("Waterweide", 1),
                new("Kampeerveld Zuid", 1)
            ])
    ];

    public static IReadOnlyList<Zone> Zones { get; } =
    [
        new("Bosrand", AccommodationCategory.HolidayHome, 27),
        new("Heideveld", AccommodationCategory.HolidayHome, 31),
        new("Parkcentrum", AccommodationCategory.HolidayHome, 29),
        new("Rustweide", AccommodationCategory.HolidayHome, 39),
        new("Waterkant", AccommodationCategory.HolidayHome, 24),
        new("Bosveld", AccommodationCategory.CampingPitch, 12),
        new("Kampeerveld Noord", AccommodationCategory.CampingPitch, 19),
        new("Kampeerveld Zuid", AccommodationCategory.CampingPitch, 10),
        new("Waterweide", AccommodationCategory.CampingPitch, 9)
    ];

    public static IEnumerable<AccommodationType> GetByCategory(AccommodationCategory category) =>
        AccommodationTypes.Where(type => type.Category == category);

    public static AccommodationType? GetBySlug(string slug) =>
        AccommodationTypes.FirstOrDefault(type => type.Slug.Equals(slug, StringComparison.OrdinalIgnoreCase));
}
