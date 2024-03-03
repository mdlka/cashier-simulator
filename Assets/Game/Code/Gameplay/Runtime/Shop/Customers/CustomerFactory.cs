using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    [CreateAssetMenu(menuName = "Cashier Simulator/Create CustomerFactory", fileName = "CustomerFactory", order = 56)]
    public class CustomerFactory : ScriptableObject
    {
        private Transform _customerContainer;
        
        [SerializeField] private Customer[] _customers;

        public Customer CreateRandomCustomer(ProductListFactory productListFactory)
        {
            _customerContainer ??= new GameObject("Customers").transform;
            
            var customer = Instantiate(_customers[Random.Range(0, _customers.Length)], _customerContainer);
            customer.Initialize(productListFactory.CreateRandomProducts(Random.Range(2, 5)), null);

            return customer;
        }
    }
}