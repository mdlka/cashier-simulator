using System;
using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    [Serializable]
    internal class ProductUpgrade
    {
        [SerializeField] private long _minValue;
        [SerializeField] private long _minUpgradePriceInCents;
        
        private long _level;

        public long CurrentValue => ValueBy(_level);
        public long AppendValue => ValueBy(_level + 1) - CurrentValue;
        public Currency Price => _minUpgradePriceInCents * (_level + 1);

        public void Upgrade()
        {
            _level += 1;
        }

        public long ValueBy(long level)
        {
            return _minValue * level;
        }
    }
}