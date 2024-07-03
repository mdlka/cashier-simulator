using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace YellowSquad.CashierSimulator.UserInput
{
    internal class WebFocusSaveButton : Button
    {
        private InputRouter _inputRouter;
        private bool _pointerIn;
        
        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);

#if UNITY_WEBGL && !UNITY_EDITOR
            _inputRouter ??= FindObjectOfType<InputRouter>();
            _inputRouter.SetActiveCursor(false);
#endif
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);

#if UNITY_WEBGL && !UNITY_EDITOR
            if (_pointerIn)
                return;

            _inputRouter ??= FindObjectOfType<InputRouter>();
            _inputRouter.SetActiveCursor(true);
#endif
        }
        
        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            _pointerIn = true;
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            _pointerIn = false;
        }
    }
}