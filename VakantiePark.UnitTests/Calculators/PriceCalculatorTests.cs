using VakantiePark.Application.Calculators;
using VakantiePark.Domain.Entities;
using VakantiePark.Domain.Enums;

namespace VakantiePark.UnitTests.Calculators
{
    public class PriceCalculatorTests
    {
        private static readonly DateOnly Monday = new(2030, 7, 1);
        private static readonly DateOnly Friday = new(2030, 7, 5);

        private static AccommodationType CreateHouse(decimal pricePerNight = 100m) => new()
        {
            Name = "Berk 4",
            Slug = "berk-4",
            Category = AccommodationCategory.HolidayHome,
            PricePerNight = pricePerNight,
        };

        private static AccommodationType CreatePitch(decimal pricePerNight = 25m) => new()
        {
            Name = "Tentplaats basis",
            Slug = "tentplaats-basis",
            Category = AccommodationCategory.CampingPitch,
            PricePerNight = pricePerNight,
        };

        private static ParkSettings CreateSettings() => new()
        {
            CleaningFee = 75m,
            TouristTaxPerPersonPerNight = 2.50m,
            TouristTaxMinimumAge = 3,
            WeekendSurchargePercentage = 10m,
            PetFeePerNight = 10m,
        };

        [Fact]
        public void Calculate_StayWithoutWeekend_AddsStayCleaningFeeAndTouristTax()
        {
            var price = PriceCalculator.Calculate(CreateHouse(), Monday, Monday.AddDays(3), 2, 0, 0, CreateSettings());

            Assert.Equal(3, price.Nights);
            Assert.Equal(300m, price.Stay);
            Assert.Equal(0, price.WeekendNights);
            Assert.Equal(0m, price.WeekendSurcharge);
            Assert.Equal(75m, price.CleaningFee);
            Assert.Equal(15m, price.TouristTax);
            Assert.Equal(390m, price.Total);
        }

        [Fact]
        public void Calculate_StayOverWeekend_AddsSurchargeForFridayAndSaturdayNight()
        {
            var price = PriceCalculator.Calculate(CreateHouse(), Friday, Friday.AddDays(3), 2, 0, 0, CreateSettings());

            Assert.Equal(2, price.WeekendNights);
            Assert.Equal(20m, price.WeekendSurcharge);
            Assert.Equal(410m, price.Total);
        }

        [Fact]
        public void Calculate_FullWeek_ChargesTwoWeekendNights()
        {
            var price = PriceCalculator.Calculate(CreateHouse(125m), Monday, Monday.AddDays(7), 2, 0, 0, CreateSettings());

            Assert.Equal(2, price.WeekendNights);
            Assert.Equal(25m, price.WeekendSurcharge);
        }

        [Fact]
        public void Calculate_OnlySundayNight_ChargesNoWeekendSurcharge()
        {
            var sunday = Friday.AddDays(2);

            var price = PriceCalculator.Calculate(CreateHouse(), sunday, sunday.AddDays(1), 2, 0, 0, CreateSettings());

            Assert.Equal(0, price.WeekendNights);
            Assert.Equal(0m, price.WeekendSurcharge);
        }

        [Fact]
        public void Calculate_DifferentSurchargePercentage_UsesParkSetting()
        {
            var settings = CreateSettings();
            settings.WeekendSurchargePercentage = 20m;

            var price = PriceCalculator.Calculate(CreateHouse(), Friday, Friday.AddDays(2), 2, 0, 0, settings);

            Assert.Equal(40m, price.WeekendSurcharge);
        }

        [Fact]
        public void Calculate_WithPets_ChargesPerPetPerNight()
        {
            var price = PriceCalculator.Calculate(CreateHouse(), Monday, Monday.AddDays(3), 2, 0, 2, CreateSettings());

            Assert.Equal(2, price.Pets);
            Assert.Equal(60m, price.PetFee);
            Assert.Equal(450m, price.Total);
        }

        [Fact]
        public void Calculate_WithoutPets_ChargesNoPetFee()
        {
            var price = PriceCalculator.Calculate(CreateHouse(), Monday, Monday.AddDays(3), 2, 0, 0, CreateSettings());

            Assert.Equal(0m, price.PetFee);
        }

        [Fact]
        public void Calculate_ChildrenUnderMinimumAge_PayNoTouristTax()
        {
            var price = PriceCalculator.Calculate(CreateHouse(), Monday, Monday.AddDays(3), 4, 2, 0, CreateSettings());

            Assert.Equal(4, price.Persons);
            Assert.Equal(2, price.YoungChildren);
            Assert.Equal(2, price.TaxedPersons);
            Assert.Equal(15m, price.TouristTax);
        }

        [Fact]
        public void Calculate_ChildrenFromMinimumAge_PayTouristTax()
        {
            var price = PriceCalculator.Calculate(CreateHouse(), Monday, Monday.AddDays(3), 4, 0, 0, CreateSettings());

            Assert.Equal(4, price.TaxedPersons);
            Assert.Equal(30m, price.TouristTax);
        }

        [Fact]
        public void Calculate_CampingPitch_ChargesNoCleaningFee()
        {
            var price = PriceCalculator.Calculate(CreatePitch(), Monday, Monday.AddDays(2), 2, 0, 0, CreateSettings());

            Assert.False(price.IncludesCleaningFee);
            Assert.Equal(0m, price.CleaningFee);
            Assert.Equal(60m, price.Total);
        }

        [Fact]
        public void Calculate_HolidayHome_ChargesCleaningFee()
        {
            var price = PriceCalculator.Calculate(CreateHouse(), Monday, Monday.AddDays(2), 2, 0, 0, CreateSettings());

            Assert.True(price.IncludesCleaningFee);
            Assert.Equal(75m, price.CleaningFee);
        }

        [Fact]
        public void Calculate_AllRulesCombined_ReturnsCorrectTotal()
        {
            var price = PriceCalculator.Calculate(CreateHouse(175m), Friday, Friday.AddDays(3), 5, 1, 1, CreateSettings());

            Assert.Equal(525m, price.Stay);
            Assert.Equal(35m, price.WeekendSurcharge);
            Assert.Equal(30m, price.PetFee);
            Assert.Equal(75m, price.CleaningFee);
            Assert.Equal(30m, price.TouristTax);
            Assert.Equal(695m, price.Total);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Calculate_NoNights_ThrowsArgumentException(int nights)
        {
            Assert.Throws<ArgumentException>(() => PriceCalculator.Calculate(CreateHouse(), Monday, Monday.AddDays(nights), 2, 0, 0, CreateSettings()));
        }

        [Fact]
        public void Calculate_NoPersons_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => PriceCalculator.Calculate(CreateHouse(), Monday, Monday.AddDays(2), 0, 0, 0, CreateSettings()));
        }

        [Fact]
        public void Calculate_OnlyYoungChildren_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => PriceCalculator.Calculate(CreateHouse(), Monday, Monday.AddDays(2), 2, 2, 0, CreateSettings()));
        }

        [Fact]
        public void Calculate_NegativeYoungChildren_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => PriceCalculator.Calculate(CreateHouse(), Monday, Monday.AddDays(2), 2, -1, 0, CreateSettings()));
        }

        [Fact]
        public void Calculate_NegativePets_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => PriceCalculator.Calculate(CreateHouse(), Monday, Monday.AddDays(2), 2, 0, -1, CreateSettings()));
        }

        [Fact]
        public void Calculate_NoPricePerNight_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => PriceCalculator.Calculate(CreateHouse(0m), Monday, Monday.AddDays(2), 2, 0, 0, CreateSettings()));
        }

        [Fact]
        public void Calculate_NegativeCleaningFee_ThrowsArgumentException()
        {
            var settings = CreateSettings();
            settings.CleaningFee = -1m;

            Assert.Throws<ArgumentException>(() => PriceCalculator.Calculate(CreateHouse(), Monday, Monday.AddDays(2), 2, 0, 0, settings));
        }

        [Fact]
        public void Calculate_NegativeTouristTax_ThrowsArgumentException()
        {
            var settings = CreateSettings();
            settings.TouristTaxPerPersonPerNight = -0.01m;

            Assert.Throws<ArgumentException>(() => PriceCalculator.Calculate(CreateHouse(), Monday, Monday.AddDays(2), 2, 0, 0, settings));
        }

        [Fact]
        public void Calculate_NegativeWeekendSurcharge_ThrowsArgumentException()
        {
            var settings = CreateSettings();
            settings.WeekendSurchargePercentage = -1m;

            Assert.Throws<ArgumentException>(() => PriceCalculator.Calculate(CreateHouse(), Monday, Monday.AddDays(2), 2, 0, 0, settings));
        }

        [Fact]
        public void Calculate_NegativePetFee_ThrowsArgumentException()
        {
            var settings = CreateSettings();
            settings.PetFeePerNight = -1m;

            Assert.Throws<ArgumentException>(() => PriceCalculator.Calculate(CreateHouse(), Monday, Monday.AddDays(2), 2, 0, 0, settings));
        }
    }
}
