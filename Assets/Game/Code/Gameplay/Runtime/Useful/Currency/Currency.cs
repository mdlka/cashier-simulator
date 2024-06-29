using System;
using System.Linq;
using Newtonsoft.Json;

namespace YellowSquad.CashierSimulator.Gameplay
{
    [Serializable]
    public readonly struct Currency
    {
        public static Currency Zero { get; } = new(0);

        public Currency(params Currency[] currencies) 
            : this(currencies.Sum(currency => currency.TotalCents)) 
        { }
        
        public Currency(long cents)
        {
            if (cents < 0)
                throw new ArgumentOutOfRangeException(nameof(cents) + " must be positive");
            
            TotalCents = cents;
        }

        [JsonProperty("TotalCents")] public long TotalCents { get; init; }
        [JsonIgnore] public long Dollars => TotalCents / 100;
        [JsonIgnore] public long Cents => TotalCents % 100;

        public override string ToString()
        {
            return $"{Dollars}.{Cents:00}";
        }

        public bool Equals(Currency other) => this == other;
        public override bool Equals(object obj) => obj is Currency other && Equals(other);
        public override int GetHashCode() => TotalCents.GetHashCode();

        public static implicit operator Currency(long value) => new(value);
        
        public static bool operator ==(Currency currency1, Currency currency2) => currency1.TotalCents == currency2.TotalCents;
        public static bool operator !=(Currency currency1, Currency currency2) => !(currency1 == currency2);
        public static Currency operator +(Currency currency1, Currency currency2) => new(currency1.TotalCents + currency2.TotalCents);
        public static Currency operator -(Currency currency1, Currency currency2) => new(currency1.TotalCents - currency2.TotalCents);
        public static Currency operator /(Currency currency, long value) => new(currency.TotalCents / value);
        public static Currency operator *(Currency currency, long value) => new(currency.TotalCents * value);
    }
}