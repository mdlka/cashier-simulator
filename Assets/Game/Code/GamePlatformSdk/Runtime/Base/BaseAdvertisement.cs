using System;
using System.Collections;
using UnityEngine;

namespace YellowSquad.GamePlatformSdk
{
    internal abstract class BaseAdvertisement : IAdvertisement
    {
        public Result LastRewardedResult { get; private set; }
        public double LastAdTime { get; private set; } = -SdkSettings.IntervalBetweenAdsInSeconds;
        
        public void ShowInterstitial(Action onEnd)
        {
            if (CanShowAds() == false)
                return;
            
            OnShowInterstitial(onEnd);
        }

        public void ShowRewarded(Action<Result> onEnd)
        {
            OnShowRewarded(onEnd);
        }

        public IEnumerator ShowInterstitial()
        {
            if (CanShowAds() == false)
                yield break;

            bool ended = false;
            
            OnShowInterstitial(onEnd: () => ended = true);
            yield return new WaitUntil(() => ended);
            
            LastAdTime = Time.realtimeSinceStartupAsDouble;
        }

        public IEnumerator ShowRewarded()
        {
            if (CanShowAds() == false)
                yield break;
            
            bool ended = false;
            LastRewardedResult = Result.Failure;
            
            OnShowRewarded(onEnd: result => { ended = true; LastRewardedResult = result; });
            yield return new WaitUntil(() => ended);
            
            LastAdTime = Time.realtimeSinceStartupAsDouble;
        }

        private bool CanShowAds()
        {
            return Time.realtimeSinceStartupAsDouble - LastAdTime >= SdkSettings.IntervalBetweenAdsInSeconds;
        }

        protected abstract void OnShowInterstitial(Action onEnd);
        protected abstract void OnShowRewarded(Action<Result> onEnd);
    }
}
