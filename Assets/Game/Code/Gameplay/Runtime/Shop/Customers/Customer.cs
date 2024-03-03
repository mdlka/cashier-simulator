using System;
using System.Collections;
using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    public class Customer : MonoBehaviour
    {
        [SerializeField] private float _speed;
        
        private IPayment _payment;
        private ProductList _productList;
        private Coroutine _movingCoroutine;

        public bool IsMoving => _movingCoroutine != null;
        
        public void Initialize(ProductList productList, IPayment payment)
        {
            _productList = productList;
            _payment = payment;
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
            {
                tape.Add(Instantiate(product));
                yield return null;
            }
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
