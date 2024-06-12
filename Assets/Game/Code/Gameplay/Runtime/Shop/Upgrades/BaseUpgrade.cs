using System;

namespace YellowSquad.CashierSimulator.Gameplay
{
    internal abstract class BaseUpgrade
    {
        [NonSerialized] private long _currentLevel;
        
        public long CurrentValue => ValueBy(_currentLevel);
        public long AppendValue => ValueBy(_currentLevel + 1) - CurrentValue;
        public abstract bool Max { get; }
        public abstract Currency Price { get; }

        protected long CurrentLevel => _currentLevel;

        public void Upgrade()
        {
            _currentLevel += 1;
        }

        protected abstract long ValueBy(long level);
    }
}