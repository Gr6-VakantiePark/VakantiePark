using Moq;
using VakantiePark.Application.Services;
using VakantiePark.Domain.Entities;
using VakantiePark.Domain.Enums;
using VakantiePark.Domain.Interfaces;

namespace VakantiePark.UnitTests.Services
{
    public class ParkServiceTests
    {
        private readonly Mock<IParkSettingsRepository> _settingsRepository = new();
        private readonly Mock<IAccommodationRepository> _accommodationRepository = new();
        private readonly ParkService _sut;

        public ParkServiceTests()
        {
            _sut = new ParkService(_settingsRepository.Object, _accommodationRepository.Object);
        }

        [Fact]
        public async Task GetInfoAsync_ReturnsContactTimesHoursAndBasins()
        {
            _settingsRepository.Setup(r => r.GetAsync()).ReturnsAsync(new ParkSettings
            {
                ArrivalTime = new TimeOnly(15, 0),
                DepartureTime = new TimeOnly(10, 0),
                Street = "Parkweg 1",
                PostalCode = "1234 AB",
                City = "Voorbeeldstad",
                PhoneNumber = "000 - 000 00 00",
                Email = "info@dropuut.example",
            });
            _settingsRepository.Setup(r => r.GetOpeningHoursAsync()).ReturnsAsync(
            [
                new OpeningHour { Facility = Facility.Reception, Label = "Dagelijks", Value = "08:30 – 18:00" },
                new OpeningHour { Facility = Facility.Pool, Label = "Buitenbad", Value = "Mei tot en met september", ShowOnHome = true },
            ]);
            _settingsRepository.Setup(r => r.GetPoolBasinsAsync()).ReturnsAsync(
            [
                new PoolBasin { Name = "Wedstrijdbad", Description = "Voor banenzwemmen.", Size = "25 meter" },
            ]);
            _accommodationRepository.Setup(r => r.CountWithAmenityAsync(ParkService.NearPoolAmenityCode)).ReturnsAsync(61);

            var info = await _sut.GetInfoAsync();

            Assert.Equal("15:00", info.ArrivalTime);
            Assert.Equal("10:00", info.DepartureTime);
            Assert.Equal("Parkweg 1", info.Street);
            Assert.Equal("info@dropuut.example", info.Email);
            Assert.Equal(61, info.AccommodationsNearPool);
            Assert.Equal("Dagelijks", Assert.Single(info.ReceptionHours).Label);
            Assert.True(Assert.Single(info.PoolHours).ShowOnHome);
            Assert.Equal("Wedstrijdbad", Assert.Single(info.PoolBasins).Name);
        }

        [Fact]
        public async Task GetInfoAsync_WithoutSettings_ThrowsKeyNotFoundException()
        {
            _settingsRepository.Setup(r => r.GetAsync()).ReturnsAsync((ParkSettings?)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _sut.GetInfoAsync());
        }
    }
}
