using System;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

namespace YellowSquad.WebFixes
{
    public static class WebCursor
    {
        public static CursorLockMode LockState
        {
            get
            {
#if UNITY_WEBGL && !UNITY_EDITOR
                return _pointerLocked ? CursorLockMode.Locked : CursorLockMode.None;
#else
                return Cursor.lockState;
#endif
            }
        }
        
        private static bool _pointerLocked;

        [DllImport("__Internal")]
        private static extern void WebCursorInitialize(Action<bool> onPointerLockChange);

#if UNITY_WEBGL && !UNITY_EDITOR
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
#endif
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Unity InitializeOnLoadMethod")]
        private static void Initialize()
        {
            WebCursorInitialize(OnPointerLockChanged);
        }
        
        [MonoPInvokeCallback(typeof(Action<bool>))]
        private static void OnPointerLockChanged(bool locked)
        {
            _pointerLocked = locked;
        }
    }
}
