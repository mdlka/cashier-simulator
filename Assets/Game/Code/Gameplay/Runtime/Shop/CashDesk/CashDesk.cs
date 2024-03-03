using System.Collections;
using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    public class CashDesk : MonoBehaviour
    {
        [SerializeField] private ProductTape _productTape;
        
        public IEnumerator AcceptCustomer(Customer customer)
        {
            yield return new WaitUntil(() => customer.IsMoving == false);
            yield return customer.PlaceProducts(_productTape);

            while (_productTape.HasProducts)
            {
                yield return null;
            }
            
            yield return customer.Payment();
        }
    }
}