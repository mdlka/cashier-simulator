using System;

namespace YellowSquad.CashierSimulator.Gameplay
{
    internal abstract class BaseUpgrade
    {
        [NonSerialized] private bool _initialized;
        
        public long CurrentValue => ValueBy(CurrentLevel);
        public long NextValue => ValueBy(CurrentLevel + 1);
        public long AppendValue => NextValue - CurrentValue;
        [field: NonSerialized] public long CurrentLevel { get; private set; }
        
        public abstract bool Max { get; }
        public abstract Currency Price { get; }

        public void Upgrade()
        {
            CurrentLevel += 1;
        }

        protected abstract long ValueBy(long level);

        internal void Initialize(long startLevel = 0)
        {
            if (_initialized)
                throw new InvalidOperationException();
            
            CurrentLevel = startLevel;
            _initialized = true;
        }
    }
}