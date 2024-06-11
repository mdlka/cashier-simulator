using System.Collections;
using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    public class Shop : MonoBehaviour
    {
        private ShopDaySettings _shopDaySettings;

        [SerializeField] private JobWatch _watch;
        [SerializeField] private CustomersQueue _customersQueue;
        [SerializeField] private CustomerFactory _customerFactory;

        public bool WorkIsDone { get; private set; }

        public void StartDay(ShopDaySettings daySettings)
        {
            _shopDaySettings = daySettings;

            WorkIsDone = false;
            StartCoroutine(Working());
        }

        private IEnumerator Working()
        {
            _watch.Run(_shopDaySettings.TimeSpeed);
            
            int createdCustomers = 0;
            
            while (_watch.EndTimeReached == false)
            {
                yield return new WaitUntil(() => _customersQueue.HasPlace);
                
                if (_watch.EndTimeReached)
                    break;

                yield return new WaitForSeconds(_shopDaySettings.MaxCostumersPerHour / _shopDaySettings.TimeSpeed);
                
                if (Random.Range(0f, 1f) > _shopDaySettings.CreateCostumerChance)
                    continue;

                _customersQueue.Add(_customerFactory.CreateRandomCustomer(_shopDaySettings.ProductListFactory));
                createdCustomers += 1;
            }

            yield return new WaitUntil(() => _watch.EndTimeReached && _customersQueue.HasCustomers == false);

            WorkIsDone = true;
        }
    }
}