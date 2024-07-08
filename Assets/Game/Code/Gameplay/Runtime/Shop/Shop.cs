using System;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine;
using YellowSquad.GamePlatformSdk;
using Random = UnityEngine.Random;

namespace YellowSquad.CashierSimulator.Gameplay
{
    public class Shop : MonoBehaviour
    {
        [SerializeField] private JobWatch _watch;
        [SerializeField] private CustomersQueue _customersQueue;
        [SerializeField] private ShopStatsView _statsView;
        [SerializeField] private CustomerFactory _customerFactory;
        [SerializeField] private GameSettings _gameSettings;
        [SerializeField] private ShopSettings _shopSettings;
        [SerializeField] private ProductList _productList;
        [SerializeField] private ShopStats _stats;

        private ISave _save;

        public bool WorkIsDone { get; private set; }
        public bool StatsShowing => _statsView.Opened;
        public int CurrentDay => (int)_stats.CurrentDay.LastDayNumber;

        public void Initialize(ISave save)
        {
            if (_save != null)
                throw new InvalidOperationException("Already initialized");
            
            _save = save;
            
            _stats.Initialize(save);
            _productList.Initialize(save);
            _gameSettings.Initialize(save, _watch);
            DeactivateBoosts();

            if (_save.HasKey(SaveConstants.ShopUpgradesSaveKey))
            {
                var shopUpgradesSave = JsonConvert.DeserializeObject<ShopUpgradesSave>(
                    _save.GetString(SaveConstants.ShopUpgradesSaveKey));
                
                _shopSettings.PopularityUpgrade.Initialize(shopUpgradesSave.PopularityUpgradeLevel);
                _shopSettings.CartCapacityUpgrade.Initialize(shopUpgradesSave.CartCapacityUpgradeLevel);
            }
            else
            {
                _shopSettings.PopularityUpgrade.Initialize();
                _shopSettings.CartCapacityUpgrade.Initialize();
            }
        }

        public void StartDay()
        {
            _stats.StartDay();

            WorkIsDone = false;
            StartCoroutine(Working());
        }

        public void ShowStats()
        {
            _statsView.Render(_stats.CurrentDay);
        }

        public void SaveProducts()
        {
            _productList.Save();
        }

        public void SaveShopUpgrades()
        {
            var shopUpgradesSave = new ShopUpgradesSave
            {
                PopularityUpgradeLevel = _shopSettings.PopularityUpgrade.CurrentLevel,
                CartCapacityUpgradeLevel = _shopSettings.CartCapacityUpgrade.CurrentLevel
            };
            
            _save.SetString(SaveConstants.ShopUpgradesSaveKey, JsonConvert.SerializeObject(shopUpgradesSave));
        }

        public void DeactivateBoosts()
        {
            _productList.DeactivateBoosts();
            _shopSettings.PopularityBoost.Deactivate();
            _shopSettings.ProductsPriceBoost.Deactivate();
        }

        private IEnumerator Working()
        {
            _watch.Run(_shopSettings.TimeSpeed, _shopSettings.TimeFactorWhenNoCustomers, needSpeedUp: () => _customersQueue.HasCustomers == false);

            int totalCustomers = 0;
            int createdCustomers = 0;
            int createCustomersHoursInterval = _shopSettings.MinCostumersPerDay > 0 ? _watch.WorkingHours / (_shopSettings.MinCostumersPerDay + 1) : -1;
            
            while (_watch.EndTimeReached == false)
            {
                yield return new WaitUntil(() => _watch.Stopped == false);
                
                if (_watch.EndTimeReached)
                    break;
                
                if (!NeedCreateCostumer())
                {
                    if (_shopSettings.MaxCostumersPerHour == 0)
                        continue;

                    if (Random.Range(0f, 1f) > 0.5f)
                        continue;
                }

                yield return new WaitUntil(() => _watch.Stopped == false);

                if (_customersQueue.HasPlace)
                {
                    _customersQueue.Add(_customerFactory.CreateRandomCustomer(_shopSettings.ProductListFactory, _shopSettings.MaxCartCapacity));
                    createdCustomers += 1;
                }
                
                totalCustomers += 1;

                float timeInMinutes = _watch.ElapsedTimeInMinutes;
                yield return new WaitUntil(() => _watch.ElapsedTimeInMinutes - timeInMinutes >=
                    60f / (_shopSettings.MaxCostumersPerHour * _shopSettings.Popularity) || _watch.EndTimeReached);
            }

            Debug.Log($"{_shopSettings.Popularity * 100}% - {totalCustomers}");
            yield return new WaitUntil(() => _watch.EndTimeReached && _customersQueue.HasCustomers == false);

            WorkIsDone = true;
            _stats.SaveCurrentDay();

            bool NeedCreateCostumer()
            {
                if (createCustomersHoursInterval == -1 || _shopSettings.MaxCostumersPerHour == 0)
                    return false;
                
                return createdCustomers < _shopSettings.MinCostumersPerDay &&
                       _watch.ElapsedHours >= createCustomersHoursInterval * (createdCustomers + 1);
            }
        }
    }

    [Serializable]
    internal class ShopUpgradesSave
    {
        [JsonProperty] public long PopularityUpgradeLevel { get; init; }
        [JsonProperty] public long CartCapacityUpgradeLevel { get; init; }
    }
}