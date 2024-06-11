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

            for (int i = 0; i < products.Length; i++)
                products[i] = _productsInventory.RandomOpenedProduct();

            return new CustomerProductList(products);
        }
    }
}