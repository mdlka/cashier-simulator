using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace YellowSquad.CashierSimulator.Gameplay
{
    [CreateAssetMenu(menuName = "Cashier Simulator/Create CustomerFactory", fileName = "CustomerFactory", order = 56)]
    public class CustomerFactory : ScriptableObject
    {
        [SerializeField] private Customer _customerTemplate;
        
        [NonSerialized] private Transform _customerContainer;

        public Customer CreateRandomCustomer(ProductListFactory productListFactory)
        {
            _customerContainer ??= new GameObject("Customers").transform;
            
            var customer = Instantiate(_customerTemplate, _customerContainer);
            customer.Initialize(productListFactory.CreateRandomProducts(), (PaymentMethod)Mathf.RoundToInt(Random.Range(0, 101) / 100f));

            return customer;
        }
    }
}