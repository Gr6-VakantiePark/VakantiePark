using Moq;
using VakantiePark.Application.Services;
using VakantiePark.Domain.Entities;
using VakantiePark.Domain.Enums;
using VakantiePark.Domain.Interfaces;
using VakantiePark.Shared.DTOs.Accommodations;

namespace VakantiePark.UnitTests.Services
{
    public class AccommodationServiceTests
    {
        private readonly Mock<IAccommodationRepository> _accommodationRepository = new();
        private readonly Mock<IParkSettingsRepository> _settingsRepository = new();
        private readonly AccommodationService _sut;

        private static readonly Guid AccommodationId = Guid.NewGuid();
        private static readonly DateOnly Arrival = new(2030, 7, 1);
        private static readonly DateOnly Departure = new(2030, 7, 4);

        public AccommodationServiceTests()
        {
            _sut = new AccommodationService(_accommodationRepository.Object, _settingsRepository.Object);
            _settingsRepository.Setup(r => r.GetAsync()).ReturnsAsync(new ParkSettings
            {
                CleaningFee = 75m,
                TouristTaxPerPersonPerNight = 2.50m,
                TouristTaxMinimumAge = 3,
                WeekendSurchargePercentage = 10m,
                PetFeePerNight = 10m,
                ArrivalTime = new TimeOnly(15, 0),
                DepartureTime = new TimeOnly(10, 0),
                MinimumNightsHighSeason = 7,
            });
        }

        private static Accommodation CreateAccommodation(decimal pricePerNight = 100m) => new()
        {
            Id = AccommodationId,
            Code = "B4-01",
            Name = "Berk 4 nr. 1",
            MaxPersons = 4,
            MaxPets = 2,
            AccommodationType = new AccommodationType { Name = "Berk 4", Slug = "berk-4", Category = AccommodationCategory.HolidayHome, PricePerNight = pricePerNight },
        };

        [Fact]
        public async Task GetByIdAsync_WithStay_ReturnsPrice()
        {
            _accommodationRepository.Setup(r => r.GetByIdAsync(AccommodationId)).ReturnsAsync(CreateAccommodation());

            var dto = await _sut.GetByIdAsync(AccommodationId, Arrival, Departure, 2);

            Assert.NotNull(dto.Price);
            Assert.Equal(390m, dto.Price.Total);
        }

        [Fact]
        public async Task GetByIdAsync_WithPetsAndYoungChildren_ReturnsPriceWithRules()
        {
            _accommodationRepository.Setup(r => r.GetByIdAsync(AccommodationId)).ReturnsAsync(CreateAccommodation());

            var dto = await _sut.GetByIdAsync(AccommodationId, Arrival, Departure, 3, 1, 1);

            Assert.NotNull(dto.Price);
            Assert.Equal(30m, dto.Price.PetFee);
            Assert.Equal(15m, dto.Price.TouristTax);
            Assert.Equal(420m, dto.Price.Total);
        }

        [Fact]
        public async Task GetByIdAsync_TooManyPets_ThrowsArgumentException()
        {
            _accommodationRepository.Setup(r => r.GetByIdAsync(AccommodationId)).ReturnsAsync(CreateAccommodation());

            await Assert.ThrowsAsync<ArgumentException>(() => _sut.GetByIdAsync(AccommodationId, Arrival, Departure, 2, 0, 3));
        }

        [Fact]
        public async Task GetByIdAsync_WithoutStay_ReturnsNoPrice()
        {
            _accommodationRepository.Setup(r => r.GetByIdAsync(AccommodationId)).ReturnsAsync(CreateAccommodation());

            var dto = await _sut.GetByIdAsync(AccommodationId);

            Assert.Null(dto.Price);
        }

        [Fact]
        public async Task GetByIdAsync_TooManyPersons_ThrowsArgumentException()
        {
            _accommodationRepository.Setup(r => r.GetByIdAsync(AccommodationId)).ReturnsAsync(CreateAccommodation());

            await Assert.ThrowsAsync<ArgumentException>(() => _sut.GetByIdAsync(AccommodationId, Arrival, Departure, 5));
        }

        [Fact]
        public async Task GetByIdAsync_IncompleteStay_ThrowsArgumentException()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => _sut.GetByIdAsync(AccommodationId, Arrival, null, 2));
            _accommodationRepository.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        }

        [Fact]
        public async Task GetByIdAsync_NotFound_ThrowsKeyNotFoundException()
        {
            _accommodationRepository.Setup(r => r.GetByIdAsync(AccommodationId)).ReturnsAsync((Accommodation?)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _sut.GetByIdAsync(AccommodationId, Arrival, Departure, 2));
        }

        [Fact]
        public async Task SearchAsync_TypeWithoutPrice_ReturnsNoPrice()
        {
            var criteria = new AccommodationSearchCriteria { Arrival = Arrival, Departure = Departure, Persons = 2 };
            _accommodationRepository.Setup(r => r.SearchAsync(criteria)).ReturnsAsync(([CreateAccommodation(0m)], 1));

            var result = await _sut.SearchAsync(criteria);

            Assert.Null(result.Items[0].Price);
        }

        [Fact]
        public async Task SearchAsync_WithResults_ReturnsPricePerAccommodation()
        {
            var criteria = new AccommodationSearchCriteria { Arrival = Arrival, Departure = Departure, Persons = 2 };
            _accommodationRepository.Setup(r => r.SearchAsync(criteria)).ReturnsAsync(([CreateAccommodation()], 1));

            var result = await _sut.SearchAsync(criteria);

            Assert.Equal(390m, result.Items[0].Price!.Total);
        }

        private static AccommodationType CreateTypeWithAccommodations()
        {
            var sauna = new Amenity { Id = Guid.NewGuid(), Code = "sauna", Name = "Sauna" };
            var smokeFree = new Amenity { Id = Guid.NewGuid(), Code = "smokeFree", Name = "Rookvrij" };
            var type = new AccommodationType
            {
                Name = "Eik 6",
                Slug = "eik-6",
                Category = AccommodationCategory.HolidayHome,
                Tagline = "Drie slaapkamers",
                Description = "Huis voor zes personen.",
            };

            type.Accommodations.Add(new Accommodation { Code = "E6-01", MaxPersons = 6, Zone = "Rustweide", Bedrooms = 3, Bathrooms = 1, Beds = 6, MaxPets = 0, Amenities = { sauna, smokeFree } });
            type.Accommodations.Add(new Accommodation { Code = "E6-02", MaxPersons = 6, Zone = "Rustweide", Bedrooms = 3, Bathrooms = 2, Beds = 6, MaxPets = 2, Amenities = { smokeFree } });
            type.Accommodations.Add(new Accommodation { Code = "E6-03", MaxPersons = 4, Zone = "Bosrand", Bedrooms = 2, Bathrooms = 1, Beds = 4, MaxPets = 1, Amenities = { smokeFree } });
            return type;
        }

        [Fact]
        public async Task GetTypesAsync_ReturnsUnitCountAndPersonRange()
        {
            _accommodationRepository.Setup(r => r.GetTypesWithAccommodationsAsync()).ReturnsAsync([CreateTypeWithAccommodations()]);

            var types = await _sut.GetTypesAsync();

            var type = Assert.Single(types);
            Assert.Equal("eik-6", type.Slug);
            Assert.Equal("Drie slaapkamers", type.Tagline);
            Assert.Equal(3, type.UnitCount);
            Assert.Equal(4, type.MinPersons);
            Assert.Equal(6, type.MaxPersons);
        }

        [Fact]
        public async Task GetTypeBySlugAsync_ReturnsRangesAmenitiesAndZones()
        {
            _accommodationRepository.Setup(r => r.GetTypeBySlugAsync("eik-6")).ReturnsAsync(CreateTypeWithAccommodations());

            var type = await _sut.GetTypeBySlugAsync(" eik-6 ");

            Assert.Equal("Huis voor zes personen.", type.Description);
            Assert.Equal(2, type.MinBedrooms);
            Assert.Equal(3, type.MaxBedrooms);
            Assert.Equal(1, type.MinBathrooms);
            Assert.Equal(2, type.MaxBathrooms);
            Assert.Equal(2, type.MaxPets);
            Assert.Equal(3, type.Amenities.Single(a => a.Code == "smokeFree").UnitCount);
            Assert.Equal(1, type.Amenities.Single(a => a.Code == "sauna").UnitCount);
            Assert.Equal(2, type.Zones.Single(z => z.Name == "Rustweide").UnitCount);
            Assert.Equal(1, type.Zones.Single(z => z.Name == "Bosrand").UnitCount);
        }

        [Fact]
        public async Task GetTypeBySlugAsync_EmptySlug_ThrowsArgumentException()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => _sut.GetTypeBySlugAsync(" "));
        }

        [Fact]
        public async Task GetTypeBySlugAsync_NotFound_ThrowsKeyNotFoundException()
        {
            _accommodationRepository.Setup(r => r.GetTypeBySlugAsync("onbekend")).ReturnsAsync((AccommodationType?)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _sut.GetTypeBySlugAsync("onbekend"));
        }

        [Fact]
        public async Task GetZonesAsync_GroupsAccommodationsPerZone()
        {
            _accommodationRepository.Setup(r => r.GetTypesWithAccommodationsAsync()).ReturnsAsync([CreateTypeWithAccommodations()]);

            var zones = await _sut.GetZonesAsync();

            Assert.Equal(2, zones.Count);
            Assert.Equal("Bosrand", zones[0].Name);
            Assert.Equal(1, zones[0].UnitCount);
            Assert.Equal("Rustweide", zones[1].Name);
            Assert.Equal(2, zones[1].UnitCount);
            Assert.All(zones, z => Assert.Equal("HolidayHome", z.Category));
        }
    }
}
