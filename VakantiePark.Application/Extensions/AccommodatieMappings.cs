using VakantiePark.Domain.Entities;
using VakantiePark.Shared.DTOs.Accommodaties;

namespace VakantiePark.Application.Extensions
{
    public static class AccommodatieMappings
    {
        public static AccommodatieDto ToDto(this Accommodatie a) => new()
        {
            Id = a.Id,
            Code = a.Code,
            Naam = a.Naam,
            Type = a.AccommodatieType.Naam,
            TypeSlug = a.AccommodatieType.Slug,
            Categorie = a.AccommodatieType.Categorie.ToString(),
            MaxPersonen = a.MaxPersonen,
            Zone = a.Zone,
            PrijsPerNacht = a.AccommodatieType.PrijsPerNacht,
            Hoofdafbeelding = AfbeeldingUrls(a.AccommodatieType).FirstOrDefault(),
        };

        public static AccommodatieDetailDto ToDetailDto(this Accommodatie a) => new()
        {
            Id = a.Id,
            Code = a.Code,
            Naam = a.Naam,
            Type = a.AccommodatieType.Naam,
            TypeSlug = a.AccommodatieType.Slug,
            Categorie = a.AccommodatieType.Categorie.ToString(),
            MaxPersonen = a.MaxPersonen,
            Zone = a.Zone,
            PrijsPerNacht = a.AccommodatieType.PrijsPerNacht,
            Hoofdafbeelding = AfbeeldingUrls(a.AccommodatieType).FirstOrDefault(),

            Omschrijving = a.AccommodatieType.Omschrijving,
            MaxHuisdieren = a.MaxHuisdieren,
            Slaapkamers = a.Slaapkamers,
            Badkamers = a.Badkamers,
            Bedden = a.Bedden,
            ElektriciteitAmpere = a.ElektriciteitAmpere,
            MaxVoertuigLengteMeters = a.MaxVoertuigLengteMeters,
            MaxCaravanLengteMeters = a.MaxCaravanLengteMeters,
            Ondergrond = a.Ondergrond,
            Schaduw = a.Schaduw,
            WifiDekking = a.WifiDekking,
            Afbeeldingen = AfbeeldingUrls(a.AccommodatieType),
            Voorzieningen = a.Voorzieningen
                .OrderBy(v => v.Naam)
                .Select(v => new VoorzieningDto { Id = v.Id, Code = v.Code, Naam = v.Naam })
                .ToList(),
        };

        private static List<string> AfbeeldingUrls(AccommodatieType type) =>
            type.Afbeeldingen
                .OrderBy(f => f.Volgorde)
                .Select(f => $"afbeeldingen/{f.Id}")
                .ToList();
    }
}
