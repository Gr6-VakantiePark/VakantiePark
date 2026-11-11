namespace VakantiePark.Domain.Entities
{
    /// <summary>
    /// Een voorziening die een accommodatie wel of niet heeft, bijv. sauna, vaatwasser of elektriciteit.
    /// Bron: de boolean-velden in "properties" van accommodaties.json.
    /// </summary>
    public class Voorziening
    {
        public int Id { get; set; }

        /// <summary>De sleutel uit de JSON, bijv. "sauna", "dishwasher", "waterConnection".</summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>Weergavenaam, bijv. "Sauna", "Vaatwasser", "Wateraansluiting".</summary>
        public string Naam { get; set; } = string.Empty;

        public ICollection<Accommodatie> Accommodaties { get; set; } = new List<Accommodatie>();
    }
}
