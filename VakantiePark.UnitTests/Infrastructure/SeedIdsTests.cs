using VakantiePark.Infrastructure.Data.Seeders;

namespace VakantiePark.UnitTests.Infrastructure
{
    public class SeedIdsTests
    {
        [Fact]
        public void For_SameKindAndKey_ReturnsSameGuid()
        {
            Assert.Equal(SeedIds.Accommodation("B4-01"), SeedIds.Accommodation("B4-01"));
        }

        [Fact]
        public void For_KeyWithOtherCasingOrSpaces_ReturnsSameGuid()
        {
            Assert.Equal(SeedIds.Accommodation("B4-01"), SeedIds.Accommodation(" b4-01 "));
        }

        [Fact]
        public void For_DifferentKind_ReturnsDifferentGuid()
        {
            Assert.NotEqual(SeedIds.Role("pool"), SeedIds.Permission("pool"));
        }

        [Fact]
        public void For_AnyKey_ReturnsVersion3Guid()
        {
            Assert.Equal(3, SeedIds.AccommodationType("berk-4").Version);
        }

        [Theory]
        [InlineData("", "key")]
        [InlineData("kind", " ")]
        public void For_EmptyKindOrKey_ThrowsArgumentException(string kind, string key)
        {
            Assert.ThrowsAny<ArgumentException>(() => SeedIds.For(kind, key));
        }
    }
}
