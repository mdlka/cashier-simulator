using Agava.WebUtility;
using UnityEngine;
using YellowSquad.GamePlatformSdk;

namespace YellowSquad.CashierSimulator.Application
{
    public class ApplicationFocus : MonoBehaviour
    {
        private bool _adsShowing;
        
        private void OnEnable()
        {
            GamePlatformSdkContext.Current.Advertisement.AdsStarted += AdsShowed;
            GamePlatformSdkContext.Current.Advertisement.AdsEnded += AdsClose;
            WebApplication.InBackgroundChangeEvent += OnBackground;
        }
        
        private void OnDisable()
        {
            GamePlatformSdkContext.Current.Advertisement.AdsStarted -= AdsShowed;
            GamePlatformSdkContext.Current.Advertisement.AdsEnded -= AdsClose;
            WebApplication.InBackgroundChangeEvent -= OnBackground;
        }

        private void AdsShowed()
        {
            _adsShowing = true;
            DisableFocus();
        }

        private void AdsClose()
        {
            _adsShowing = false;
            EnableFocus();
        }

        private void OnBackground(bool isBackground)
        {
            if (_adsShowing)
                return;
            
            if (isBackground)
                DisableFocus();
            else
                EnableFocus();
        }

        private void DisableFocus()
        {
            Time.timeScale = 0f;
            AudioListener.pause = true;
        }

        private void EnableFocus()
        {
            Time.timeScale = 1f;
            AudioListener.pause = false;
        }
    }
}