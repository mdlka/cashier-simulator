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
        [SerializeField] private ShopSettings _settings;
        [SerializeField] private ProductList _productList;
        [SerializeField] private ShopStats _stats;

        private ISave _save;

        public bool WorkIsDone { get; private set; }
        public bool StatsShowing => _statsView.Opened;

        public void Initialize(ISave save)
        {
            if (_save != null)
                throw new InvalidOperationException("Already initialized");
            
            _save = save;
            
            _stats.Initialize(save);
            _productList.Initialize(save);
            DeactivateBoosts();

            if (_save.HasKey(SaveConstants.ShopUpgradesSaveKey))
            {
                var shopUpgradesSave = JsonConvert.DeserializeObject<ShopUpgradesSave>(
                    _save.GetString(SaveConstants.ShopUpgradesSaveKey));
                
                _settings.PopularityUpgrade.Initialize(shopUpgradesSave.PopularityUpgradeLevel);
                _settings.CartCapacityUpgrade.Initialize(shopUpgradesSave.CartCapacityUpgradeLevel);
            }
            else
            {
                _settings.PopularityUpgrade.Initialize();
                _settings.CartCapacityUpgrade.Initialize();
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
                PopularityUpgradeLevel = _settings.PopularityUpgrade.CurrentLevel,
                CartCapacityUpgradeLevel = _settings.CartCapacityUpgrade.CurrentLevel
            };
            
            _save.SetString(SaveConstants.ShopUpgradesSaveKey, JsonConvert.SerializeObject(shopUpgradesSave));
        }

        public void DeactivateBoosts()
        {
            _productList.DeactivateBoosts();
            _settings.PopularityBoost.Deactivate();
            _settings.ProductsPriceBoost.Deactivate();
        }

        private IEnumerator Working()
        {
            _watch.Run(_settings.TimeSpeed);
            
            int createdCustomers = 0;
            int createCustomersHoursInterval = _settings.MinCostumersPerDay > 0 ? _watch.WorkingHours / (_settings.MinCostumersPerDay + 1) : -1;
            
            while (_watch.EndTimeReached == false)
            {
                yield return new WaitUntil(() => _customersQueue.HasPlace);
                
                if (_watch.EndTimeReached)
                    break;
                
                if (!NeedCreateCostumer())
                {
                    if (_settings.MaxCostumersPerHour == 0)
                        continue;

                    yield return new WaitForSeconds(_watch.HourDuration / _settings.MaxCostumersPerHour);
                
                    if (Random.Range(0f, 1f) > _settings.Popularity)
                        continue;
                }

                _customersQueue.Add(_customerFactory.CreateRandomCustomer(_settings.ProductListFactory, _settings.MaxCartCapacity));
                createdCustomers += 1;
            }

            yield return new WaitUntil(() => _watch.EndTimeReached && _customersQueue.HasCustomers == false);

            WorkIsDone = true;
            _stats.SaveCurrentDay();

            bool NeedCreateCostumer()
            {
                if (createCustomersHoursInterval == -1)
                    return false;
                
                return createdCustomers < _settings.MinCostumersPerDay &&
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