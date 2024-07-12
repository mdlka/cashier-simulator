using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using YellowSquad.CashierSimulator.Gameplay.Useful;

namespace YellowSquad.CashierSimulator.Gameplay.Meta
{
    internal class PurchaseProductListView : MonoBehaviour
    {
        private readonly List<PurchaseProductView> _purchaseProductsInstances = new();

        [SerializeField] private Button _closeButton;
        [SerializeField] private Transform _productsViewContent;
        [SerializeField] private PurchaseProductView _purchaseProductViewTemplate;
        [SerializeField] private ProductList _productList;
        [SerializeField] private CanvasGroup _canvasGroup;

        private bool _rendered;
        private Action _onCloseButtonClick;
        private Action<ProductInfo> _onProductSelect;

        private void OnEnable()
        {
            _closeButton.onClick.AddListener(OnCloseButtonClick);
            
            foreach (var product in _purchaseProductsInstances)
                product.Clicked += OnProductClick;
        }

        private void OnDisable()
        {
            _closeButton.onClick.RemoveListener(OnCloseButtonClick);
            
            foreach (var product in _purchaseProductsInstances)
                product.Clicked -= OnProductClick;
        }
        
        internal void Render(IReadOnlyCollection<string> openedProducts, Action onCloseButtonClick = null, Action<ProductInfo> onProductSelect = null, float openDuration = 0.2f)
        {
            if (_rendered)
                throw new InvalidOperationException("Before this you need to call " + nameof(Close));
            
            _canvasGroup.Enable(openDuration);
            _onCloseButtonClick = onCloseButtonClick;
            _onProductSelect = onProductSelect;

            foreach (var product in _productList.Products)
            {
                var viewInstance = Instantiate(_purchaseProductViewTemplate, _productsViewContent);

                viewInstance.Clicked += OnProductClick;
                viewInstance.Render(_productList.FindInfoBy(product.NameTag), 
                    opened: openedProducts.Contains(product.NameTag));
                
                _purchaseProductsInstances.Add(viewInstance);
            }

            _rendered = true;
        }

        internal void UpdateRender(ProductInfo product, bool opened)
        {
            _purchaseProductsInstances.First(view => view.ProductInfo.Product.NameTag == product.Product.NameTag)
                .Render(product, opened);
        }

        internal void Close(float duration = 0.2f)
        {
            foreach (var product in _purchaseProductsInstances)
            {
                product.Clicked -= OnProductClick;
                Destroy(product.gameObject);
            }
            
            _purchaseProductsInstances.Clear();
            _rendered = false;
            
            _canvasGroup.Disable(duration);
        }

        private void OnProductClick(PurchaseProductView productView)
        {
            _onProductSelect?.Invoke(productView.ProductInfo);
        }
        
        private void OnCloseButtonClick()
        {
            if (_rendered == false)
                return;
            
            _onCloseButtonClick?.Invoke();
        }
    }
}