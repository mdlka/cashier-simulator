using System;
using System.Collections;
using Agava.YandexGames;
using UnityEngine;

namespace YellowSquad.GamePlatformSdk
{
    internal class YandexGamesAdvertisement : BaseAdvertisement
    {
        protected override IEnumerator OnShowInterstitial()
        {
            bool ended = false;
            
            InterstitialAd.Show(
                onCloseCallback: _ => ended = true, 
                onErrorCallback: _ => ended = true, 
                onOfflineCallback: () => ended = true);

            yield return new WaitUntil(() => ended == false);
        }

        protected override IEnumerator OnShowRewarded(Action<Result> onEnd)
        {
            bool ended = false;
            var rewardedResult = Result.Failure;
            
            VideoAd.Show(
                onRewardedCallback: () => { ended = true; rewardedResult = Result.Success; }, 
                onCloseCallback: () => ended = true, 
                onErrorCallback: _ => ended = true);

            yield return new WaitUntil(() => ended == false);
            onEnd.Invoke(rewardedResult);
        }
    }
}