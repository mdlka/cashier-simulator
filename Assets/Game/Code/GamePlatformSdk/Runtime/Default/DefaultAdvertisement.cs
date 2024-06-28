using System;
using System.Collections;
using UnityEngine;

namespace YellowSquad.GamePlatformSdk
{
    internal class DefaultAdvertisement : BaseAdvertisement
    {
        protected override IEnumerator OnShowInterstitial()
        {
            Debug.Log("Show Interstitial");
            yield break;
        }

        protected override IEnumerator OnShowRewarded(Action<Result> onEnd)
        {
            Debug.Log("Show Rewarded");
            onEnd.Invoke(Result.Success);
            yield break;
        }
    }
}
