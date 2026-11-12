using System.ComponentModel.DataAnnotations;

namespace VakantiePark.Shared.Common.Pagination
{
    public enum AccommodationOrderBy
    {
        [Display(Name = "Capaciteit")]
        MaxPersons,

        [Display(Name = "Naam")]
        Name,

        [Display(Name = "Type")]
        Type,

        [Display(Name = "Zone")]
        Zone
    }
}
