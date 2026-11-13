using VakantiePark.Domain.Entities;
using VakantiePark.Domain.Enums;
using VakantiePark.Shared.DTOs.Prices;

namespace VakantiePark.Application.Calculators
{
    public static class PriceCalculator
    {
        public static PriceDto Calculate(
            AccommodationType type,
            DateOnly arrival,
            DateOnly departure,
            int persons,
            int youngChildren,
            int pets,
            ParkSettings settings)
        {
            var nights = departure.DayNumber - arrival.DayNumber;

            if (type.PricePerNight <= 0m)
            {
                throw new ArgumentException("Voor dit type is nog geen prijs per nacht bekend.", nameof(type));
            }

            if (settings.CleaningFee < 0m)
            {
                throw new ArgumentException("De eindschoonmaak kan niet negatief zijn.", nameof(settings));
            }

            if (settings.TouristTaxPerPersonPerNight < 0m)
            {
                throw new ArgumentException("De toeristenbelasting kan niet negatief zijn.", nameof(settings));
            }

            if (settings.WeekendSurchargePercentage < 0m)
            {
                throw new ArgumentException("De weekendtoeslag kan niet negatief zijn.", nameof(settings));
            }

            if (settings.PetFeePerNight < 0m)
            {
                throw new ArgumentException("De prijs per huisdier kan niet negatief zijn.", nameof(settings));
            }

            var weekendNights = CountWeekendNights(arrival, nights);

            var stay = type.PricePerNight * nights;
            var weekendSurcharge = Math.Round(
                type.PricePerNight * weekendNights * settings.WeekendSurchargePercentage / 100m,
                2,
                MidpointRounding.AwayFromZero);
            var petFee = settings.PetFeePerNight * pets * nights;
            var cleaningFee = type.Category == AccommodationCategory.HolidayHome ? settings.CleaningFee : 0m;
            var touristTax = settings.TouristTaxPerPersonPerNight * (persons - youngChildren) * nights;

            return new PriceDto
            {
                Nights = nights,
                Persons = persons,
                PricePerNight = type.PricePerNight,
                Stay = stay,
                WeekendNights = weekendNights,
                WeekendSurchargePercentage = settings.WeekendSurchargePercentage,
                WeekendSurcharge = weekendSurcharge,
                Pets = pets,
                PetFeePerNight = settings.PetFeePerNight,
                PetFee = petFee,
                CleaningFee = cleaningFee,
                YoungChildren = youngChildren,
                TouristTaxMinimumAge = settings.TouristTaxMinimumAge,
                TouristTaxPerPersonPerNight = settings.TouristTaxPerPersonPerNight,
                TouristTax = touristTax,
                Total = stay + weekendSurcharge + petFee + cleaningFee + touristTax,
            };
        }

        private static int CountWeekendNights(DateOnly arrival, int nights)
        {
            var count = 0;

            for (var i = 0; i < nights; i++)
            {
                var day = arrival.AddDays(i).DayOfWeek;
                if (day == DayOfWeek.Friday || day == DayOfWeek.Saturday)
                {
                    count++;
                }
            }

            return count;
        }
    }
}
