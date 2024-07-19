using System.Collections;
using Lean.Localization;
using TMPro;
using UnityEngine;
using YellowSquad.CashierSimulator.Gameplay.Useful;

namespace YellowSquad.CashierSimulator.Gameplay
{
    internal class StartDayView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField, LeanTranslationName] private string _headerTranslationName;

        public IEnumerator Render(long day)
        {
            _text.text = $"{LeanLocalization.GetTranslationText(_headerTranslationName)} {day}";
            _canvasGroup.Enable(0.2f);

            yield return new WaitForSeconds(1f);
            
            _canvasGroup.Disable(0.2f);
        }
    }
}