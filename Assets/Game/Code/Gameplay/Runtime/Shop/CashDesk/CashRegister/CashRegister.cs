using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    public class CashRegister : MonoBehaviour
    {
        private const float ChangePercentDifferenceForCanEnd = 20;
        
        private readonly Queue<(SlotAction, CashSlot)> _slotsQueue = new();
        private readonly List<Cash> _cash = new();

        [SerializeField] private List<CashSlot> _slots;
        [SerializeField] private CashRegisterMonitor _monitor;
        [SerializeField] private Transform _cashPoint;
        [SerializeField] private Transform _cashBox;
        [SerializeField] private Vector3 _cashBoxCloseLocalPosition;
        [SerializeField] private Vector3 _cashBoxOpenLocalPosition;

        private bool _paymentEnded = true;
        private bool _canEnd;
        
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
            
            _slotsQueue.Enqueue((SlotAction.Take, slot));
        }

        public void Return(CashSlot slot)
        {
            if (_paymentEnded) 
                return;
            
            _slotsQueue.Enqueue((SlotAction.Return, slot));
        }

        public IEnumerator AcceptPayment(PaymentObject paymentCash, float givingCash, float productsPrice)
        {
            _canEnd = false;
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

                if (slot.Item1 == SlotAction.Take)
                {
                    var cash = slot.Item2.Take(_cashPoint.position);
                    CurrentChange += cash.Value;
                    _cash.Add(cash);
                }
                else if (slot.Item1 == SlotAction.Return)
                {
                    var cash = _cash.FirstOrDefault(cash => Math.Abs(cash.Value - slot.Item2.TargetCashValue) < float.Epsilon);

                    if (cash != null)
                    {
                        slot.Item2.Return(cash);
                        CurrentChange -= cash.Value;
                        _cash.Remove(cash);
                    }
                }

                float targetChange = givingCash - productsPrice;
                _canEnd = Mathf.Abs(targetChange - CurrentChange) / targetChange * 100 < ChangePercentDifferenceForCanEnd;
                
                _monitor.UpdateInfo(givingCash, productsPrice, CurrentChange);
            }
        }

        public void EndPayment()
        {
            if (_canEnd == false)
                return;
            
            _paymentEnded = true;

            foreach (var cash in _cash)
                Destroy(cash.gameObject);
            
            _cash.Clear();
            _slots.ForEach(slot => slot.gameObject.SetActive(false));
            _cashBox.DOLocalMove(_cashBoxCloseLocalPosition, 0.2f);
            
            _monitor.UpdateInfo();
        }
        
        private enum SlotAction
        {
            Take,
            Return
        }
    }
}
