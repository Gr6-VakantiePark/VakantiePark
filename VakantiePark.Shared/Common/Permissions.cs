namespace VakantiePark.Shared.Common
{
    public static class Permissions
    {
        public const string ClaimType = "permission";

        public const string OwnerClaimType = "owner";

        public const string Owner = "owner";

        public const string ViewGuests = "guests.view";
        public const string ManageGuests = "guests.manage";
        public const string ManageReservations = "reservations.manage";
        public const string ManageAccommodations = "accommodations.manage";
        public const string ManagePoolCapacity = "pool.capacity.manage";

        public static readonly IReadOnlyList<string> All =
        [
            ViewGuests,
            ManageGuests,
            ManageReservations,
            ManageAccommodations,
            ManagePoolCapacity,
        ];
    }
}
