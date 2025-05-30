using System.Text.RegularExpressions;

namespace CurrencyExchange.Core.Models;

public class Currency
{
    private Currency(int id, string code, string fullName, string sign)
    {
        Id = id;
        Code = code;
        FullName = fullName;
        Sign = sign;
    }

    public int Id { get; }
    public string Code { get; }
    public string FullName { get; }
    public string Sign { get; }


    public static Currency Create(int id, string code, string fullName, string sign)
    {
        code = code.ToUpperInvariant();

        Validate(code, 3, 5, @"[^A-Z]");
        Validate(fullName, 3, 60, @"[^A-Za-zА-Яа-яЁё() ]");
        Validate(sign, 1, 3, @"[ \n]");

        return new Currency(id, code, fullName, sign);
    }

    public static void Validate(string validateString, int minLength, int maxLength, string pattern)
    {
        if (string.IsNullOrEmpty(validateString))
            throw new ArgumentException("Не заполнен один или несколько параметров\n");
        else if (validateString.Length < minLength || validateString.Length > maxLength)
            throw new ArgumentException(
                $"Не корректное количество символов указано для поля {validateString} валюты. Длина строки может быть от {minLength}-х до {maxLength} символов.\n");
        else if (Regex.IsMatch(validateString, pattern, RegexOptions.Compiled))
            throw new ArgumentException($"Не корректные символы в строке {validateString}.\n");
    }
}