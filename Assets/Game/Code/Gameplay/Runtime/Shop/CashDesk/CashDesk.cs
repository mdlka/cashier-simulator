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

        public ProductScanner ProductScanner => _productScanner;
        public CashRegister CashRegister => _cashRegister;
        public PaymentTerminal PaymentTerminal => _paymentTerminal;
        
        public IEnumerator AcceptCustomer(Customer customer)
        {
            yield return new WaitUntil(() => customer.IsMoving == false);
            yield return new WaitForSeconds(1f);
            
            yield return customer.PlaceProducts(_productTape);

            yield return new WaitUntil(() => _productTape.HasProducts == false);

            Debug.Log($"Price: {_productScanner.ScannedProductsPrice}");

            yield return customer.StartPayment();

            if (customer.PaymentMethod == PaymentMethod.Cash)
            {
                float givingCash = Mathf.Ceil(_productScanner.ScannedProductsPrice / 10) * 10;
                float targetChange = givingCash - _productScanner.ScannedProductsPrice;
                
                Debug.Log($"Giving cash: {givingCash}, need - {targetChange} change");
                
                yield return _cashRegister.AcceptPayment(givingCash, targetChange);

                string rep = _cashRegister.CurrentChange >= targetChange ? "+rep" : "-rep";
                Debug.Log($"Target: {targetChange}, Current: {_cashRegister.CurrentChange} ({rep})");
            }
            else if (customer.PaymentMethod == PaymentMethod.Card)
            {
                _cameraMovement.MoveTo(_paymentTerminal.CameraPoint);
                
                yield return _paymentTerminal.AcceptPayment(_productScanner.ScannedProductsPrice);
                
                _cameraMovement.ReturnToBase();
            }
            
            _productScanner.Restart();
        }
    }
}