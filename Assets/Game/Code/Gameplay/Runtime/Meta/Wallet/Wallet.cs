using System;
using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    public class Wallet : MonoBehaviour
    {
        [SerializeField] private WalletView _view;
        
        public Currency CurrentValue { get; private set; }

        private void Awake()
        {
            _view.Render(CurrentValue);
        }

        public void Add(Currency value)
        {
            if (value.TotalCents < 0)
                throw new ArgumentOutOfRangeException(nameof(value));

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
                throw new ArgumentOutOfRangeException(nameof(value));
            
            return (CurrentValue - value).TotalCents >= 0;
        }
    }
}
