using VakantiePark.Shared.DTOs.Prices;

namespace VakantiePark.Application.Calculators
{
    public static class PriceCalculator
    {
        public static PriceDto Calculate(decimal pricePerNight, int nights, int persons, decimal cleaningFee, decimal touristTaxPerPersonPerNight)
        {
            if (nights < 1)
                throw new ArgumentException("Een verblijf duurt minimaal 1 nacht.", nameof(nights));
            if (persons < 1)
                throw new ArgumentException("Er moet minimaal 1 persoon meegaan.", nameof(persons));
            if (pricePerNight <= 0m)
                throw new ArgumentException("Voor dit type is nog geen prijs per nacht bekend.", nameof(pricePerNight));
            if (cleaningFee < 0m)
                throw new ArgumentException("De eindschoonmaak kan niet negatief zijn.", nameof(cleaningFee));
            if (touristTaxPerPersonPerNight < 0m)
                throw new ArgumentException("De toeristenbelasting kan niet negatief zijn.", nameof(touristTaxPerPersonPerNight));

            decimal stay = pricePerNight * nights;
            decimal touristTax = touristTaxPerPersonPerNight * persons * nights;

            return new PriceDto
            {
                Nights = nights,
                Persons = persons,
                PricePerNight = pricePerNight,
                Stay = stay,
                CleaningFee = cleaningFee,
                TouristTaxPerPersonPerNight = touristTaxPerPersonPerNight,
                TouristTax = touristTax,
                Total = stay + cleaningFee + touristTax,
            };
        }
    }
}
