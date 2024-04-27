using DG.Tweening;
using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    public class ProductScanner : MonoBehaviour
    {
        [SerializeField] private ProductTape _tape;
        [SerializeField] private ScannedProductsMonitor _productsMonitor;
        [SerializeField] private Transform _scannedProductsPoint;
        [SerializeField, Min(0f)] private float _animationDuration;

        public Currency ScannedProductsPrice => _productsMonitor.ScannedProductsPrice;
        
        public void Scan(Product product)
        {
            _tape.Remove(product);
            _productsMonitor.Add(product);

            product.transform.DOMove(_scannedProductsPoint.position, _animationDuration)
                .OnComplete(() => Destroy(product.gameObject));
        }

        public void Clear()
        {
            _productsMonitor.Clear();
        }
    }
}