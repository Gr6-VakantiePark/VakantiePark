using VakantiePark.Application.Interfaces;
using VakantiePark.Shared.DTOs.Afbeeldingen;

namespace VakantiePark.Application.Services
{
    public class AfbeeldingService(IAfbeeldingRepository repository) : IAfbeeldingService
    {
        public async Task<AfbeeldingBestandDto> GetBestandAsync(int id)
        {
            var afbeelding = await repository.GetMetBestandAsync(id)
                ?? throw new KeyNotFoundException($"Afbeelding met id {id} is niet gevonden.");

            return new AfbeeldingBestandDto
            {
                Bestandsnaam = afbeelding.Bestandsnaam,
                ContentType = afbeelding.ContentType,
                Data = afbeelding.Bestand.Data,
            };
        }
    }
}
