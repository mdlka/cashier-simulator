using System;
using Newtonsoft.Json;
using UnityEngine;
using YellowSquad.GamePlatformSdk;

namespace YellowSquad.CashierSimulator.Gameplay
{
    public class Wallet : MonoBehaviour, IReadOnlyWallet
    {
        [SerializeField, Min(0)] private long _startValueInCents;
        [SerializeField] private WalletView _view;
        
        private ISave _save;

        public Currency CurrentValue { get; private set; }

        public void Initialize(ISave save)
        {
            _save = save;
            
            CurrentValue = _save.HasKey(SaveConstants.WalletValueSaveKey) 
                ? JsonConvert.DeserializeObject<Currency>(_save.GetString(SaveConstants.WalletValueSaveKey)) 
                : _startValueInCents;
            
            _view.Render(CurrentValue, Currency.Zero);
        }

        public void Add(Currency value)
        {
            if (value.TotalCents < 0)
                throw new ArgumentOutOfRangeException();
            
            CurrentValue += value;
            
            _save.SetString(SaveConstants.WalletValueSaveKey, JsonConvert.SerializeObject(CurrentValue));
            _view.Render(CurrentValue, value);
        }

        public void Spend(Currency value)
        {
            if (CanSpend(value) == false)
                throw new InvalidOperationException();

            CurrentValue -= value;
            
            _save.SetString(SaveConstants.WalletValueSaveKey, JsonConvert.SerializeObject(CurrentValue));
            _view.Render(CurrentValue, value, Sign.Minus);
        }

        public bool CanSpend(Currency value)
        {
            if (value.TotalCents < 0)
                throw new ArgumentOutOfRangeException();
            
            return CurrentValue.TotalCents - value.TotalCents >= 0;
        }
    }

    public interface IReadOnlyWallet
    {
        Currency CurrentValue { get; }
        bool CanSpend(Currency value);
    }
    
    public enum Sign
    {
        Plus,
        Minus
    }
}
