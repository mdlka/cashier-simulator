using System;
using System.Collections;
using UnityEngine;

namespace YellowSquad.GamePlatformSdk
{
    internal abstract class BaseAdvertisement : IAdvertisement
    {
        public Result LastRewardedResult { get; private set; }
        public double LastAdTime { get; private set; }

        public IEnumerator ShowInterstitial()
        {
            if (CanShowAds() == false)
                yield break;
            
            yield return OnShowInterstitial();
            LastAdTime = Time.realtimeSinceStartupAsDouble;
        }

        public IEnumerator ShowRewarded()
        {
            if (CanShowAds() == false)
                yield break;
            
            yield return OnShowRewarded(onEnd: result => LastRewardedResult = result);
            LastAdTime = Time.realtimeSinceStartupAsDouble;
        }

        private bool CanShowAds()
        {
            return Time.realtimeSinceStartupAsDouble - LastAdTime >= SdkSettings.IntervalBetweenAdsInSeconds;
        }

        protected abstract IEnumerator OnShowInterstitial();
        protected abstract IEnumerator OnShowRewarded(Action<Result> onEnd);
    }
}
