using System.Xml.Serialization;

namespace JobLoaderCurrency.Interfaces.CurrencyLoader.Dtos
{
    public record ValuteDto
    {
        [XmlAttribute("ID")]
        public string? Id { get; init; }

        [XmlElement("NumCode")]
        public string? NumCode { get; init; }

        [XmlElement("CharCode")]
        public string? CharCode { get; init; }

        [XmlElement("Nominal")]
        public int Nominal { get; init; }

        [XmlElement("Name")]
        public string? Name { get; init; }

        [XmlElement("Value")]
        public string? ValueString { get; init; }

        [XmlElement("VunitRate")]
        public string? VunitRateString { get; init; }

        [XmlIgnore]
        public decimal Value => decimal.Parse(ValueString?.Replace(".", ",") ?? "0");

        [XmlIgnore]
        public decimal VunitRate => Value / Nominal;
    }
}
