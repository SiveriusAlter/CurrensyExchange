namespace CurrencyExchange.Data.Entities
{
    public class CurrencyEntity(int id, string code, string fullName, string sign)
    {
        private string code = code.ToUpperInvariant();

        public int Id { get; set; } = id;
        public string Code { get => code; set => code = value.ToUpperInvariant(); }
        public string FullName { get; set; } = fullName;
        public string Sign { get; set; } = sign;
    }
}
