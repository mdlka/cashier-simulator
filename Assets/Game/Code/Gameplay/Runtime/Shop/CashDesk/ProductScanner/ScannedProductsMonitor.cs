using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    internal class ScannedProductsMonitor : MonoBehaviour
    {
        private readonly List<ScannedProductPanel> _scannedProductsPanels = new();
        
        [SerializeField] private Transform _content;
        [SerializeField] private ScannedProductPanel _scannedProductPanelTemplate;
        [SerializeField] private TMP_Text _totalPriceText;
        
        public Currency ScannedProductsPrice { get; private set; }
        
        internal void Add(Product product)
        {
            ScannedProductsPrice += product.PriceInCents;
            _totalPriceText.text = ScannedProductsPrice.ToPriceTag();

            foreach (var productPanel in _scannedProductsPanels)
            {
                if (productPanel.TargetProductNameTag != product.NameTag) 
                    continue;
                
                productPanel.Render(product.NameTag, product.PriceInCents, productPanel.CurrentProductsCount + 1);
                return;
            }

            var scannedProductPanelInstance = Instantiate(_scannedProductPanelTemplate, _content);
            scannedProductPanelInstance.Render(product.NameTag, product.PriceInCents);
            
            _scannedProductsPanels.Add(scannedProductPanelInstance);
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