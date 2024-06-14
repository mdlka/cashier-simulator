using DG.Tweening;
using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    public class ProductScanner : MonoBehaviour
    {
        [SerializeField] private ShopStats _shopStats;
        [SerializeField] private ProductTape _tape;
        [SerializeField] private ScannedProductsMonitor _productsMonitor;
        [SerializeField] private Transform _scannedProductsPoint;
        [SerializeField] private Transform _jumpPoint;
        [SerializeField, Min(0f)] private float _moveDuration;
        [SerializeField, Min(0f)] private float _jumpDuration;

        public Currency ScannedProductsPrice => _productsMonitor.ScannedProductsPrice;
        
        public void Scan(Product product)
        {
            _tape.Remove(product);
            var productPrice = _productsMonitor.Add(product);
            _shopStats.CurrentDayShopStats.Products.Add(product.NameTag, productPrice);

            var sequence = DOTween.Sequence();
            sequence.Append(product.transform.DOMove(_jumpPoint.position, _moveDuration));
            sequence.AppendCallback(() => product.transform.DOJump(_scannedProductsPoint.position, 0.6f, 1, _jumpDuration));
            sequence.AppendCallback(() => product.transform.DOScale(Vector3.zero, _jumpDuration));
            sequence.AppendInterval(_jumpDuration);
            sequence.AppendCallback(() => Destroy(product.gameObject));
        }

        public void Clear()
        {
            _productsMonitor.Clear();
        }
    }
}