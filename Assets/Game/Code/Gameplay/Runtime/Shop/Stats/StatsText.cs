using TMPro;
using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    internal class StatsText : MonoBehaviour
    {
        [SerializeField] private string _statsName;
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
            _text.text = $"{_statsName} {textParams}{stats}";
        }
    }
}