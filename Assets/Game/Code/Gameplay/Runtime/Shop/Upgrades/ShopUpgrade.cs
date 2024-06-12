using System;
using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    [Serializable]
    internal class ShopUpgrade : BaseUpgrade
    {
        [SerializeField] private long _startValue;
        [SerializeField] private long _maxValue;
        [SerializeField] private long _startUpgradePriceInCents;

        public override bool Max => CurrentValue >= _maxValue;
        public override Currency Price => _startUpgradePriceInCents * (CurrentLevel + 1);
        
        protected override long ValueBy(long level)
        {
            return Math.Min(_startValue * (level + 1), _maxValue);
        }
    }
}