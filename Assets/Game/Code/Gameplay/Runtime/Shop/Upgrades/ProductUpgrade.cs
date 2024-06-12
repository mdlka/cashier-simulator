using System;
using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    [Serializable]
    internal class ProductUpgrade : BaseUpgrade
    {
        [SerializeField] private long _startValue;
        [SerializeField] private long _startUpgradePriceInCents;

        public override Currency Price => _startUpgradePriceInCents * (CurrentLevel + 1);
        public override bool Max => false;
        
        protected override long ValueBy(long level)
        {
            return _startValue + _startValue * level;
        }
    }
}