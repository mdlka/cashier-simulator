using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    [CreateAssetMenu(menuName = "Cashier Simulator/Create CustomerProductListFactory", fileName = "CustomerProductListFactory", order = 56)]
    public class CustomerProductListFactory : ScriptableObject
    {
        [SerializeField, Min(1)] private Vector2Int _minMaxProducts;
        [SerializeField] private ProductsInventory _productsInventory;

        public CustomerProductList CreateRandomProducts()
        {
            var products = new Product[Random.Range(_minMaxProducts.x, _minMaxProducts.y + 1)];

            for (int i = 0; i < products.Length; i++)
                products[i] = _productsInventory.RandomOpenedProduct();

            return new CustomerProductList(products);
        }
    }
}