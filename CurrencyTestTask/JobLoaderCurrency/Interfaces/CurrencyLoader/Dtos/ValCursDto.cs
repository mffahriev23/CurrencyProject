using System.Xml.Serialization;

namespace JobLoaderCurrency.Interfaces.CurrencyLoader.Dtos
{
    [XmlRoot("ValCurs")]
    public record ValCursDto
    {
        [XmlAttribute("Date")]
        public string? Date { get; init; }

        [XmlAttribute("name")]
        public string? Name { get; init; }

        [XmlElement("Valute")]
        public ValuteDto[] Valutes { get; init; }

        public ValCursDto()
        {
            Valutes = Array.Empty<ValuteDto>();
        }
    }
}
