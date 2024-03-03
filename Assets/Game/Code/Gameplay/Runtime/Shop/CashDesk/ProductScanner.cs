using DG.Tweening;
using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    public class ProductScanner : MonoBehaviour
    {
        [SerializeField] private ProductTape _tape;
        [SerializeField] private Transform _scannedProductsPoint;
        [SerializeField, Min(0f)] private float _animationDuration;
        
        public float ScannedProductsPrice { get; private set; }
        
        public void Scan(Product product)
        {
            _tape.Remove(product);
            ScannedProductsPrice += product.PriceInDollars;

            product.transform.DOMove(_scannedProductsPoint.position, _animationDuration)
                .OnComplete(() => Destroy(product.gameObject));
        }

        public void Restart()
        {
            ScannedProductsPrice = 0;
        }
    }
}