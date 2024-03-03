using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    [CreateAssetMenu(menuName = "Cashier Simulator/Create ProductListFactory", fileName = "ProductListFactory", order = 56)]
    public class ProductListFactory : ScriptableObject
    {
        [SerializeField] private Product[] _products;

        public ProductList CreateRandomProducts(int productsCount)
        {
            var products = new Product[productsCount];

            for (int i = 0; i < products.Length; i++)
                products[i] = _products[Random.Range(0, _products.Length)];

            return new ProductList(products);
        }
    }
}