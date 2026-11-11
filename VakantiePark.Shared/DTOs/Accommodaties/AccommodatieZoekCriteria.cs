using VakantiePark.Shared.Common;

namespace VakantiePark.Shared.DTOs.Accommodaties
{
    public class AccommodatieZoekCriteria : Pagination<AccommodatieOrderBy>
    {
        public const int StandaardPageSize = 12;
        public const AccommodatieOrderBy StandaardOrderBy = AccommodatieOrderBy.MaxPersonen;
        public const OrderType StandaardOrderType = OrderType.Ascending;

        public AccommodatieZoekCriteria()
        {
            PageSize = StandaardPageSize;
            OrderBy = StandaardOrderBy;
            OrderType = StandaardOrderType;
        }

        public DateOnly Aankomst { get; set; }
        public DateOnly Vertrek { get; set; }
        public int Personen { get; set; }

        public string? Categorie { get; set; }

        public string? Type { get; set; }

        public int Huisdieren { get; set; }

        public string? Voorzieningen { get; set; }

        public int AantalNachten => Vertrek.DayNumber - Aankomst.DayNumber;

        public List<string> VoorzieningCodes =>
            (Voorzieningen ?? string.Empty)
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .ToList();

        public bool HeeftFilters =>
            !string.IsNullOrWhiteSpace(Categorie)
            || !string.IsNullOrWhiteSpace(Type)
            || Huisdieren > 0
            || VoorzieningCodes.Count > 0;

        public AccommodatieZoekCriteria MetPagina(int pageNumber) => Kopie(c => c.PageNumber = pageNumber);

        public AccommodatieZoekCriteria ZonderFilters() =>
            Kopie(c =>
            {
                c.Categorie = null;
                c.Type = null;
                c.Huisdieren = 0;
                c.Voorzieningen = null;
                c.PageNumber = 1;
            });

        public AccommodatieZoekCriteria MetSortering(AccommodatieOrderBy orderBy, OrderType orderType) =>
            Kopie(c =>
            {
                c.OrderBy = orderBy;
                c.OrderType = orderType;
                c.PageNumber = 1;
            });

        public string ToQueryString()
        {
            var delen = new List<string>
            {
                $"aankomst={Aankomst:yyyy-MM-dd}",
                $"vertrek={Vertrek:yyyy-MM-dd}",
                $"personen={Personen}",
            };
            if (!string.IsNullOrWhiteSpace(Categorie)) delen.Add($"categorie={Uri.EscapeDataString(Categorie)}");
            if (!string.IsNullOrWhiteSpace(Type)) delen.Add($"type={Uri.EscapeDataString(Type)}");
            if (Huisdieren > 0) delen.Add($"huisdieren={Huisdieren}");
            if (!string.IsNullOrWhiteSpace(Voorzieningen)) delen.Add($"voorzieningen={Uri.EscapeDataString(Voorzieningen)}");
            if (OrderBy != StandaardOrderBy) delen.Add($"orderBy={OrderBy}");
            if (OrderType != StandaardOrderType) delen.Add($"orderType={OrderType}");
            if (PageNumber > 1) delen.Add($"pageNumber={PageNumber}");
            if (PageSize != StandaardPageSize) delen.Add($"pageSize={PageSize}");
            return string.Join("&", delen);
        }

        private AccommodatieZoekCriteria Kopie(Action<AccommodatieZoekCriteria> aanpassing)
        {
            var kopie = (AccommodatieZoekCriteria)MemberwiseClone();
            aanpassing(kopie);
            return kopie;
        }
    }
}
