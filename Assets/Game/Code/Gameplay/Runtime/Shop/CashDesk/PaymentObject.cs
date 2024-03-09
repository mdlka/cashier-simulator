using DG.Tweening;
using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    public class PaymentObject : MonoBehaviour
    {
        [field: SerializeField] public PaymentMethod PaymentMethod { get; private set; }

        public void Enable()
        {
            gameObject.SetActive(true);
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

        public void MoveTo(Transform point, float animationDuration)
        {
            transform.DOComplete();
            
            transform.parent = point;

            transform.DOLocalMove(Vector3.zero, animationDuration);
            transform.DOLocalRotate(Vector3.zero, animationDuration);
        }
    }
}