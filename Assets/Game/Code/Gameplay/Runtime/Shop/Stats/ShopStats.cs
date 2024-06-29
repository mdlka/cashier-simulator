using System;
using Newtonsoft.Json;
using UnityEngine;
using YellowSquad.GamePlatformSdk;

namespace YellowSquad.CashierSimulator.Gameplay
{
    [CreateAssetMenu(menuName = "Cashier Simulator/Create ShopStats", fileName = "ShopStats", order = 56)]
    public class ShopStats : ScriptableObject
    {
        [NonSerialized] private ShopStatsData _shopStats;
        private bool _currentDaySaved;
        private ISave _save;
        public ShopStatsData CurrentDay { get; private set; }

        public void Initialize(ISave save)
        {
            _save = save;
            
            _shopStats = _save.HasKey(SaveConstants.ShopStatsSaveKey) 
                    ? JsonConvert.DeserializeObject<ShopStatsData>(_save.GetString(SaveConstants.ShopStatsSaveKey))
                    : new ShopStatsData();
        }

        public void StartDay()
        {
            CurrentDay = new ShopStatsData(_shopStats.LastDayNumber + 1);
            _currentDaySaved = false;
        }

        public void SaveCurrentDay()
        {
            if (_currentDaySaved)
                throw new InvalidOperationException("Already saved");
            
            _shopStats += CurrentDay;
            _save.SetString(SaveConstants.ShopStatsSaveKey, JsonConvert.SerializeObject(_shopStats));
            _currentDaySaved = true;
        }
    }
}
