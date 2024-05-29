using System.Collections.Generic;

namespace YellowSquad.CashierSimulator.Gameplay
{
    public class CustomerProductList
    {
        private readonly Product[] _products;

        public CustomerProductList(Product[] products)
        {
            _products = products;
        }

        public IEnumerable<Product> Products => _products;
    }
}