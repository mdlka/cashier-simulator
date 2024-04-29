using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace YellowSquad.CashierSimulator.Gameplay
{
    internal class CustomerAnimator : MonoBehaviour
    {
        private const string MovingParam = "Moving";
        private const string PutUpHandParam = "PutUpHand";

        [SerializeField] private AnimatorOverrideController[] _maleControllers;
        [SerializeField] private AnimatorOverrideController[] _femaleControllers;

        private Animator _animator;

        public void Initialize(CustomerModel model)
        {
            var targetControllers = model.Gender == Gender.Male ? _maleControllers : _femaleControllers;
            
            _animator = model.Animator;
            _animator.runtimeAnimatorController = targetControllers[Random.Range(0, targetControllers.Length)];
        }

        public void EnableMove()
        {
            _animator.SetBool(MovingParam, true);
        }

        public void DisableMove()
        {
            _animator.SetBool(MovingParam, false);
        }

        public void PutDownHand()
        {
            AnimateLayerWeight(1, 0f, 0.5f);
            _animator.SetBool(PutUpHandParam, false);
        }

        public void PutUpHand()
        {
            AnimateLayerWeight(1, 1f, 0.5f);
            _animator.SetBool(PutUpHandParam, true);
        }

        private void AnimateLayerWeight(int index, float endValue, float duration)
        {
            DOTween.To(() => _animator.GetLayerWeight(index), weight => _animator.SetLayerWeight(index, weight), endValue, duration);
        }
    }
}