using DG.Tweening;
using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    public class CashSlot : MonoBehaviour
    {
        [SerializeField] private Cash _targetCashTemplate;
        
        public Cash Take(Vector3 cashPosition)
        {
            var cashInstance = Instantiate(_targetCashTemplate, transform.position, Quaternion.identity);
            cashInstance.transform.DOMove(cashPosition, 0.5f);
            
            return cashInstance;
        }
    }
}