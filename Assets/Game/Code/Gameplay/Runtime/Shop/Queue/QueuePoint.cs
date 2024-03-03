using System;
using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    public class QueuePoint : MonoBehaviour
    {
        private Customer _customer;

        public bool IsBusy => _customer != null;
        public Customer Customer => _customer;
        public Vector3 Position => transform.position;
        
        public void Take(Customer customer)
        {
            if (IsBusy)
                throw new InvalidOperationException();
            
            _customer = customer;
        }

        public void Free()
        {
            if (IsBusy == false)
                throw new InvalidOperationException();
            
            _customer = null;
        }
    }
}