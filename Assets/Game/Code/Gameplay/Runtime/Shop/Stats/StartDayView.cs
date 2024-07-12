using System.Collections;
using TMPro;
using UnityEngine;
using YellowSquad.CashierSimulator.Gameplay.Useful;

namespace YellowSquad.CashierSimulator.Gameplay
{
    internal class StartDayView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private string _header;

        public IEnumerator Render(long day)
        {
            _text.text = $"{_header} {day}";
            _canvasGroup.Enable(0.2f);

            yield return new WaitForSeconds(1f);
            
            _canvasGroup.Disable(0.2f);
        }
    }
}