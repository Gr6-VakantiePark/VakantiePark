using System.ComponentModel.DataAnnotations;

namespace VakantiePark.UI.Models;

public sealed class StayChoice
{
    [Required(ErrorMessage = "Kies je aankomstdatum.")]
    public DateOnly? Arrival { get; set; }

    [Required(ErrorMessage = "Kies je vertrekdatum.")]
    public DateOnly? Departure { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Kies minimaal 1 persoon.")]
    public int Persons { get; set; } = 2;

    [Range(0, int.MaxValue, ErrorMessage = "Het aantal kinderen kan niet negatief zijn.")]
    public int YoungChildren { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Het aantal huisdieren kan niet negatief zijn.")]
    public int Pets { get; set; }
}
