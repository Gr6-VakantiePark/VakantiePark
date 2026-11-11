using Moq;
using VakantiePark.Application.Interfaces;
using VakantiePark.Application.Services;
using VakantiePark.Domain.Entities;
using VakantiePark.Shared.DTOs.Prices;

namespace VakantiePark.UnitTests.Services
{
    public class PriceServiceTests
    {
        private readonly Mock<IAccommodationRepository> _accommodationRepository = new();
        private readonly Mock<IParkSettingsRepository> _settingsRepository = new();
        private readonly PriceService _sut;

        public PriceServiceTests()
        {
            _sut = new PriceService(_accommodationRepository.Object, _settingsRepository.Object);
        }

        private static AccommodationType CreateType(decimal pricePerNight = 100m, int maxPersons = 4) => new()
        {
            Name = "Berk 4",
            Slug = "berk-4",
            PricePerNight = pricePerNight,
            Accommodations = [new Accommodation { MaxPersons = maxPersons }],
        };

        private static ParkSettings CreateSettings() => new()
        {
            CleaningFee = 75m,
            TouristTaxPerPersonPerNight = 2.50m,
            ArrivalTime = new TimeOnly(15, 0),
            DepartureTime = new TimeOnly(10, 0),
        };

        private static PriceRequestDto CreateRequest(int persons = 2) => new()
        {
            Type = "berk-4",
            Arrival = new DateOnly(2027, 7, 1),
            Departure = new DateOnly(2027, 7, 4),
            Persons = persons,
        };

        [Fact]
        public async Task CalculateAsync_ValidRequest_ReturnsPriceBreakdown()
        {
            _accommodationRepository.Setup(r => r.GetTypeBySlugAsync("berk-4")).ReturnsAsync(CreateType());
            _settingsRepository.Setup(r => r.GetAsync()).ReturnsAsync(CreateSettings());

            var price = await _sut.CalculateAsync(CreateRequest());

            Assert.Equal(3, price.Nights);
            Assert.Equal(300m, price.Stay);
            Assert.Equal(15m, price.TouristTax);
            Assert.Equal(390m, price.Total);
        }

        [Fact]
        public async Task CalculateAsync_TypeNotFound_ThrowsKeyNotFoundException()
        {
            _accommodationRepository.Setup(r => r.GetTypeBySlugAsync("berk-4")).ReturnsAsync((AccommodationType?)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _sut.CalculateAsync(CreateRequest()));
        }

        [Fact]
        public async Task CalculateAsync_SettingsNotFound_ThrowsKeyNotFoundException()
        {
            _accommodationRepository.Setup(r => r.GetTypeBySlugAsync("berk-4")).ReturnsAsync(CreateType());
            _settingsRepository.Setup(r => r.GetAsync()).ReturnsAsync((ParkSettings?)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _sut.CalculateAsync(CreateRequest()));
        }

        [Fact]
        public async Task CalculateAsync_TooManyPersons_ThrowsArgumentException()
        {
            _accommodationRepository.Setup(r => r.GetTypeBySlugAsync("berk-4")).ReturnsAsync(CreateType(maxPersons: 4));
            _settingsRepository.Setup(r => r.GetAsync()).ReturnsAsync(CreateSettings());

            await Assert.ThrowsAsync<ArgumentException>(() => _sut.CalculateAsync(CreateRequest(persons: 5)));
        }

        [Fact]
        public async Task CalculateAsync_DepartureNotAfterArrival_ThrowsArgumentException()
        {
            var request = CreateRequest();
            request.Departure = request.Arrival;

            await Assert.ThrowsAsync<ArgumentException>(() => _sut.CalculateAsync(request));
            _accommodationRepository.Verify(r => r.GetTypeBySlugAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task CalculateAsync_NoType_ThrowsArgumentException()
        {
            var request = CreateRequest();
            request.Type = " ";

            await Assert.ThrowsAsync<ArgumentException>(() => _sut.CalculateAsync(request));
        }

        [Fact]
        public async Task CalculateAsync_TypeWithoutPrice_ThrowsArgumentException()
        {
            _accommodationRepository.Setup(r => r.GetTypeBySlugAsync("berk-4")).ReturnsAsync(CreateType(pricePerNight: 0m));
            _settingsRepository.Setup(r => r.GetAsync()).ReturnsAsync(CreateSettings());

            await Assert.ThrowsAsync<ArgumentException>(() => _sut.CalculateAsync(CreateRequest()));
        }
    }
}
