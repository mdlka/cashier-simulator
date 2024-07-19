using Lean.Localization;
using TMPro;
using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    internal class StatsText : MonoBehaviour
    {
        [SerializeField, LeanTranslationName] private string _statsHeaderTranslationName;
        [SerializeField] private Color _statsColor;
        [SerializeField] private TMP_Text _text;

        private string _htmlStatsColor;
        
        private void Awake()
        {
            _htmlStatsColor = ColorUtility.ToHtmlStringRGB(_statsColor);
        }

        public void Render(string stats, bool needChangeColor = true)
        {
            string textParams = needChangeColor ? $"<b><color=#{_htmlStatsColor}>" : "<b>";
            _text.text = $"{LeanLocalization.GetTranslationText(_statsHeaderTranslationName)} {textParams}{stats}";
        }
    }
}