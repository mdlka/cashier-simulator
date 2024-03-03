using System.Collections;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    public class CustomersQueue : MonoBehaviour
    {
        [SerializeField] private CashDesk _cashDesk;
        [SerializeField] private Transform _exitPoint;
        [SerializeField] private CustomerPoint[] _queuePoints;

        public bool HasPlace => _queuePoints[^1].IsBusy == false;
        public bool HasCustomers => _queuePoints[0].IsBusy;

        private void Start()
        {
            StartCoroutine(Working());
        }

        public void Add(Customer customer)
        {
            var point = _queuePoints.First(point => point.IsBusy == false);
            point.Take(customer);

            var customerScale = customer.transform.localScale;
            
            customer.transform.localScale = Vector3.zero;
            customer.transform.position = point.Position;

            customer.transform.DOScale(customerScale, 0.7f);
        }

        private IEnumerator Working()
        {
            while (true)
            {
                yield return new WaitUntil(() => _queuePoints[0].IsBusy);

                var currentCostumer = _queuePoints[0].Value;
                
                yield return _cashDesk.AcceptCustomer(currentCostumer);
                
                currentCostumer.MoveTo(_exitPoint.position, onComplete: () => Destroy(currentCostumer.gameObject));
                _queuePoints[0].Free();

                MoveQueue();
            }
        }

        private void MoveQueue()
        {
            for (int i = 1; i < _queuePoints.Length; i++)
            {
                if (_queuePoints[i].IsBusy == false)
                    break;

                var customer = _queuePoints[i].Value;

                _queuePoints[i].Free();
                _queuePoints[i - 1].Take(customer);

                customer.MoveTo(_queuePoints[i - 1].Position);
            }
        }
    }
}