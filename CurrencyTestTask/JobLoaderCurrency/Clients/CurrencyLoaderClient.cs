using System.Text;
using System.Xml.Serialization;
using JobLoaderCurrency.Interfaces.CurrencyLoader;
using JobLoaderCurrency.Interfaces.CurrencyLoader.Dtos;

namespace JobLoaderCurrency.Clients
{
    public class CurrencyLoaderClient : ICurrencyLoaderClient
    {
        readonly HttpClient _client;
        const string _getCurrencies = "scripts/XML_daily.asp";

        public CurrencyLoaderClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<ValCursDto> GetCurrencies(CancellationToken cancellationToken)
        {
            using HttpResponseMessage response = await _client.GetAsync(_getCurrencies, cancellationToken);
            response.EnsureSuccessStatusCode();

            byte[] bytes = await response.Content.ReadAsByteArrayAsync();

            Encoding encoding = System.Text.Encoding.GetEncoding("windows-1251");
            string xmlString = encoding.GetString(bytes);

            using StringReader reader = new StringReader(xmlString);
            XmlSerializer serializer = new XmlSerializer(typeof(ValCursDto));
            return (ValCursDto)serializer.Deserialize(reader);
        }
    }
}
