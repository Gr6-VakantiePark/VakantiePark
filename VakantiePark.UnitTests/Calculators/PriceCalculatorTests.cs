using VakantiePark.Application.Calculators;

namespace VakantiePark.UnitTests.Calculators
{
    public class PriceCalculatorTests
    {
        [Fact]
        public void Calculate_ValidInput_AddsStayCleaningFeeAndTouristTax()
        {
            var price = PriceCalculator.Calculate(100m, 3, 4, 75m, 2.50m);

            Assert.Equal(300m, price.Stay);
            Assert.Equal(75m, price.CleaningFee);
            Assert.Equal(30m, price.TouristTax);
            Assert.Equal(405m, price.Total);
        }

        [Theory]
        [InlineData(85, 1, 1, 75, 2.50, 162.50)]
        [InlineData(105, 7, 6, 75, 2.50, 915)]
        [InlineData(25, 2, 2, 0, 0, 50)]
        public void Calculate_DifferentStays_ReturnsCorrectTotal(double pricePerNight, int nights, int persons, double cleaningFee, double touristTax, double expected)
        {
            var price = PriceCalculator.Calculate((decimal)pricePerNight, nights, persons, (decimal)cleaningFee, (decimal)touristTax);

            Assert.Equal((decimal)expected, price.Total);
        }

        [Fact]
        public void Calculate_OneNight_ChargesOneNight()
        {
            var price = PriceCalculator.Calculate(120m, 1, 2, 75m, 2.50m);

            Assert.Equal(1, price.Nights);
            Assert.Equal(120m, price.Stay);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Calculate_NoNights_ThrowsArgumentException(int nights)
        {
            Assert.Throws<ArgumentException>(() => PriceCalculator.Calculate(100m, nights, 2, 75m, 2.50m));
        }

        [Fact]
        public void Calculate_NoPersons_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => PriceCalculator.Calculate(100m, 2, 0, 75m, 2.50m));
        }

        [Fact]
        public void Calculate_NoPricePerNight_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => PriceCalculator.Calculate(0m, 2, 2, 75m, 2.50m));
        }

        [Fact]
        public void Calculate_NegativeCleaningFee_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => PriceCalculator.Calculate(100m, 2, 2, -1m, 2.50m));
        }

        [Fact]
        public void Calculate_NegativeTouristTax_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => PriceCalculator.Calculate(100m, 2, 2, 75m, -0.01m));
        }
    }
}
