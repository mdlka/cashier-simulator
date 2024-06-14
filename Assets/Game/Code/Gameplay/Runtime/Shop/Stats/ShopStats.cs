using System;
using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    [CreateAssetMenu(menuName = "Cashier Simulator/Create ShopStats", fileName = "ShopStats", order = 56)]
    public class ShopStats : ScriptableObject
    {
        private ShopStatsData _shopStats;
        private bool _currentDaySaved;
        
        public ShopStatsData CurrentDay { get; private set; }

        public void Initialize()
        {
            _shopStats ??= new ShopStatsData();
        }

        public void StartDay()
        {
            CurrentDay = new ShopStatsData();
            _currentDaySaved = false;
        }

        public void SaveCurrentDay()
        {
            if (_currentDaySaved)
                throw new InvalidOperationException("Already saved");
            
            _shopStats += CurrentDay;
            _currentDaySaved = true;
        }
    }
}
