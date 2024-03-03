using System;
using System.Collections;
using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    public class Customer : MonoBehaviour
    {
        [SerializeField] private float _speed;
        
        private PaymentMethod _paymentMethod;
        private ProductList _productList;
        private Coroutine _movingCoroutine;

        public bool IsMoving => _movingCoroutine != null;
        
        public void Initialize(ProductList productList, PaymentMethod paymentMethod)
        {
            _productList = productList;
            _paymentMethod = paymentMethod;
        }

        public void MoveTo(Vector3 position, Action onComplete = null)
        {
            if (_movingCoroutine != null)
                throw new InvalidOperationException();

            _movingCoroutine = StartCoroutine(Moving(position, onComplete));
        }

        public IEnumerator PlaceProducts(ProductTape tape)
        {
            foreach (var product in _productList.Products)
                yield return tape.Add(Instantiate(product, transform.position, Quaternion.identity));
        }

        public IEnumerator Payment()
        {
            yield return null;
        }

        private IEnumerator Moving(Vector3 targetPosition, Action onComplete = null)
        {
            Vector3 startPosition = transform.position;
            float moveDuration = Vector3.Distance(startPosition, targetPosition) / _speed;
            float elapsedTime = 0;

            while (elapsedTime <= moveDuration)
            {
                transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);
                elapsedTime += Time.deltaTime;

                yield return null;
            }

            transform.position = targetPosition;
            onComplete?.Invoke();

            _movingCoroutine = null;
        }
    }
}
