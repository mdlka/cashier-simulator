using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    [CreateAssetMenu(menuName = "Cashier Simulator/Create CustomerProductListFactory", fileName = "CustomerProductListFactory", order = 56)]
    public class CustomerProductListFactory : ScriptableObject
    {
        [SerializeField] private ProductsInventory _productsInventory;

        public CustomerProductList CreateRandomProducts(int maxProducts)
        {
            var products = new Product[Random.Range(1, maxProducts + 1)];
            _productsInventory.FillWithRandomOpenedProducts(products);

            return new CustomerProductList(products);
        }
    }
}