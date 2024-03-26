using DG.Tweening;
using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    public class CashSlot : MonoBehaviour
    {
        [SerializeField] private Cash _targetCashTemplate;

        public float TargetCashValue => _targetCashTemplate.Value;
        
        public Cash Take(Vector3 cashPosition, Vector3 rotation)
        {
            var cashInstance = Instantiate(_targetCashTemplate, transform.position, Quaternion.identity);
            cashInstance.transform.DOJump(cashPosition, 0.1f, 1, 0.5f);
            cashInstance.transform.DORotate(rotation, 0.5f);
            
            return cashInstance;
        }

        public void Return(Cash cash)
        {
            cash.transform.DORotate(Vector3.zero, 0.3f);
            cash.transform.DOJump(transform.position, 0.1f, 1, 0.3f).OnComplete(() => Destroy(cash.gameObject));
        }
    }
}