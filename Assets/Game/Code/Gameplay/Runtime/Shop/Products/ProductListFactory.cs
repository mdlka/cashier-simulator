using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    [CreateAssetMenu(menuName = "Cashier Simulator/Create ProductListFactory", fileName = "ProductListFactory", order = 56)]
    public class ProductListFactory : ScriptableObject
    {
        [SerializeField, Min(1)] private Vector2Int _minMaxProducts;
        [SerializeField] private ProductsInventory _productsInventory;

        public ProductList CreateRandomProducts()
        {
            var products = new Product[Random.Range(_minMaxProducts.x, _minMaxProducts.y + 1)];

            for (int i = 0; i < products.Length; i++)
                products[i] = _productsInventory.RandomOpenedProduct();

            return new ProductList(products);
        }
    }
}