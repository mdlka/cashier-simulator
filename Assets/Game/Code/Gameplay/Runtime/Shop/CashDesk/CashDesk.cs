using System.Collections;
using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    public class CashDesk : MonoBehaviour
    {
        [SerializeField] private CameraMovement _cameraMovement;
        [SerializeField] private ProductTape _productTape;
        [SerializeField] private ProductScanner _productScanner;
        [SerializeField] private CashRegister _cashRegister;
        [SerializeField] private PaymentTerminal _paymentTerminal;
        [SerializeField] private GameObject _paperboard;

        private PaymentObject _currentPaymentObject;

        public ProductScanner ProductScanner => _productScanner;
        public CashRegister CashRegister => _cashRegister;
        public PaymentTerminal PaymentTerminal => _paymentTerminal;

        private void Awake()
        {
            _productScanner.Clear();
            _paperboard.SetActive(false);
        }

        public void AcceptPaymentObject(PaymentObject paymentObject)
        {
            _currentPaymentObject = paymentObject;
        }
        
        public IEnumerator AcceptCustomer(Customer customer)
        {
            _currentPaymentObject = null;
            
            _paperboard.SetActive(true);

            yield return customer.PlaceProducts(_productTape);
            yield return new WaitUntil(() => _productTape.HasProducts == false);

            customer.StartPayment();

            yield return new WaitUntil(() => _currentPaymentObject != null);
            
            customer.EndPayment();

            if (customer.PaymentMethod == PaymentMethod.Cash)
            {
                var givingCash = new Currency((long)(Mathf.Ceil(_productScanner.ScannedProductsPrice.TotalCents / 10f) * 10));
                
                yield return _cashRegister.AcceptPayment(_currentPaymentObject, givingCash, _productScanner.ScannedProductsPrice);

                var targetChange = givingCash - _productScanner.ScannedProductsPrice;
                string rep = _cashRegister.CurrentChange == targetChange ? "+rep" : "-rep";
                Debug.Log($"Target: {targetChange}, Current: {_cashRegister.CurrentChange} ({rep})");
            }
            else if (customer.PaymentMethod == PaymentMethod.Card)
            {
                _cameraMovement.MoveTo(_paymentTerminal.CameraPoint);
                
                yield return _paymentTerminal.AcceptPayment(_currentPaymentObject, _productScanner.ScannedProductsPrice);
                
                _cameraMovement.ReturnToBase();
            }
            
            _productScanner.Clear();
            _paperboard.SetActive(false);
        }
    }
}