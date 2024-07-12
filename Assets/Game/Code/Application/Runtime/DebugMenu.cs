using UnityEngine;
using YellowSquad.GamePlatformSdk;

namespace YellowSquad.CashierSimulator.Application
{
    public class DebugMenu : MonoBehaviour
    {
        private void Update()
        {
            if (gameObject.activeSelf == false)
                return;

            if (enabled == false)
                return;
            
            if (Input.GetKeyDown(KeyCode.Alpha9))
                DeleteSave();
        }

        private void DeleteSave()
        {
            GamePlatformSdkContext.Current.Save.DeleteAll();
            GamePlatformSdkContext.Current.Save.Save();
            
            Debug.Log("Saves deleted");
        }
    }
}