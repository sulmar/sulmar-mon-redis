namespace CurrencyCalculatorConsoleApp.Models;

public class ExchangeRate
{
    public string table { get; set; }
    public string currency { get; set; }
    public string code { get; set; }
    public Rate[] rates { get; set; }

    public override string ToString()
    {
        return $"{table} {currency} {code}: {rates[0]}";
    }
}

public class Rate
{
    public string no { get; set; }
    public DateOnly effectiveDate { get; set; }
    public decimal mid { get; set; }

    public override string ToString()
    {
        return $"{effectiveDate} {mid}";
    }
}