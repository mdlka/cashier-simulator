namespace YellowSquad.CashierSimulator.Gameplay
{
    public static class CurrencyExtensions
    {
        public static string ToPriceTag(this Currency currency)
        {
            return $"<size=80%>$<size=100%>{currency.Dollars}.<size=80%>{currency.Cents:00}";
        }
    }
}