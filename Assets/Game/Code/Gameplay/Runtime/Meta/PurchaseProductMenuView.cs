using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using YellowSquad.CashierSimulator.Gameplay.Useful;

namespace YellowSquad.CashierSimulator.Gameplay.Meta
{
    internal class PurchaseProductMenuView : MonoBehaviour
    {
        private readonly List<PurchaseProductView> _purchaseProductsInstances = new();

        [SerializeField] private Button _closeButton;
        [SerializeField] private Transform _productsViewContent;
        [SerializeField] private PurchaseProductView _purchaseProductViewTemplate;
        [SerializeField] private ProductsNames _productsNames;
        [SerializeField] private CanvasGroup _canvasGroup;

        private bool _rendered;
        private Action _onCloseButtonClick;

        private void OnEnable()
        {
            _closeButton.onClick.AddListener(OnCloseButtonClick);
        }

        private void OnDisable()
        {
            _closeButton.onClick.RemoveListener(OnCloseButtonClick);
        }
        
        internal void Render(IEnumerable<PurchaseProduct> products, IReadOnlyCollection<Product> openedProducts, Action onCloseButtonClick = null)
        {
            if (_rendered)
                throw new InvalidOperationException("Before this you need to call " + nameof(Close));

            _canvasGroup.Enable();
            _onCloseButtonClick = onCloseButtonClick;

            foreach (var product in products)
            {
                var viewInstance = Instantiate(_purchaseProductViewTemplate, _productsViewContent);
                viewInstance.Render(product, _productsNames.RuName(product.NameTag), 
                    opened: openedProducts.Any(p => p.NameTag == product.NameTag));
                
                _purchaseProductsInstances.Add(viewInstance);
            }
        }

        internal void Close()
        {
            foreach (var product in _purchaseProductsInstances)
                Destroy(product.gameObject);
            
            _purchaseProductsInstances.Clear();
            _rendered = false;
            
            _canvasGroup.Disable();
        }
        
        private void OnCloseButtonClick()
        {
            if (_rendered == false)
                return;
            
            _onCloseButtonClick?.Invoke();
        }
    }
}