using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    public class CashRegister : MonoBehaviour
    {
        private readonly Queue<CashSlot> _slotsQueue = new();
        private readonly List<Cash> _cash = new();

        [SerializeField] private List<CashSlot> _slots;
        [SerializeField] private CashRegisterMonitor _monitor;
        [SerializeField] private Transform _cashPoint;
        [SerializeField] private Transform _cashBox;
        [SerializeField] private Vector3 _cashBoxCloseLocalPosition;
        [SerializeField] private Vector3 _cashBoxOpenLocalPosition;

        private bool _paymentEnded = true;
        
        public float CurrentChange { get; private set; }

        private void Awake()
        {
            _cashBox.localPosition = _cashBoxCloseLocalPosition;
            _slots.ForEach(slot => slot.gameObject.SetActive(false));
            
            _monitor.UpdateInfo();
        }

        public void Take(CashSlot slot)
        {
            if (_paymentEnded)
                return;
            
            _slotsQueue.Enqueue(slot);
        }

        public IEnumerator AcceptPayment(PaymentObject paymentCash, float givingCash, float productsPrice)
        {
            _paymentEnded = false;
            CurrentChange = 0;
            
            paymentCash.Destroy();
            
            _monitor.UpdateInfo(givingCash, productsPrice, CurrentChange);
            
            _slots.ForEach(slot => slot.gameObject.SetActive(true));
            _cashBox.DOLocalMove(_cashBoxOpenLocalPosition, 0.2f);

            while (_paymentEnded == false)
            {
                yield return new WaitUntil(() => _slotsQueue.Count > 0 || _paymentEnded);

                if (_paymentEnded)
                    yield break;

                var slot = _slotsQueue.Dequeue();
                var cash = slot.Take(_cashPoint.position);
                CurrentChange += cash.Value;
                
                _monitor.UpdateInfo(givingCash, productsPrice, CurrentChange);
                _cash.Add(cash);
            }
        }

        public void EndPayment()
        {
            _paymentEnded = true;

            foreach (var cash in _cash)
                Destroy(cash.gameObject);
            
            _cash.Clear();
            _slots.ForEach(slot => slot.gameObject.SetActive(false));
            _cashBox.DOLocalMove(_cashBoxCloseLocalPosition, 0.2f);
            
            _monitor.UpdateInfo();
        }
    }
}
