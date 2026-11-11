using System.ComponentModel.DataAnnotations;

namespace VakantiePark.Shared.Common
{
    public enum AccommodatieOrderBy
    {
        [Display(Name = "Capaciteit")]
        MaxPersonen,

        [Display(Name = "Naam")]
        Naam,

        [Display(Name = "Type")]
        Type,

        [Display(Name = "Zone")]
        Zone
    }
}
