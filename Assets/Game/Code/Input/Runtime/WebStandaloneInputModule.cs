using UnityEngine;
using UnityEngine.EventSystems;

namespace YellowSquad.CashierSimulator.UserInput
{
    // It's a class to override mouse position on web when Cursor.lockState = locked.
    // Because on web we can't quickly change cursor lock state due to browsers security (we need to wait for user click to change).
    // Don't pay attention to the code. It's a copy (with comments) from PointerInputModule.cs.
    internal class WebStandaloneInputModule : StandaloneInputModule
    {
        private readonly MouseState m_MouseState = new();
        
        [SerializeField] private GameCursor _cursor;

        protected override MouseState GetMousePointerEventData(int id)
        {
            // Populate the left button...
            var created = GetPointerData(kMouseLeftId, out var leftData, true);

            leftData.Reset();

            if (created)
                leftData.position = _cursor.Enabled ? _cursor.Position : input.mousePosition;
            
            Vector2 pos = _cursor.Enabled ? _cursor.Position : input.mousePosition;
            leftData.delta = pos - leftData.position;
            leftData.position = pos;
            leftData.scrollDelta = input.mouseScrollDelta;
            leftData.button = PointerEventData.InputButton.Left;
            eventSystem.RaycastAll(leftData, m_RaycastResultCache);
            var raycast = FindFirstRaycast(m_RaycastResultCache);
            leftData.pointerCurrentRaycast = raycast;
            m_RaycastResultCache.Clear();

            // copy the apropriate data into right and middle slots
            GetPointerData(kMouseRightId, out var rightData, true);
            rightData.Reset();

            CopyFromTo(leftData, rightData);
            rightData.button = PointerEventData.InputButton.Right;

            GetPointerData(kMouseMiddleId, out var middleData, true);
            middleData.Reset();

            CopyFromTo(leftData, middleData);
            middleData.button = PointerEventData.InputButton.Middle;

            m_MouseState.SetButtonState(PointerEventData.InputButton.Left, StateForMouseButton(0), leftData);
            m_MouseState.SetButtonState(PointerEventData.InputButton.Right, StateForMouseButton(1), rightData);
            m_MouseState.SetButtonState(PointerEventData.InputButton.Middle, StateForMouseButton(2), middleData);

            return m_MouseState;
        }

        protected override void ProcessMove(PointerEventData pointerEvent)
        {
            var targetGO = pointerEvent.pointerCurrentRaycast.gameObject;
            HandlePointerExitAndEnter(pointerEvent, targetGO);
        }

        protected override void ProcessDrag(PointerEventData pointerEvent)
        {
            if (!pointerEvent.IsPointerMoving() ||
                pointerEvent.pointerDrag == null)
                return;

            if (!pointerEvent.dragging && 
                ShouldStartDrag(pointerEvent.pressPosition, pointerEvent.position, eventSystem.pixelDragThreshold, pointerEvent.useDragThreshold))
            {
                ExecuteEvents.Execute(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.beginDragHandler);
                pointerEvent.dragging = true;
            }

            // Drag notification
            if (pointerEvent.dragging)
            {
                // Before doing drag we should cancel any pointer down state
                // And clear selection!
                if (pointerEvent.pointerPress != pointerEvent.pointerDrag)
                {
                    ExecuteEvents.Execute(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerUpHandler);

                    pointerEvent.eligibleForClick = false;
                    pointerEvent.pointerPress = null;
                    pointerEvent.rawPointerPress = null;
                }
                ExecuteEvents.Execute(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.dragHandler);
            }
        }

        private static bool ShouldStartDrag(Vector2 pressPos, Vector2 currentPos, float threshold, bool useDragThreshold)
        {
            if (!useDragThreshold)
                return true;

            return (pressPos - currentPos).sqrMagnitude >= threshold * threshold;
        }
    }
}