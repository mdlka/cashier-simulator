using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using YellowSquad.CashierSimulator.Gameplay.Useful;
using Random = UnityEngine.Random;

namespace YellowSquad.CashierSimulator.Gameplay
{
    public class Customer : MonoBehaviour
    {
        private readonly List<Vector3> _targetMovePoints = new();
        
        [SerializeField] private float _speed;
        [SerializeField] private CustomerAnimator _animator;
        [SerializeField] private PaymentObject[] _paymentObjects;
        [SerializeField] private CustomerModel[] _models;
        [SerializeField] private Material[] _materials;
        
        private PaymentMethod _paymentMethod;
        private CustomerProductList _productList;
        private Coroutine _movingCoroutine;

        public bool IsMoving => _movingCoroutine != null;
        public PaymentMethod PaymentMethod => _paymentMethod;
        
        public void Initialize(CustomerProductList productList, PaymentMethod paymentMethod)
        {
            _productList = productList;
            _paymentMethod = paymentMethod;
            
            var targetModel = _models[Random.Range(0, _models.Length)];
            targetModel.gameObject.SetActive(true);
            targetModel.SetupMaterial(_materials[Random.Range(0, _materials.Length)]);
            
            foreach (var paymentObject in _paymentObjects)
                targetModel.SetupPaymentObject(paymentObject);
            
            _animator.Initialize(targetModel);
        }
        
        public void MoveThrough(Action onComplete = null, params Vector3[] positions)
        {
            if (positions.Length == 0)
                return;
            
            _targetMovePoints.AddRange(positions);

            if (_movingCoroutine != null)
                return;

            _movingCoroutine = StartCoroutine(Moving(onComplete));
        }

        public void RotateY(float rotationY, float duration = 0.2f)
        {
            transform.DOComplete(true);
            transform.DORotate(Vector3.up * rotationY, duration);
        }

        public IEnumerator PlaceProducts(ProductTape tape)
        {
            foreach (var product in _productList.Products)
            {
                yield return tape.Add(Instantiate(product, transform.position, Quaternion.identity));
                yield return new WaitUntil(() => tape.HasFreePoint);
            }
        }

        public void StartPayment()
        {
            var paymentObject = _paymentObjects.First(obj => obj.PaymentMethod == _paymentMethod);
            paymentObject.Enable();
            
            _animator.PutUpHand();
        }

        public void EndPayment()
        {
            _animator.PutDownHand();
        }

        private IEnumerator Moving(Action onComplete = null)
        {
            _animator.EnableMove();

            for (int i = 0; i < _targetMovePoints.Count; i++)
            {
                var targetPosition = _targetMovePoints[i];
                
                Vector3 startPosition = transform.position;
                float moveDuration = Vector3.Distance(startPosition, targetPosition) / _speed;
                float elapsedTime = 0;

                Vector3 moveDirection = (targetPosition.XZ() - startPosition.XZ()).normalized;
                RotateY(Quaternion.LookRotation(moveDirection).eulerAngles.y - 180f, 0.35f);

                while (elapsedTime <= moveDuration)
                {
                    transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);
                    elapsedTime += Time.deltaTime;

                    yield return null;
                }
            }

            transform.position = _targetMovePoints[^1];
            _targetMovePoints.Clear();

            _animator.DisableMove();
            onComplete?.Invoke();

            _movingCoroutine = null;
        }
    }
}
