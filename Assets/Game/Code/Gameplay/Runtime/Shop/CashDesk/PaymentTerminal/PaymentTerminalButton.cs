using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    public class PaymentTerminalButton : InputButton
    {
        [field: SerializeField] public PaymentTerminalButtonType Type { get; private set; }
    }
}