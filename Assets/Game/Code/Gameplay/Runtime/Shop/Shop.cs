using System.Collections;
using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    public class Shop : MonoBehaviour
    {
        [SerializeField] private JobWatch _watch;
        [SerializeField] private CustomersQueue _customersQueue;
        [SerializeField] private CustomerFactory _customerFactory;
        [SerializeField] private ShopSettings _settings;

        public bool WorkIsDone { get; private set; }

        public void StartDay()
        {
            WorkIsDone = false;
            StartCoroutine(Working());
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