using Moq;
using VakantiePark.Application.Services;
using VakantiePark.Domain.Entities;
using VakantiePark.Domain.Interfaces;

namespace VakantiePark.UnitTests.Services
{
    public class PoolServiceTests
    {
        private static readonly Guid MorningId = Guid.NewGuid();
        private static readonly Guid AfternoonId = Guid.NewGuid();

        private readonly Mock<IPoolRepository> _repository = new();
        private readonly PoolService _sut;

        public PoolServiceTests()
        {
            _sut = new PoolService(_repository.Object);

            _repository.Setup(r => r.GetTimeSlotsAsync(It.IsAny<DayOfWeek>()))
                .ReturnsAsync(
                [
                    new PoolTimeSlot { Id = MorningId, StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(10, 0), Capacity = 50 },
                    new PoolTimeSlot { Id = AfternoonId, StartTime = new TimeOnly(14, 0), EndTime = new TimeOnly(15, 0), Capacity = 50 },
                ]);
            _repository.Setup(r => r.GetReservedPlacesAsync(It.IsAny<DateOnly>()))
                .ReturnsAsync(new Dictionary<Guid, int>());
        }

        private static DateOnly Today => DateOnly.FromDateTime(DateTime.Today);

        private void SetReservedPlaces(Guid timeSlotId, int persons) =>
            _repository.Setup(r => r.GetReservedPlacesAsync(It.IsAny<DateOnly>()))
                .ReturnsAsync(new Dictionary<Guid, int> { [timeSlotId] = persons });

        [Fact]
        public async Task GetTimeSlotsAsync_NoReservations_AllPlacesAreAvailable()
        {
            var day = await _sut.GetTimeSlotsAsync(Today.AddDays(1));

            Assert.Equal(2, day.TimeSlots.Count);
            Assert.All(day.TimeSlots, t => Assert.Equal(50, t.AvailablePlaces));
            Assert.All(day.TimeSlots, t => Assert.False(t.IsFull));
        }

        [Fact]
        public async Task GetTimeSlotsAsync_WithReservations_ReturnsRemainingPlaces()
        {
            SetReservedPlaces(MorningId, 20);

            var day = await _sut.GetTimeSlotsAsync(Today.AddDays(1));

            var morning = day.TimeSlots.Single(t => t.Id == MorningId);
            Assert.Equal(50, morning.Capacity);
            Assert.Equal(30, morning.AvailablePlaces);
            Assert.False(morning.IsFull);
        }

        [Fact]
        public async Task GetTimeSlotsAsync_FullTimeSlot_IsMarkedFull()
        {
            SetReservedPlaces(MorningId, 50);

            var day = await _sut.GetTimeSlotsAsync(Today.AddDays(1));

            var morning = day.TimeSlots.Single(t => t.Id == MorningId);
            var afternoon = day.TimeSlots.Single(t => t.Id == AfternoonId);
            Assert.Equal(0, morning.AvailablePlaces);
            Assert.True(morning.IsFull);
            Assert.False(afternoon.IsFull);
        }

        [Fact]
        public async Task GetTimeSlotsAsync_MoreReservedThanCapacity_HasNoNegativePlaces()
        {
            SetReservedPlaces(MorningId, 60);

            var day = await _sut.GetTimeSlotsAsync(Today.AddDays(1));

            var morning = day.TimeSlots.Single(t => t.Id == MorningId);
            Assert.Equal(0, morning.AvailablePlaces);
            Assert.True(morning.IsFull);
        }

        [Fact]
        public async Task GetTimeSlotsAsync_KeepsOrderOfTimeSlots()
        {
            var day = await _sut.GetTimeSlotsAsync(Today.AddDays(1));

            Assert.Equal([MorningId, AfternoonId], day.TimeSlots.Select(t => t.Id));
        }

        [Fact]
        public async Task GetTimeSlotsAsync_Date_UsesDayOfWeekAndDateOfThatDay()
        {
            var date = Today.AddDays(3);

            var day = await _sut.GetTimeSlotsAsync(date);

            Assert.Equal(date, day.Date);
            _repository.Verify(r => r.GetTimeSlotsAsync(date.DayOfWeek), Times.Once);
            _repository.Verify(r => r.GetReservedPlacesAsync(date), Times.Once);
        }

        [Fact]
        public async Task GetTimeSlotsAsync_NoDate_UsesToday()
        {
            var day = await _sut.GetTimeSlotsAsync();

            Assert.Equal(Today, day.Date);
            _repository.Verify(r => r.GetTimeSlotsAsync(Today.DayOfWeek), Times.Once);
        }

        [Fact]
        public async Task GetTimeSlotsAsync_DayWithoutTimeSlots_ReturnsEmptyList()
        {
            _repository.Setup(r => r.GetTimeSlotsAsync(It.IsAny<DayOfWeek>())).ReturnsAsync([]);

            var day = await _sut.GetTimeSlotsAsync(Today.AddDays(1));

            Assert.Empty(day.TimeSlots);
        }

        [Fact]
        public async Task GetTimeSlotsAsync_DateInPast_ThrowsArgumentException()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => _sut.GetTimeSlotsAsync(Today.AddDays(-1)));
            _repository.Verify(r => r.GetTimeSlotsAsync(It.IsAny<DayOfWeek>()), Times.Never);
        }
    }
}
