using System.Collections;
using UnityEngine;

namespace YellowSquad.WebSdk
{
    public class DefaultAdvertisement : IAdvertisement
    {
        public Result LastRewardedResult => Result.Success;
        public double LastInterstitialTime { get; private set; }
        public double LastAdTime { get; private set; }

        public IEnumerator ShowInterstitial()
        {
            LastInterstitialTime = LastAdTime = Time.realtimeSinceStartupAsDouble;
            yield break;
        }

        public IEnumerator ShowRewarded()
        {
            LastAdTime = Time.realtimeSinceStartupAsDouble;
            yield break;
        }
    }
}
