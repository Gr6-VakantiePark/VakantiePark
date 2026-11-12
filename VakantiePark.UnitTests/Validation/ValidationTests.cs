using VakantiePark.Application.Extensions;
using VakantiePark.Application.Extensions.Accommodations;
using VakantiePark.Shared.Common;
using VakantiePark.Shared.Common.Pagination;
using VakantiePark.Shared.DTOs.Accommodations;
using VakantiePark.Shared.DTOs.Auth;
using VakantiePark.Shared.DTOs.Prices;

namespace VakantiePark.UnitTests.Validation
{
    public class ValidationTests
    {
        private static readonly DateOnly Arrival = new(2030, 7, 1);
        private static readonly DateOnly Departure = new(2030, 7, 4);

        private static CreateEmployeeDto CreateEmployee() => new()
        {
            Email = "nieuw@example.com",
            Password = "Zomer2027!Vakantie",
            FirstName = "Sanne",
            LastName = "de Vries",
            Roles = ["reception"],
        };

        private static AccommodationSearchCriteria CreateCriteria() => new()
        {
            Arrival = Arrival,
            Departure = Departure,
            Persons = 2,
        };

        [Fact]
        public void Validate_ValidCreateEmployee_DoesNotThrow()
        {
            var exception = Record.Exception(() => CreateEmployee().Validate());

            Assert.Null(exception);
        }

        [Theory]
        [InlineData("geen-email")]
        [InlineData("")]
        public void Validate_CreateEmployeeInvalidEmail_ThrowsArgumentException(string email)
        {
            var request = CreateEmployee();
            request.Email = email;

            Assert.Throws<ArgumentException>(() => request.Validate());
        }

        [Fact]
        public void Validate_CreateEmployeePasswordOneTooShort_ThrowsArgumentException()
        {
            var request = CreateEmployee();
            request.Password = new string('a', FieldLimits.PasswordMinLength - 1);

            Assert.Throws<ArgumentException>(() => request.Validate());
        }

        [Fact]
        public void Validate_CreateEmployeePasswordAtMinimum_DoesNotThrow()
        {
            var request = CreateEmployee();
            request.Password = new string('a', FieldLimits.PasswordMinLength);

            Assert.Null(Record.Exception(() => request.Validate()));
        }

        [Fact]
        public void Validate_CreateEmployeeNameTooLong_ThrowsArgumentException()
        {
            var request = CreateEmployee();
            request.FirstName = new string('a', FieldLimits.FirstNameMaxLength + 1);

            Assert.Throws<ArgumentException>(() => request.Validate());
        }

        [Fact]
        public void Validate_CreateEmployeeWithoutRoles_ThrowsArgumentException()
        {
            var request = CreateEmployee();
            request.Roles = [];

            Assert.Throws<ArgumentException>(() => request.Validate());
        }

        [Fact]
        public void Validate_NullCreateEmployee_ThrowsArgumentException()
        {
            CreateEmployeeDto? request = null;

            Assert.Throws<ArgumentException>(() => request.Validate());
        }

        [Fact]
        public void Validate_LoginPasswordTooLong_ThrowsArgumentException()
        {
            var request = new LoginRequestDto { Email = "a@example.com", Password = new string('a', FieldLimits.PasswordMaxLength + 1) };

            Assert.Throws<ArgumentException>(() => request.Validate());
        }

        [Fact]
        public void Validate_ValidSearchCriteria_DoesNotThrow()
        {
            Assert.Null(Record.Exception(() => CreateCriteria().Validate()));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(FieldLimits.MaxPersons + 1)]
        public void Validate_SearchCriteriaInvalidPersons_ThrowsArgumentException(int persons)
        {
            var criteria = CreateCriteria();
            criteria.Persons = persons;

            Assert.Throws<ArgumentException>(() => criteria.Validate());
        }

        [Fact]
        public void Validate_SearchCriteriaDepartureOnArrival_ThrowsArgumentException()
        {
            var criteria = CreateCriteria();
            criteria.Departure = criteria.Arrival;

            Assert.Throws<ArgumentException>(() => criteria.Validate());
        }

        [Fact]
        public void Validate_SearchCriteriaTooManyNights_ThrowsArgumentException()
        {
            var criteria = CreateCriteria();
            criteria.Departure = criteria.Arrival.AddDays(FieldLimits.MaxNights + 1);

            Assert.Throws<ArgumentException>(() => criteria.Validate());
        }

        [Fact]
        public void Validate_SearchCriteriaUnknownCategory_ThrowsArgumentException()
        {
            var criteria = CreateCriteria();
            criteria.Category = "Hotel";

            Assert.Throws<ArgumentException>(() => criteria.Validate());
        }

        [Fact]
        public void Validate_SearchCriteriaPageSizeTooLarge_ThrowsArgumentException()
        {
            var criteria = CreateCriteria();
            criteria.PageSize = PaginationBase.MaxPageSize + 1;

            Assert.Throws<ArgumentException>(() => criteria.Validate());
        }

        [Fact]
        public void Validate_PriceRequestTypeTooLong_ThrowsArgumentException()
        {
            var request = new PriceRequestDto
            {
                Type = new string('a', FieldLimits.SlugMaxLength + 1),
                Arrival = Arrival,
                Departure = Departure,
                Persons = 2,
            };

            Assert.Throws<ArgumentException>(() => request.Validate());
        }

        [Fact]
        public void ValidateStay_IncompleteStay_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => AccommodationValidation.ValidateStay(Arrival, null, 2));
        }
    }
}
