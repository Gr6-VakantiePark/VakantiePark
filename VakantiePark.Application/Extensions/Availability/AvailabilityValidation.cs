using VakantiePark.Shared.DTOs.Reservations;

namespace VakantiePark.Application.Extensions.Availability
{
    public static class AvailabilityValidation
    {
        public const int MinimumHoursBeforeStart = 5;

        public static void Validate(this ReservationAvailabilityRequest? request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request), "Zoekcriteria zijn verplicht.");
            }

            if (request.StartTime <= DateTime.UtcNow.AddHours(MinimumHoursBeforeStart))
            {
                throw new ArgumentException("ongeldige start- en/of einddatum");
            }

            if (request.StartTime >= request.EndTime)
            {
                throw new ArgumentException("De startdatum moet voor de einddatum liggen.");
            }
        }
    }
}
