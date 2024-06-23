using System.Runtime.InteropServices;

namespace YellowSquad.WebFixes
{
    public static class WebMouse
    {
        public static void LockCursor()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            RequestPointerLock();
#endif
        }
        
        [DllImport("__Internal")]
        private static extern void RequestPointerLock();
    }
}
