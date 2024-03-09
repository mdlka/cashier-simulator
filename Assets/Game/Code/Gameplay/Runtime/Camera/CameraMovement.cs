using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace YellowSquad.CashierSimulator.Gameplay
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField, Min(0f)] private float _animationDuration;
        [SerializeField] private Transform _base;
        [SerializeField] private Image _crosshair;

        public void ReturnToBase()
        {
            if (transform.parent == _base)
                return;

            MoveTo(_base, true);
        }

        public void MoveTo(Transform point, bool enableCrosshair = false)
        {
            transform.DOComplete(true);

            _crosshair.enabled = enableCrosshair;
            
            transform.parent = point;
            transform.DOLocalMove(Vector3.zero, _animationDuration).SetEase(Ease.Linear);
            transform.DOLocalRotate(Vector3.zero, _animationDuration).SetEase(Ease.Linear);
        }
    }
}