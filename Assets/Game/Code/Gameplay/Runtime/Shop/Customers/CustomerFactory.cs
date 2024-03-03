using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace YellowSquad.CashierSimulator.Gameplay
{
    [CreateAssetMenu(menuName = "Cashier Simulator/Create CustomerFactory", fileName = "CustomerFactory", order = 56)]
    public class CustomerFactory : ScriptableObject
    {
        [SerializeField] private Customer[] _customers;
        
        [NonSerialized] private Transform _customerContainer;

        public Customer CreateRandomCustomer(ProductListFactory productListFactory)
        {
            _customerContainer ??= new GameObject("Customers").transform;
            
            var customer = Instantiate(_customers[Random.Range(0, _customers.Length)], _customerContainer);
            customer.Initialize(productListFactory.CreateRandomProducts(), PaymentMethod.Cash);

            return customer;
        }
    }
}