using System.Collections;
using System.Linq;
using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    public class CustomersQueue : MonoBehaviour
    {
        [SerializeField] private CashDesk _cashDesk;
        [SerializeField] private Transform[] _enterPoints;
        [SerializeField] private Transform[] _exitPoints;
        [SerializeField] private CustomerPoint[] _queuePoints;

        private Vector3[] _enterPositions;
        private Vector3[] _exitPositions;

        public bool HasPlace => _queuePoints[^1].IsBusy == false;
        public bool HasCustomers => _queuePoints[0].IsBusy;

        private void Start()
        {
            _enterPositions = new Vector3[_enterPoints.Length];
            for (int i = 1; i < _enterPoints.Length; i++)
                _enterPositions[i-1] = _enterPoints[i].position;
            
            _exitPositions = _exitPoints.Select(point => point.position).ToArray();
            
            StartCoroutine(Working());
        }

        public void Add(Customer customer)
        {
            var point = _queuePoints.First(point => point.IsBusy == false);
            point.Take(customer);

            _enterPositions[^1] = point.Position;
            
            customer.transform.position = _enterPoints[0].position;
            customer.MoveThrough(positions: _enterPositions.ToArray());
        }

        private IEnumerator Working()
        {
            while (true)
            {
                yield return new WaitUntil(() => _queuePoints[0].IsBusy);

                var currentCostumer = _queuePoints[0].Value;
                
                yield return new WaitUntil(() => currentCostumer.IsMoving == false);
                
                currentCostumer.RotateY(-60f);
                
                yield return new WaitForSeconds(1f);
                
                yield return _cashDesk.AcceptCustomer(currentCostumer);
                
                currentCostumer.MoveThrough(positions: _exitPositions, 
                    onComplete: () => Destroy(currentCostumer.gameObject));
                
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
                
                if (i == 1)
                    customer.MoveThrough(positions: _queuePoints[i - 1].Position);
                else
                    customer.MoveThrough(positions: _queuePoints[i - 1].Position, onComplete: () => customer.RotateY(0f));
            }
        }
    }
}