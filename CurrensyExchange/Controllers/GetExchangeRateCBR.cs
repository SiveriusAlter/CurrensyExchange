namespace CurrencyExchange.API.Controllers
{
    public class GetExchangeRateCBR
    {

        public async Task Get()
        {

            var httpClient = new HttpClient();
            var httpRequestMessage = await httpClient.GetAsync("https://www.cbr-xml-daily.ru/daily_utf8.xml");
            var response = await httpRequestMessage.Content.ReadAsStringAsync();
            Console.WriteLine($"++{response}\n++");
        }
    }
}
