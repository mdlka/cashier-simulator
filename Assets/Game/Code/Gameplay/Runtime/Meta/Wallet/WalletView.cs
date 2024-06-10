using TMPro;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

namespace YellowSquad.CashierSimulator.Gameplay
{
    internal class WalletView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private TMP_Text _diffText;
        [SerializeField] private Color _positiveDiffColor;
        [SerializeField] private Color _negativeDiffColor;
        [SerializeField, Min(0)] private float _animationDuration;

        private TweenerCore<Vector2, Vector2, VectorOptions> _moveTweener;

        private void Awake()
        {
            _diffText.enabled = false;
        }

        public void Render(Currency value, Currency diff, Sign diffSign = Sign.Plus)
        {
            _text.text = value.ToPriceTag();

            if (diff.TotalCents == 0)
                return;

            string sign = diffSign == Sign.Minus ? "-" : "+";
            _diffText.color = diffSign == Sign.Minus ? _negativeDiffColor : _positiveDiffColor;
            _diffText.text = $"{sign}{diff.ToPriceTag()}";
            
            PlayAnimation();
        }

        private void PlayAnimation()
        {
            _diffText.DOComplete();
            _moveTweener?.Kill(complete: true);
            
            _diffText.rectTransform.anchoredPosition = 
                new Vector2(_diffText.rectTransform.anchoredPosition.x, -_diffText.rectTransform.rect.size.y);

            _diffText.alpha = 1;
            _diffText.enabled = true;
            
            _diffText.DOFade(0, _animationDuration);
            _moveTweener = DOTween.To(() => _diffText.rectTransform.anchoredPosition,
                vector2 => _diffText.rectTransform.anchoredPosition = vector2,
                new Vector2(_diffText.rectTransform.anchoredPosition.x, 0), _animationDuration);
        }
    }
}