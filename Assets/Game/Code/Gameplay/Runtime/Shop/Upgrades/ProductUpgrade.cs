using System;
using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    [Serializable]
    internal class ProductUpgrade : BaseUpgrade
    {
        private const int MaxLevel = 1000;
        private const int UpgradePriceFactor = 10;

        [SerializeField] private long _startValue;

        public override Currency Price => PriceHyperbola(_startValue, CurrentLevel + 1, UpgradePriceFactor);
        public override bool Max => CurrentLevel >= MaxLevel;
        
        protected override long ValueBy(long level)
        {
            return PriceHyperbola(_startValue, level);
        }
        
        private long PriceHyperbola(long startValue, long level, float startValueFactor = 1)
        {
            const int a = 30;
            const float b = 0.15f;
            const int k = 100;

            // a, b, k - math variables. I don't know any other name for them. Hyperbola: y = k / (x + a) + b, where x = level

            return (long)(Mathf.Round(startValue * startValueFactor 
                                     + level * ((k + startValue * startValueFactor) / (level + a) 
                                                + startValue * startValueFactor * b)));
        }
    }
}