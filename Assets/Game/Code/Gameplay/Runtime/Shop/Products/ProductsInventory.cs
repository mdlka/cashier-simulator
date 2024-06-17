using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YellowSquad.CashierSimulator.Gameplay.Useful;
using Random = UnityEngine.Random;

namespace YellowSquad.CashierSimulator.Gameplay
{
    [CreateAssetMenu(menuName = "Cashier Simulator/Create ProductsInventory", fileName = "ProductsInventory", order = 56)]
    public class ProductsInventory : ScriptableObject
    {
        private const int DefaultIndexesCountForRandom = 100;
        private const int PopularityBoostFactor = 2;
        
        [NonSerialized] private readonly List<Product> _openedProducts = new();

        [SerializeField] private ProductList _productList;
        [SerializeField] private Product _startOpenedProduct;

        public IReadOnlyList<Product> OpenedProducts => _openedProducts;

        public void Initialize()
        {
            _openedProducts.Add(_startOpenedProduct);
        }

        public void Add(Product product)
        {
            if (_openedProducts.Contains(product))
                throw new InvalidOperationException();
            
            _openedProducts.Add(product);
        }

        public void FillWithRandomOpenedProducts(Product[] buffer)
        {
            HashSet<string> openedProductsTags = OpenedProducts.Select(product => product.NameTag).ToHashSet();
            ProductInfo[] openedProducts = _productList.FindInfoBy(openedProductsTags);

            var openedProductsIndexes = new List<int>();
            
            for (int i = 0; i < openedProducts.Length; i++)
            {
                int[] indexes = new int[openedProducts[i].PopularityBoost.Active ? DefaultIndexesCountForRandom * PopularityBoostFactor : DefaultIndexesCountForRandom];
                Array.Fill(indexes, i);
                
                openedProductsIndexes.AddRange(indexes);
            }
            
            openedProductsIndexes.Shuffle();

            for (int i = 0; i < buffer.Length; i++)
                buffer[i] = openedProducts[openedProductsIndexes[Random.Range(0, openedProductsIndexes.Count)]].Product;
        }
    }
}