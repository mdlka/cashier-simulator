using System;
using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    [Serializable]
    internal class ShopUpgrade : BaseUpgrade
    {
        private const float Factor = 1.3f;
        
        [SerializeField] private long _startValue;
        [SerializeField] private long _maxValue;
        [SerializeField] private long _startUpgradePriceInCents;

        public override bool Max => CurrentValue >= _maxValue;
        public override Currency Price => (long)(_startUpgradePriceInCents * Math.Exp(CurrentLevel * Factor));
        
        protected override long ValueBy(long level)
        {
            return Math.Min(_startValue * (level + 1), _maxValue);
        }
    }
}