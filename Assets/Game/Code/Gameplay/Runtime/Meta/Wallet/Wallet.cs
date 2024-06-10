using System;
using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    public class Wallet : MonoBehaviour
    {
        [SerializeField, Min(0)] private long _startValueInCents;
        [SerializeField] private WalletView _view;
        
        public Currency CurrentValue { get; private set; }

        private void Awake()
        {
            CurrentValue = _startValueInCents;
            _view.Render(CurrentValue);
        }

        public void Add(Currency value)
        {
            if (value.TotalCents < 0)
                throw new ArgumentOutOfRangeException();
            
            CurrentValue += value;
            _view.Render(CurrentValue);
        }

        public void Spend(Currency value)
        {
            if (CanSpend(value) == false)
                throw new InvalidOperationException();

            CurrentValue -= value;
            _view.Render(CurrentValue);
        }

        public bool CanSpend(Currency value)
        {
            if (value.TotalCents < 0)
                throw new ArgumentOutOfRangeException();
            
            return CurrentValue.TotalCents - value.TotalCents >= 0;
        }
    }
}
