using System.Collections;
using UnityEngine;
using YellowSquad.GamePlatformSdk;

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

        public bool WorkIsDone { get; private set; }
        public bool StatsShowing => _statsView.Opened;

        public void Initialize(ISave save)
        {
            _stats.Initialize(save);
            _productList.Initialize(save);
            DeactivateBoosts();
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
}