using Moq;
using VakantiePark.Application.Services.Booking;
using VakantiePark.Domain.Entities;
using VakantiePark.Domain.Interfaces;

namespace VakantiePark.UnitTests.Services
{
    public class BookingOwnershipCheckTests
    {
        private const string BookingNumber = "111001";
        private const string Email = "gast@example.com";

        private readonly Mock<IReservationRepository> _reservationRepository = new();
        private readonly BookingOwnershipCheck _sut;

        public BookingOwnershipCheckTests()
        {
            _sut = new BookingOwnershipCheck(_reservationRepository.Object);

            _reservationRepository.Setup(r => r.GetByBookingNumberAsync(BookingNumber))
                .ReturnsAsync(new Reservation { BookingNumber = BookingNumber, Guest = new Guest { Email = Email } });
        }

        [Fact]
        public async Task GetOwnedReservationAsync_MatchingEmail_ReturnsReservation()
        {
            var reservation = await _sut.GetOwnedReservationAsync(BookingNumber, Email);

            Assert.Equal(BookingNumber, reservation.BookingNumber);
        }

        [Fact]
        public async Task GetOwnedReservationAsync_EmailWithOtherCasingAndSpaces_ReturnsReservation()
        {
            var reservation = await _sut.GetOwnedReservationAsync(BookingNumber, "  Gast@Example.COM ");

            Assert.Equal(BookingNumber, reservation.BookingNumber);
        }

        [Fact]
        public async Task GetOwnedReservationAsync_BookingNumberWithSpaces_LooksUpTrimmedNumber()
        {
            await _sut.GetOwnedReservationAsync($" {BookingNumber} ", Email);

            _reservationRepository.Verify(r => r.GetByBookingNumberAsync(BookingNumber), Times.Exactly(1));
        }

        [Fact]
        public async Task GetOwnedReservationAsync_OtherEmail_ThrowsKeyNotFoundException()
        {
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _sut.GetOwnedReservationAsync(BookingNumber, "iemand@example.com"));

            Assert.Equal(BookingOwnershipCheck.NotFound, exception.Message);
        }

        [Fact]
        public async Task GetOwnedReservationAsync_UnknownBookingNumber_ThrowsKeyNotFoundException()
        {
            _reservationRepository.Setup(r => r.GetByBookingNumberAsync("119999")).ReturnsAsync((Reservation?)null);

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _sut.GetOwnedReservationAsync("119999", Email));

            Assert.Equal(BookingOwnershipCheck.NotFound, exception.Message);
        }

        [Theory]
        [InlineData("", Email)]
        [InlineData("   ", Email)]
        [InlineData(BookingNumber, "")]
        [InlineData(BookingNumber, "   ")]
        public async Task GetOwnedReservationAsync_MissingDetails_ThrowsArgumentException(string bookingNumber, string email)
        {
            await Assert.ThrowsAsync<ArgumentException>(() => _sut.GetOwnedReservationAsync(bookingNumber, email));

            _reservationRepository.Verify(r => r.GetByBookingNumberAsync(It.IsAny<string>()), Times.Never);
        }
    }
}
