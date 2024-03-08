using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    public class CashRegister : MonoBehaviour
    {
        private readonly Queue<CashSlot> _slotsQueue = new();
        private readonly List<Cash> _cash = new();
            
        [SerializeField] private Transform _cashPoint;

        private bool _paymentEnded = true;
        
        public float CurrentChange { get; private set; }
        
        public void Take(CashSlot slot)
        {
            if (_paymentEnded)
                return;
            
            _slotsQueue.Enqueue(slot);
        }

        public IEnumerator AcceptPayment(float givingCash, float targetChange)
        {
            _paymentEnded = false;
            CurrentChange = 0;

            while (_paymentEnded == false)
            {
                yield return new WaitUntil(() => _slotsQueue.Count > 0 || _paymentEnded);

                if (_paymentEnded)
                    yield break;

                var slot = _slotsQueue.Dequeue();
                var cash = slot.Take(_cashPoint.position);
                CurrentChange += cash.Value;
                
                _cash.Add(cash);
            }
        }

        public void EndPayment()
        {
            _paymentEnded = true;

            foreach (var cash in _cash)
                Destroy(cash.gameObject);
            
            _cash.Clear();
        }
    }
}
