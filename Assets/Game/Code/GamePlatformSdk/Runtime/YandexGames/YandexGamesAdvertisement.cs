using System.Collections;
using Agava.YandexGames;
using UnityEngine;

namespace YellowSquad.GamePlatformSdk
{
    internal class YandexGamesAdvertisement : IAdvertisement
    {
        public Result LastRewardedResult { get; private set; }
        public double LastInterstitialTime { get; private set; }
        public double LastAdTime { get; private set; }
        
        public IEnumerator ShowInterstitial()
        {
            bool ended = false;
            
            InterstitialAd.Show(
                onCloseCallback: _ => ended = true, 
                onErrorCallback: _ => ended = true, 
                onOfflineCallback: () => ended = true);

            yield return new WaitUntil(() => ended == false);
            
            LastInterstitialTime = LastAdTime = Time.realtimeSinceStartupAsDouble;
        }

        public IEnumerator ShowRewarded()
        {
            bool ended = false;
            LastRewardedResult = Result.Failure;
            
            VideoAd.Show(
                onRewardedCallback: () => { ended = true; LastRewardedResult = Result.Success; }, 
                onCloseCallback: () => ended = true, 
                onErrorCallback: _ => ended = true);

            yield return new WaitUntil(() => ended == false);
            
            LastAdTime = Time.realtimeSinceStartupAsDouble;
        }
    }
}