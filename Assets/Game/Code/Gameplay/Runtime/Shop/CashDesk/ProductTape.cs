using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    public class ProductTape : MonoBehaviour
    {
        [SerializeField] private float _animationDuration;
        [SerializeField] private ProductPoint[] _productPoints;

        public bool HasProducts => _productPoints.Any(point => point.IsBusy);

        public void Add(Product product)
        {
            var point = _productPoints.First(p => p.IsBusy == false);
            
            point.Take(product);
            product.transform.DOJump(point.Position, 3f, 1, _animationDuration);
        }

        public void Remove(Product product)
        {
            var targetPoint = _productPoints.First(point => point.Value == product);
            targetPoint.Free();
        }
    }
}