using System;
using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    [Serializable]
    internal class ProductUpgrade
    {
        [SerializeField] private long _startValue;
        [SerializeField] private long _startUpgradePriceInCents;
        
        [NonSerialized] private long _level;

        public long CurrentValue => ValueBy(_level);
        public long AppendValue => ValueBy(_level + 1) - CurrentValue;
        public Currency Price => _startUpgradePriceInCents * (_level + 1);

        public void Upgrade()
        {
            _level += 1;
        }

        private long ValueBy(long level)
        {
            return _startValue + _startValue * level;
        }
    }
}