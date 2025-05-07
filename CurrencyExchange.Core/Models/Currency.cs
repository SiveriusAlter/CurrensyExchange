
namespace CurrencyExchange.Core.Models
{
    public class Currency(int Id, string Code, string FullName, string Sign)
    {
        public int Id { get; set; } = Id;
        public string Code { get; set; } = Code;
        public string FullName { get; set; } = FullName;
        public string Sign { get; set; } = Sign;



        public static Currency Create(int id, string code, string fullName, string sign)
        {
            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(sign))
            {
                throw new ArgumentException( "Не заполнен один или несколько параметров валюты");
            }

            var currency = new Currency(id, code, fullName, sign);
            return currency;
        }
    }
}
