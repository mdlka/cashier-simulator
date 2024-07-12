using System;
using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    [Serializable]
    internal class ProductUpgrade : BaseUpgrade
    {
        private const int MaxLevel = 1000;
        private const float UpgradePriceBasePow = 1.0027f;
        private const float UpgradePriceFactor = 0.5f;

        [SerializeField] private long _startValue;

        public override bool Max => CurrentLevel >= MaxLevel;
        public override Currency Price => (long)(_startValue * UpgradePriceFactor * (CurrentLevel + 1) 
                                                 * Math.Pow(UpgradePriceBasePow, CurrentLevel + 1));

        protected override long ValueBy(long level)
        {
            return PriceHyperbola(_startValue, level);
        }
        
        private long PriceHyperbola(long startValue, long level, float startValueFactor = 1)
        {
            const int a = 30;
            const float b = 0.1f;
            const int k = 100;

            // a, b, k - math variables. I don't know any other name for them. Hyperbola: y = k / (x + a) + b, where x = level

            return (long)(Mathf.Round(startValue * startValueFactor 
                                     + level * ((k + startValue * startValueFactor) / (level + a) 
                                                + startValue * b)));
        }
    }
}