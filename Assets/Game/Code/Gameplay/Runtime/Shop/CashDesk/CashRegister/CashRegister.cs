using System.Collections;
using System.Collections.Generic;
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
        [SerializeField] private CashStack _dollarsStack;
        [SerializeField] private CashStack _centsStack;
        [SerializeField] private Transform _cashBox;
        [SerializeField] private Vector3 _cashBoxCloseLocalPosition;
        [SerializeField] private Vector3 _cashBoxOpenLocalPosition;
        [Header("Audio")] 
        [SerializeField] private AudioSource _audioSource;

        private bool _paymentEnded = true;
        private bool _canEnd;
        
        public Currency CurrentChange { get; private set; }

        private void Awake()
        {
            _cashBox.localPosition = _cashBoxCloseLocalPosition;
            _slots.ForEach(slot => slot.Disable());
            
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

        public IEnumerator AcceptPayment(PaymentObject paymentCash, Currency givingCash, Currency productsPrice)
        {
            CurrentChange = Currency.Zero;
            
            _paymentEnded = false;
            _canEnd = CalculateCanEnd(givingCash - productsPrice);

            paymentCash.Destroy();
            
            _monitor.UpdateInfo(givingCash, productsPrice, CurrentChange);
            
            _slots.ForEach(slot => slot.Enable());
            _cashBox.DOLocalMove(_cashBoxOpenLocalPosition, 0.2f);
            _audioSource.Play();

            while (_paymentEnded == false)
            {
                yield return new WaitUntil(() => _slotsQueue.Count > 0 || _paymentEnded);

                if (_paymentEnded)
                    yield break;

                var slot = _slotsQueue.Dequeue();

                if (slot.Item1 == SlotAction.Take)
                {
                    var targetStack = TargetStack(slot.Item2.TargetCurrencyTotalCents);
                    var cash = slot.Item2.Take(targetStack);
                    CurrentChange += cash.TotalCents;
                    _cash.Add(cash);
                }
                else if (slot.Item1 == SlotAction.Return)
                {
                    int lastIndex = _cash.FindLastIndex(cash => cash.TotalCents == slot.Item2.TargetCurrencyTotalCents);
                    var cash = lastIndex == -1 ? null : _cash[lastIndex];

                    if (cash != null)
                    {
                        TargetStack(slot.Item2.TargetCurrencyTotalCents).Remove(cash);
                        slot.Item2.Return(cash);
                        CurrentChange -= cash.TotalCents;
                        _cash.Remove(cash);
                    }
                }

                _canEnd = CalculateCanEnd(givingCash - productsPrice);
                _monitor.UpdateInfo(givingCash, productsPrice, CurrentChange);
            }
        }

        public void EndPayment()
        {
            if (_canEnd == false)
                return;
            
            _paymentEnded = true;

            foreach (var cash in _cash)
            {
                cash.transform.DOComplete();
                Destroy(cash.gameObject);
            }
            
            _cash.Clear();
            _slots.ForEach(slot => slot.Disable());
            _cashBox.DOLocalMove(_cashBoxCloseLocalPosition, 0.2f);
            
            _monitor.UpdateInfo();
        }

        private bool CalculateCanEnd(Currency targetChange)
        {
            if (targetChange.TotalCents == 0)
                return CurrentChange.TotalCents == 0;

            if (targetChange.TotalCents - CurrentChange.TotalCents < 0)
                return false;
            
            return Mathf.Abs(targetChange.TotalCents - CurrentChange.TotalCents) / targetChange.TotalCents * 100 < ChangePercentDifferenceForCanEnd;
        }

        private CashStack TargetStack(long cents)
        {
            return cents >= 100 ? _dollarsStack : _centsStack;
        }
        
        private enum SlotAction
        {
            Take,
            Return
        }
    }
}
