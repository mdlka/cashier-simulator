using System;
using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    [CreateAssetMenu(menuName = "Cashier Simulator/Create ShopStats", fileName = "ShopStats", order = 56)]
    public class ShopStats : ScriptableObject
    {
        private ShopStatsData _shopStats;
        private bool _currentDaySaved;
        
        public ShopStatsData CurrentDayShopStats { get; private set; }

        public void Initialize()
        {
            _shopStats ??= new ShopStatsData();
        }

        public void StartDay()
        {
            CurrentDayShopStats = new ShopStatsData();
            _currentDaySaved = false;
        }

        public void SaveCurrentDay()
        {
            if (_currentDaySaved)
                throw new InvalidOperationException("Already saved");
            
            _shopStats += CurrentDayShopStats;
            _currentDaySaved = true;
        }
    }
}
