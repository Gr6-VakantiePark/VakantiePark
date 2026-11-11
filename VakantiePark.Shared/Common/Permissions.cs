namespace VakantiePark.Shared.Common
{
    public static class Permissions
    {
        public const string ClaimType = "permission";

        public const string ManageEmployees = "employees.manage";
        public const string ViewGuests = "guests.view";
        public const string ManageGuests = "guests.manage";
        public const string ManageReservations = "reservations.manage";
        public const string ManageAccommodations = "accommodations.manage";
        public const string ManagePoolCapacity = "pool.capacity.manage";

        public static readonly IReadOnlyList<string> All =
        [
            ManageEmployees,
            ViewGuests,
            ManageGuests,
            ManageReservations,
            ManageAccommodations,
            ManagePoolCapacity,
        ];
    }

    public static class Roles
    {
        public const string Administrator = "administrator";
        public const string Reception = "reception";
        public const string Pool = "pool";
    }
}
