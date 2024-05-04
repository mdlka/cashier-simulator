using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    public class ProductTape : MonoBehaviour
    {
        private readonly List<ProductPoint> _productPoints = new();
        
        [SerializeField] private float _animationDuration;
        [SerializeField] private Vector3 _pointsStartPosition;
        [SerializeField] private Vector2 _pointsOffset;
        [SerializeField, Min(0)] private Vector2Int _pointsCount;

        public bool HasProducts => _productPoints.Any(point => point.IsBusy);
        public bool HasFreePoint => _productPoints.Any(point => !point.IsBusy);

        private void Awake()
        {
            for (int i = 0; i < _pointsCount.y; i++)
            {
                for (int j = 0; j < _pointsCount.x; j++)
                {
                    var instance = new GameObject("ProductPoint").AddComponent<ProductPoint>();
                    instance.transform.position = _pointsStartPosition + new Vector3(_pointsOffset.x * j, 0, _pointsOffset.y * i);
                    instance.transform.SetParent(transform);
                    
                    _productPoints.Add(instance);
                }
            }
        }

        public IEnumerator Add(Product product)
        {
            var point = _productPoints.First(p => p.IsBusy == false);
            
            point.Take(product);
            product.transform.DOJump(point.Position, 0.5f, 1, _animationDuration);
            
            yield return new WaitForSeconds(_animationDuration);
        }

        public void Remove(Product product)
        {
            var targetPoint = _productPoints.First(point => point.Value == product);
            targetPoint.Free();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;

            for (int i = 0; i < _pointsCount.y; i++)
                for (int j = 0; j < _pointsCount.x; j++)
                    Gizmos.DrawSphere(_pointsStartPosition + new Vector3(_pointsOffset.x * j, 0, _pointsOffset.y * i), 0.1f);
        }
    }
}