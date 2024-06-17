using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    internal class ScannedProductsMonitor : MonoBehaviour
    {
        private readonly List<ScannedProductPanel> _scannedProductsPanels = new();
        
        [SerializeField] private Transform _content;
        [SerializeField] private ProductList _productList;
        [SerializeField] private ScannedProductPanel _scannedProductPanelTemplate;
        [SerializeField] private TMP_Text _totalPriceText;
        
        public Currency ScannedProductsPrice { get; private set; }
        
        internal Currency Add(Product product, long priceFactor)
        {
            Currency productPrice = _productList.FindInfoBy(product.NameTag).Price * priceFactor;
            
            ScannedProductsPrice += productPrice;
            _totalPriceText.text = ScannedProductsPrice.ToPriceTag();

            foreach (var productPanel in _scannedProductsPanels)
            {
                if (productPanel.TargetProductNameTag != product.NameTag) 
                    continue;
                
                productPanel.Render(product.NameTag, productPrice, productPanel.CurrentProductsCount + 1);
                return productPrice;
            }

            var scannedProductPanelInstance = Instantiate(_scannedProductPanelTemplate, _content);
            scannedProductPanelInstance.Render(product.NameTag, productPrice);
            
            _scannedProductsPanels.Add(scannedProductPanelInstance);
            return productPrice;
        }

        internal void Clear()
        {
            ScannedProductsPrice = Currency.Zero;

            foreach (var productPanel in _scannedProductsPanels)
                Destroy(productPanel.gameObject);
            
            _scannedProductsPanels.Clear();
            _totalPriceText.text = ScannedProductsPrice.ToPriceTag();
        }
    }
}