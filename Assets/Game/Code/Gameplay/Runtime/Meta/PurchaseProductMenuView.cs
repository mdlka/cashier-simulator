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
        [SerializeField] private ProductList _productList;
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
        
        internal void Render(IReadOnlyCollection<Product> openedProducts, Action onCloseButtonClick = null)
        {
            if (_rendered)
                throw new InvalidOperationException("Before this you need to call " + nameof(Close));

            _canvasGroup.Enable();
            _onCloseButtonClick = onCloseButtonClick;

            foreach (var product in _productList.Products)
            {
                var viewInstance = Instantiate(_purchaseProductViewTemplate, _productsViewContent);
                var info = _productList.FindInfoBy(product.NameTag);

                viewInstance.Render(info.Icon, info.PurchasePrice, info.RuName,
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