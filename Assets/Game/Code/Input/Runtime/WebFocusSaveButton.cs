using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace YellowSquad.CashierSimulator.UserInput
{
    internal class WebFocusSaveButton : Button
    {
        private InputRouter _inputRouter;
        
        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);

#if UNITY_WEBGL && !UNITY_EDITOR
            _inputRouter ??= FindObjectOfType<InputRouter>();
            _inputRouter.SetActiveCursor(false);
#endif
        }
    }
}