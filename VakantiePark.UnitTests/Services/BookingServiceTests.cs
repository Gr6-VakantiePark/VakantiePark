using Moq;
using VakantiePark.Application.Interfaces;
using VakantiePark.Application.Services.Booking;
using VakantiePark.Domain.Entities;
using VakantiePark.Domain.Enums;
using VakantiePark.Domain.Interfaces;
using VakantiePark.Shared.DTOs.Reservations;

namespace VakantiePark.UnitTests.Services
{
    public class BookingServiceTests
    {
        private const decimal ExpectedTotal = 390m;
        private static readonly Guid AccommodationId = Guid.NewGuid();
        private static readonly DateTime Monday = new(2030, 3, 4, 15, 0, 0);
        private static readonly DateTime Friday = new(2030, 3, 8, 15, 0, 0);

        private readonly Mock<IAvailabilityService> _availabilityService = new();
        private readonly Mock<IAccommodationRepository> _accommodationRepository = new();
        private readonly Mock<IParkSettingsRepository> _settingsRepository = new();
        private readonly Mock<IReservationRepository> _reservationRepository = new();
        private readonly Mock<IRepository<Reservation>> _reservations = new();
        private readonly Mock<ITransactionRunner> _transactions = new();
        private readonly BookingService _sut;

        public BookingServiceTests()
        {
            _sut = new BookingService(
                _availabilityService.Object,
                _accommodationRepository.Object,
                _settingsRepository.Object,
                _reservationRepository.Object,
                _reservations.Object,
                _transactions.Object);

            _transactions.Setup(t => t.InTransactionAsync(It.IsAny<Func<Task<BookingDto>>>()))
                .Returns((Func<Task<BookingDto>> work) => work());
            _availabilityService.Setup(a => a.CheckForPeriodAvailability(It.IsAny<ReservationAvailabilityRequest>()))
                .ReturnsAsync(true);
            _accommodationRepository.Setup(r => r.GetByIdAsync(AccommodationId))
                .ReturnsAsync(new Accommodation
                {
                    Id = AccommodationId,
                    Name = "Berk 4 01",
                    MaxPersons = 4,
                    MaxPets = 1,
                    AccommodationType = new AccommodationType { PricePerNight = 100m, Category = AccommodationCategory.HolidayHome },
                });
            _settingsRepository.Setup(r => r.GetAsync())
                .ReturnsAsync(new ParkSettings
                {
                    CleaningFee = 75m,
                    TouristTaxPerPersonPerNight = 2.50m,
                    WeekendSurchargePercentage = 10m,
                    PetFeePerNight = 5m,
                });
            _reservationRepository.Setup(r => r.GetHighestBookingSequenceAsync())
                .ReturnsAsync((int?)null);
        }

        private static CreateBookingDto CreateRequest(decimal expectedTotal = ExpectedTotal, bool hasPaid = true, string email = "gast@example.com", DateTime? start = null, int youngChildren = 0, int pets = 0)
        {
            DateTime arrival = start ?? Monday;
            return new CreateBookingDto
            {
                AccommodationId = AccommodationId,
                StartDate = arrival,
                EndDate = arrival.AddDays(3).AddHours(-5),
                NumberOfGuests = 2,
                YoungChildren = youngChildren,
                Pets = pets,
                ExpectedTotalPrice = expectedTotal,
                HasPaid = hasPaid,
                Guest = new GuestDto { Name = "Gast", Email = email },
            };
        }

        [Fact]
        public async Task CreateAsync_ValidRequest_SavesReservationWithGuest()
        {
            Reservation? saved = null;
            _reservations.Setup(r => r.Add(It.IsAny<Reservation>())).Callback<Reservation>(r => saved = r);

            var booking = await _sut.CreateAsync(CreateRequest(email: " Gast@Example.com "));

            _reservations.Verify(r => r.Add(It.IsAny<Reservation>()), Times.Exactly(1));
            _reservations.Verify(r => r.SaveChangesAsync(), Times.Exactly(1));
            Assert.NotNull(saved);
            Assert.Equal(AccommodationId, saved.AccommodationId);
            Assert.Equal(ExpectedTotal, saved.TotalPrice);
            Assert.Equal("gast@example.com", saved.Guest.Email);
            Assert.Equal(saved.BookingNumber, booking.BookingNumber);
            Assert.Equal("Berk 4 01", booking.AccommodationName);
            Assert.Equal(ExpectedTotal, booking.Price.Total);
        }

        [Fact]
        public async Task CreateAsync_ValidRequest_RunsInTransaction()
        {
            await _sut.CreateAsync(CreateRequest());

            _transactions.Verify(t => t.InTransactionAsync(It.IsAny<Func<Task<BookingDto>>>()), Times.Exactly(1));
        }

        [Fact]
        public async Task CreateAsync_HasPaid_SetsStatusConfirmed()
        {
            var booking = await _sut.CreateAsync(CreateRequest(hasPaid: true));

            Assert.Equal(nameof(ReservationStatus.Confirmed), booking.Status);
        }

        [Fact]
        public async Task CreateAsync_HasNotPaid_SetsStatusPending()
        {
            var booking = await _sut.CreateAsync(CreateRequest(hasPaid: false));

            Assert.Equal(nameof(ReservationStatus.Pending), booking.Status);
        }

        [Fact]
        public async Task CreateAsync_FirstBooking_GetsMonthFollowedBy1001()
        {
            Reservation? saved = null;
            _reservations.Setup(r => r.Add(It.IsAny<Reservation>())).Callback<Reservation>(r => saved = r);

            var booking = await _sut.CreateAsync(CreateRequest());

            Assert.Equal($"{DateTime.UtcNow:MM}1001", booking.BookingNumber);
            Assert.Equal(BookingService.FirstBookingSequence, saved!.BookingSequence);
        }

        [Fact]
        public async Task CreateAsync_ExistingBookings_UsesNextSequenceNumber()
        {
            _reservationRepository.Setup(r => r.GetHighestBookingSequenceAsync()).ReturnsAsync(1041);

            var booking = await _sut.CreateAsync(CreateRequest());

            Assert.Equal($"{DateTime.UtcNow:MM}1042", booking.BookingNumber);
        }

        [Fact]
        public async Task CreateAsync_NotAvailable_ThrowsArgumentExceptionAndSavesNothing()
        {
            _availabilityService.Setup(a => a.CheckForPeriodAvailability(It.IsAny<ReservationAvailabilityRequest>()))
                .ThrowsAsync(new ArgumentException("De accommodatie is niet beschikbaar in de opgegeven periode."));

            await Assert.ThrowsAsync<ArgumentException>(() => _sut.CreateAsync(CreateRequest()));

            _reservations.Verify(r => r.Add(It.IsAny<Reservation>()), Times.Never);
            _reservations.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        [Theory]
        [InlineData(389.99)]
        [InlineData(390.01)]
        [InlineData(1)]
        public async Task CreateAsync_PriceDiffersFromClient_ThrowsArgumentExceptionAndSavesNothing(double clientTotal)
        {
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _sut.CreateAsync(CreateRequest(expectedTotal: (decimal)clientTotal)));

            Assert.Equal(BookingService.PriceChanged, exception.Message);
            _reservations.Verify(r => r.Add(It.IsAny<Reservation>()), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_WeekendYoungChildAndPet_UsesSamePriceRulesAsBookingPage()
        {
            var booking = await _sut.CreateAsync(CreateRequest(expectedTotal: 417.50m, start: Friday, youngChildren: 1, pets: 1));

            Assert.Equal(2, booking.Price.WeekendNights);
            Assert.Equal(20m, booking.Price.WeekendSurcharge);
            Assert.Equal(15m, booking.Price.PetFee);
            Assert.Equal(7.50m, booking.Price.TouristTax);
            Assert.Equal(417.50m, booking.Price.Total);
        }

        [Fact]
        public async Task CreateAsync_MorePetsThanAllowed_ThrowsArgumentExceptionAndSavesNothing()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => _sut.CreateAsync(CreateRequest(pets: 2)));

            _reservations.Verify(r => r.Add(It.IsAny<Reservation>()), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_MoreYoungChildrenThanPersons_ThrowsArgumentExceptionBeforeTransaction()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => _sut.CreateAsync(CreateRequest(youngChildren: 3)));

            _transactions.Verify(t => t.InTransactionAsync(It.IsAny<Func<Task<BookingDto>>>()), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_AccommodationNotFound_ThrowsKeyNotFoundException()
        {
            _accommodationRepository.Setup(r => r.GetByIdAsync(AccommodationId)).ReturnsAsync((Accommodation?)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _sut.CreateAsync(CreateRequest()));
        }

        [Fact]
        public async Task CreateAsync_SettingsNotFound_ThrowsKeyNotFoundException()
        {
            _settingsRepository.Setup(r => r.GetAsync()).ReturnsAsync((ParkSettings?)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _sut.CreateAsync(CreateRequest()));
        }

        [Theory]
        [InlineData("", "gast@example.com")]
        [InlineData("Gast", "geen-email")]
        [InlineData("Gast", "")]
        public async Task CreateAsync_InvalidGuest_ThrowsArgumentExceptionBeforeTransaction(string name, string email)
        {
            var request = CreateRequest();
            request.Guest = new GuestDto { Name = name, Email = email };

            await Assert.ThrowsAsync<ArgumentException>(() => _sut.CreateAsync(request));

            _transactions.Verify(t => t.InTransactionAsync(It.IsAny<Func<Task<BookingDto>>>()), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_NullRequest_ThrowsArgumentException()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => _sut.CreateAsync(null!));
        }
    }
}
