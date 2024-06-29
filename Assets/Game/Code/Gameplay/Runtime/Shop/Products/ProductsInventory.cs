using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using YellowSquad.CashierSimulator.Gameplay.Useful;
using YellowSquad.GamePlatformSdk;
using Random = UnityEngine.Random;

namespace YellowSquad.CashierSimulator.Gameplay
{
    [CreateAssetMenu(menuName = "Cashier Simulator/Create ProductsInventory", fileName = "ProductsInventory", order = 56)]
    public class ProductsInventory : ScriptableObject
    {
        private const int DefaultIndexesCountForRandom = 100;
        private const int PopularityBoostFactor = 2;
        
        [NonSerialized] private readonly HashSet<string> _openedProducts = new();

        [SerializeField] private ProductList _productList;
        [SerializeField] private Product _startOpenedProduct;

        private ISave _save;

        public IReadOnlyCollection<string> OpenedProducts => _openedProducts;

        public void Initialize(ISave save)
        {
            _save = save;

            if (_save.HasKey(SaveConstants.OpenedProductsSaveKey))
            {
                var savedOpenedProducts = JsonConvert.DeserializeObject<HashSet<string>>(
                    _save.GetString(SaveConstants.OpenedProductsSaveKey));

                foreach (string openedProduct in savedOpenedProducts)
                    _openedProducts.Add(openedProduct);
            }
            else
            {
                _openedProducts.Add(_startOpenedProduct.NameTag);
            }
            
            _openedProducts.Add(_startOpenedProduct.NameTag);
        }

        public void Add(Product product)
        {
            if (_openedProducts.Contains(product.NameTag))
                throw new InvalidOperationException();
            
            _openedProducts.Add(product.NameTag);
            _save.SetString(SaveConstants.OpenedProductsSaveKey, JsonConvert.SerializeObject(_openedProducts));
        }

        public void FillWithRandomOpenedProducts(Product[] buffer)
        {
            ProductInfo[] openedProducts = _productList.FindInfoBy(_openedProducts);
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