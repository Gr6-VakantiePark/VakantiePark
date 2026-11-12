using VakantiePark.Domain.Enums;
using VakantiePark.Shared.Common;
using VakantiePark.Shared.Common.Pagination;
using VakantiePark.Shared.DTOs.Accommodations;

namespace VakantiePark.Application.Extensions.Accommodations
{
    public static class AccommodationValidation
    {
        public static void Validate(this AccommodationSearchCriteria? criteria)
        {
            if (criteria is null)
                throw new ArgumentException("De zoekcriteria ontbreken.");

            ValidatePeriod(criteria.Arrival, criteria.Departure);

            if (criteria.Arrival < DateOnly.FromDateTime(DateTime.Today))
                throw new ArgumentException("De aankomstdatum mag niet in het verleden liggen.");

            ValidatePersons(criteria.Persons);

            if (criteria.Pets < 0)
                throw new ArgumentException("Het aantal huisdieren kan niet negatief zijn.");
            if (criteria.Pets > FieldLimits.MaxPets)
                throw new ArgumentException($"Het aantal huisdieren mag maximaal {FieldLimits.MaxPets} zijn.");

            if (criteria.PageNumber < 1)
                throw new ArgumentException("Het paginanummer moet minimaal 1 zijn.");

            if (criteria.PageSize < 1 || criteria.PageSize > PaginationBase.MaxPageSize)
                throw new ArgumentException($"De paginagrootte moet tussen 1 en {PaginationBase.MaxPageSize} liggen.");

            if (!Enum.IsDefined(criteria.OrderBy))
                throw new ArgumentException($"Op '{criteria.OrderBy}' kan niet gesorteerd worden.");

            if (!Enum.IsDefined(criteria.OrderType))
                throw new ArgumentException($"Onbekende sorteerrichting '{criteria.OrderType}'.");

            if (!string.IsNullOrWhiteSpace(criteria.Category)
                && (!Enum.TryParse<AccommodationCategory>(criteria.Category, ignoreCase: true, out var category) || !Enum.IsDefined(category)))
                throw new ArgumentException($"Onbekende soort verblijf '{criteria.Category}'.");

            if (criteria.Type is { Length: > FieldLimits.SlugMaxLength })
                throw new ArgumentException($"Het type mag maximaal {FieldLimits.SlugMaxLength} tekens hebben.");

            if (criteria.Amenities is { Length: > FieldLimits.AmenitiesMaxLength })
                throw new ArgumentException($"De lijst met voorzieningen mag maximaal {FieldLimits.AmenitiesMaxLength} tekens hebben.");
        }

        public static void ValidateStay(DateOnly? arrival, DateOnly? departure, int? persons)
        {
            if (arrival is null || departure is null || persons is null)
                throw new ArgumentException("Geef aankomst, vertrek en het aantal personen samen op om een prijs te berekenen.");

            ValidatePeriod(arrival.Value, departure.Value);
            ValidatePersons(persons.Value);
        }

        internal static void ValidatePeriod(DateOnly arrival, DateOnly departure)
        {
            if (arrival == default || departure == default)
                throw new ArgumentException("Vul een aankomst- en vertrekdatum in.");

            if (departure <= arrival)
                throw new ArgumentException("De vertrekdatum moet na de aankomstdatum liggen.");

            if (departure.DayNumber - arrival.DayNumber > FieldLimits.MaxNights)
                throw new ArgumentException($"Een verblijf mag maximaal {FieldLimits.MaxNights} nachten duren.");
        }

        internal static void ValidatePersons(int persons)
        {
            if (persons < 1)
                throw new ArgumentException("Het aantal personen moet minimaal 1 zijn.");

            if (persons > FieldLimits.MaxPersons)
                throw new ArgumentException($"Het aantal personen mag maximaal {FieldLimits.MaxPersons} zijn.");
        }
    }
}
