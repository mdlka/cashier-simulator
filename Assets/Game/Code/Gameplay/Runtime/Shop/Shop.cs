using System.Collections;
using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    public class Shop : MonoBehaviour
    {
        private ShopDaySettings _shopDaySettings;

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
            int createdCustomers = 0;
            
            while (createdCustomers < _shopDaySettings.CostumersCount)
            {
                yield return new WaitUntil(() => _customersQueue.HasPlace);
                yield return new WaitForSeconds(Random.Range(2, 5));

                _customersQueue.Add(_customerFactory.CreateRandomCustomer(_shopDaySettings.ProductListFactory));
                createdCustomers += 1;
            }

            yield return new WaitUntil(() => _customersQueue.HasCustomers);

            WorkIsDone = true;
        }
    }
}