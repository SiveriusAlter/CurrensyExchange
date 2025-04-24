
namespace CurrencyExchange.Core.Models
{
    public class Currency(int Id, string Code, string FullName, string Sign)
    {
        public int Id { get; set; } = Id;
        public string Code { get; set; } = Code;
        public string FullName { get; set; } = FullName;
        public string Sign { get; set; } = Sign;



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

        public static object Create(int v1, object id, string v2, object code, string v3, object fullName, string v4, object sign)
        {
            throw new NotImplementedException();
        }
    }
}
