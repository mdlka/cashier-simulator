using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace YellowSquad.CashierSimulator.Gameplay
{
    internal class CashPaymentCalculator
    {
        private const float ExactChangeChance = 0.15f;
        private const int BillsOffset = 2;
        
        private readonly Currency[] _bills = 
        {
            new Currency(1),
            new Currency(5),
            new Currency(10),
            new Currency(25),
            new Currency(50),
            new Currency(100),
            new Currency(500),
            new Currency(1000),
            new Currency(2000),
            new Currency(3000),
            new Currency(4000),
            new Currency(5000),
            new Currency(7500),
            new Currency(10000),
        };
        
        public Currency Calculate(Currency targetPrice)
        {
            if (Random.Range(0f, 1f) <= ExactChangeChance)
                return targetPrice;
            
            if (targetPrice.TotalCents > _bills[^1].TotalCents)
                return (long)(Mathf.Ceil(targetPrice.TotalCents / 100f) * 100);

            int index = IndexMinSuitableBills(targetPrice);
            return _bills[Random.Range(index, Math.Min(index + BillsOffset, _bills.Length))];
        }

        private int IndexMinSuitableBills(Currency targetPrice)
        {
            for (int i = 0; i < _bills.Length; i++)
                if (targetPrice.TotalCents <= _bills[i].TotalCents)
                    return i;

            throw new InvalidOperationException();
        }
    }
}