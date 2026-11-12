using Moq;
using VakantiePark.Application.Services;
using VakantiePark.Domain.Entities;
using VakantiePark.Domain.Interfaces;
using VakantiePark.Shared.DTOs.Reservations;

namespace VakantiePark.UnitTests.Services
{
    public class AvailabilityServiceTests
    {
        private static readonly Guid AccommodationId = Guid.NewGuid();

        private readonly Mock<IReservationRepository> _reservationRepository = new();
        private readonly Mock<IUnrentablePeriodRepository> _unrentablePeriodRepository = new();
        private readonly Mock<IAccommodationRepository> _accommodationRepository = new();
        private readonly AvailabilityService _sut;

        public AvailabilityServiceTests()
        {
            _sut = new AvailabilityService(_reservationRepository.Object, _unrentablePeriodRepository.Object, _accommodationRepository.Object);

            _accommodationRepository.Setup(r => r.GetByIdAsync(AccommodationId))
                .ReturnsAsync(new Accommodation { Id = AccommodationId, MaxPersons = 4 });
            _reservationRepository.Setup(r => r.GetActiveAndFutureReservationsForAccomodation(It.IsAny<ReservationAvailabilityRequest>()))
                .ReturnsAsync([]);
            _unrentablePeriodRepository.Setup(r => r.GetActiveAndFuturePeriodsForAccomodation(It.IsAny<ReservationAvailabilityRequest>()))
                .ReturnsAsync([]);
        }

        // Relative to now so the tests don't start failing once a fixed date has passed.
        private static ReservationAvailabilityRequest CreateRequest(int persons = 2, Guid? accommodationId = null, int startInDays = 30, int nights = 3)
        {
            DateTime start = DateTime.UtcNow.Date.AddDays(startInDays).AddHours(15);
            return new ReservationAvailabilityRequest(start, start.AddDays(nights).AddHours(-5), accommodationId ?? AccommodationId, persons);
        }

        [Fact]
        public async Task CheckForPeriodAvailability_NoOverlaps_ReturnsTrue()
        {
            bool available = await _sut.CheckForPeriodAvailability(CreateRequest());

            Assert.True(available);
        }

        [Fact]
        public async Task CheckForPeriodAvailability_PassesRequestToRepositories()
        {
            var request = CreateRequest();

            await _sut.CheckForPeriodAvailability(request);

            _reservationRepository.Verify(r => r.GetActiveAndFutureReservationsForAccomodation(request), Times.Once);
            _unrentablePeriodRepository.Verify(r => r.GetActiveAndFuturePeriodsForAccomodation(request), Times.Once);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(4)]
        public async Task CheckForPeriodAvailability_PersonsWithinCapacity_ReturnsTrue(int persons)
        {
            Assert.True(await _sut.CheckForPeriodAvailability(CreateRequest(persons)));
        }

        [Fact]
        public async Task CheckForPeriodAvailability_NullRequest_ThrowsArgumentNullException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.CheckForPeriodAvailability(null!));
        }

        [Fact]
        public async Task CheckForPeriodAvailability_AccommodationNotFound_ThrowsKeyNotFoundException()
        {
            var unknownId = Guid.NewGuid();
            _accommodationRepository.Setup(r => r.GetByIdAsync(unknownId)).ReturnsAsync((Accommodation?)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _sut.CheckForPeriodAvailability(CreateRequest(accommodationId: unknownId)));
        }

        [Fact]
        public async Task CheckForPeriodAvailability_EmptyAccommodationId_ThrowsKeyNotFoundException()
        {
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _sut.CheckForPeriodAvailability(CreateRequest(accommodationId: Guid.Empty)));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(5)]
        public async Task CheckForPeriodAvailability_PersonsOutsideCapacity_ThrowsArgumentException(int persons)
        {
            await Assert.ThrowsAsync<ArgumentException>(() => _sut.CheckForPeriodAvailability(CreateRequest(persons)));
        }

        [Fact]
        public async Task CheckForPeriodAvailability_StartsSoonAndShorterThanOneDay_ThrowsArgumentException()
        {
            DateTime start = DateTime.UtcNow.AddHours(1);
            var request = new ReservationAvailabilityRequest(start, start.AddHours(2), AccommodationId, 2);

            await Assert.ThrowsAsync<ArgumentException>(() => _sut.CheckForPeriodAvailability(request));
        }

        [Fact]
        public async Task CheckForPeriodAvailability_OverlappingReservation_ThrowsArgumentException()
        {
            _reservationRepository.Setup(r => r.GetActiveAndFutureReservationsForAccomodation(It.IsAny<ReservationAvailabilityRequest>()))
                .ReturnsAsync([new Reservation { AccommodationId = AccommodationId }]);

            await Assert.ThrowsAsync<ArgumentException>(() => _sut.CheckForPeriodAvailability(CreateRequest()));
        }

        [Fact]
        public async Task CheckForPeriodAvailability_OverlappingUnrentablePeriod_ThrowsArgumentException()
        {
            _unrentablePeriodRepository.Setup(r => r.GetActiveAndFuturePeriodsForAccomodation(It.IsAny<ReservationAvailabilityRequest>()))
                .ReturnsAsync([new UnrentablePeriod { AccommodationId = AccommodationId, Reason = "Onderhoud" }]);

            await Assert.ThrowsAsync<ArgumentException>(() => _sut.CheckForPeriodAvailability(CreateRequest()));
        }

        [Fact]
        public async Task CheckForPeriodAvailability_InvalidPersons_DoesNotQueryAvailability()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => _sut.CheckForPeriodAvailability(CreateRequest(persons: 99)));

            _reservationRepository.Verify(r => r.GetActiveAndFutureReservationsForAccomodation(It.IsAny<ReservationAvailabilityRequest>()), Times.Never);
            _unrentablePeriodRepository.Verify(r => r.GetActiveAndFuturePeriodsForAccomodation(It.IsAny<ReservationAvailabilityRequest>()), Times.Never);
        }

        [Fact]
        public async Task CheckForPeriodAvailability_EndBeforeStart_Throws()
        {
            DateTime start = DateTime.UtcNow.Date.AddDays(30);
            var request = new ReservationAvailabilityRequest(start, start.AddDays(-5), AccommodationId, 2);

            await Assert.ThrowsAsync<ArgumentException>(() => _sut.CheckForPeriodAvailability(request));
        }

        [Fact]
        public async Task CheckForPeriodAvailability_StartInPast_Throws()
        {
            DateTime start = DateTime.UtcNow.Date.AddDays(-30);
            var request = new ReservationAvailabilityRequest(start, start.AddDays(5), AccommodationId, 2);

            await Assert.ThrowsAsync<ArgumentException>(() => _sut.CheckForPeriodAvailability(request));
        }
    }
}
