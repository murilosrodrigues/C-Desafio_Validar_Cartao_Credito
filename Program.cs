
using System.Text.RegularExpressions;

public class CardValidator
{
    public static bool LuhnAlgorithm(string cardNumber)
    {
        string cleanNumber = new string(cardNumber.Where(char.IsDigit).ToArray());
        
        int sum = 0;
        bool shouldDouble = false;

        for (int i = cleanNumber.Length - 1; i >= 0; i--)
        {
            int digit = cleanNumber[i] - '0';

            if (shouldDouble)
            {
                digit *= 2;
                if (digit > 9) digit -= 9;
            }

            sum += digit;
            shouldDouble = !shouldDouble;
        }

        return sum % 10 == 0;
    }

    public static string GetCardIssuer(string cardNumber)
    {
        var cardPatterns = new Dictionary<string, string>
        {
            { "Visa", @"^4[0-9]{12}(?:[0-9]{3})?$" },
            { "MasterCard", @"^5[1-5][0-9]{14}$" },
            { "American Express", @"^3[47][0-9]{13}$" },
            { "Discover", @"^6(?:011|5[0-9]{2})[0-9]{12}$" },
            { "JCB", @"^(?:2131|1800|35\d{3})\d{11}$" },
            { "Diners Club", @"^3(?:0[0-5]|[68][0-9])[0-9]{11}$" },
            { "Enroute", @"^497138\d{8,10}$" },
            { "Voyager", @"^8608\d{12}(\d{3})?$" },
            { "HiperCard", @"^(38|60)\d{14}$" },
            { "Aura", @"^50[0-9]{14}$" }
        };

        foreach (var pattern in cardPatterns)
        {
            if (Regex.IsMatch(cardNumber, pattern.Value))
            {
                return pattern.Key;
            }
        }

        return "Unknown";
    }

    public static object ValidateCreditCard(string cardNumber)
    {
        if (LuhnAlgorithm(cardNumber))
        {
            string issuer = GetCardIssuer(cardNumber);
            return new { Valid = true, Bandeira = issuer };
        }
        
        return new { Valid = false, Bandeira = (string)null };
    }
}

// Exemplo de uso
public class Program
{
    public static void Main()
    {
        string cardNumber = "5000954942836955";
        var result = CardValidator.ValidateCreditCard(cardNumber);
        
        Console.WriteLine(result.ToString());
    }
}