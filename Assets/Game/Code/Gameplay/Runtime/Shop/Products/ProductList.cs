using System.Collections.Generic;

namespace YellowSquad.CashierSimulator.Gameplay
{
    public class ProductList
    {
        private readonly Product[] _products;

        public ProductList(Product[] products)
        {
            _products = products;
        }

        public IEnumerable<Product> Products => _products;
    }
}