namespace CurrencyExchange.Core.Models
{
    public class Currency(int id, string code, string fullName, string sign)
    {
        public int ID { get; } = id;
        public string Code { get; } = code;
        public string FullName { get; } = fullName;
        public string Sign { get; } = sign;



        public static (Currency currency, string error) Create(int id, string code, string fullName, string sign) 
        {
            var error = string.Empty;

            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(sign)) 
            {
                error = "Не заполнен один или несколько параметров валюты";
            }

            var currency = new Currency(id, code, fullName, sign);
            return (currency, error);
        }
    }
}
