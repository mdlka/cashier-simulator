using DG.Tweening;
using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    public class CashSlot : MonoBehaviour
    {
        [SerializeField] private Cash _targetCashTemplate;

        public long TargetCurrencyTotalCents => _targetCashTemplate.TotalCents;

        public Cash Take(CashStack cashStack)
        {
            var cashInstance = Instantiate(_targetCashTemplate, transform.position, Quaternion.identity);
            cashInstance.DisableOutline();
            cashStack.Add(cashInstance);
            
            return cashInstance;
        }

        public void Return(Cash cash)
        {
            cash.transform.DOKill();
            
            cash.transform.DORotate(Vector3.zero, 0.3f);
            cash.transform.DOJump(transform.position, 0.1f, 1, 0.3f).OnComplete(() => Destroy(cash.gameObject));
        }

        public void Enable()
        {
            gameObject.SetActive(true);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}