using System;
using Agava.YandexGames;

namespace YellowSquad.GamePlatformSdk
{
    internal class YandexGamesAdvertisement : BaseAdvertisement
    {
        protected override void OnShowInterstitial(Action onEnd)
        {
            InterstitialAd.Show(
                onCloseCallback: _ => onEnd.Invoke(), 
                onErrorCallback: _ => onEnd.Invoke(), 
                onOfflineCallback: onEnd.Invoke);
        }

        protected override void OnShowRewarded(Action<Result> onEnd)
        {
            VideoAd.Show(
                onRewardedCallback: () => onEnd.Invoke(Result.Success), 
                onCloseCallback: () => onEnd.Invoke(Result.Failure), 
                onErrorCallback: _ => onEnd.Invoke(Result.Failure));
        }
    }
}