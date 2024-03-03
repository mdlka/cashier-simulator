using System.Collections;
using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    public class CashDesk : MonoBehaviour
    {
        [SerializeField] private ProductTape _productTape;
        [SerializeField] private ProductScanner _productScanner;
        
        public IEnumerator AcceptCustomer(Customer customer)
        {
            yield return new WaitUntil(() => customer.IsMoving == false);
            yield return new WaitForSeconds(1f);
            
            yield return customer.PlaceProducts(_productTape);

            yield return new WaitUntil(() => _productTape.HasProducts == false);

            Debug.Log($"Price: {_productScanner.ScannedProductsPrice}");
            
            yield return customer.Payment();
            
            _productScanner.Restart();
        }
    }
}