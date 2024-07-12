using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YellowSquad.CashierSimulator.Gameplay.Useful;

namespace YellowSquad.CashierSimulator.Gameplay
{
    public class GameTutorial : MonoBehaviour
    {
        [SerializeField] private Image _screenImage;
        [SerializeField] private TMP_Text _descriptionText;
        [SerializeField] private Button _nextButton;
        [SerializeField] private Button _endTutorialButton;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Pair[] _screens;

        private bool _played;
        private bool _needNext;
        private bool _ended;

        private void Awake()
        {
            _canvasGroup.Disable();
        }

        public IEnumerator Play()
        {
            if (_played)
                yield break;
            
            _played = true;
            
            _canvasGroup.Enable(0.5f);
            
            _endTutorialButton.gameObject.SetActive(false);
            _nextButton.gameObject.SetActive(true);
            _nextButton.onClick.AddListener(OnNextButtonClick);

            for (int i = 0; i < _screens.Length - 1; i++)
            {
                var pair = _screens[i];
                _screenImage.sprite = pair.Screen;
                _descriptionText.text = pair.RuDescription;

                yield return new WaitUntil(() => _needNext);
                _needNext = false;
            }
            
            _screenImage.sprite = _screens[^1].Screen;
            _descriptionText.text = _screens[^1].RuDescription;

            _nextButton.gameObject.SetActive(false);
            _endTutorialButton.gameObject.SetActive(true);
            
            _nextButton.onClick.RemoveListener(OnNextButtonClick);
            _endTutorialButton.onClick.AddListener(OnEndTutorialButtonClick);

            yield return new WaitUntil(() => _ended);
            
            _endTutorialButton.onClick.RemoveListener(OnEndTutorialButtonClick);
            
            _canvasGroup.Disable(0.2f);
        }

        private void OnNextButtonClick()
        {
            _needNext = true;
        }

        private void OnEndTutorialButtonClick()
        {
            _ended = true;
        }
        
        [Serializable]
        private class Pair
        {
            [field: SerializeField] public Sprite Screen { get; private set; }
            [field: SerializeField, TextArea] public string RuDescription { get; private set; }
        }
    }
}