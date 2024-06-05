using TMPro;
using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    internal class WalletView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        
        public void Render(Currency value)
        {
            _text.text = value.ToPriceTag();
        }
    }
}