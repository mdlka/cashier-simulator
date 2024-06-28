﻿using System.Collections;

namespace YellowSquad.WebSdk
{
    public interface IAdvertisement
    {
        Result LastRewardedResult { get; }
        double LastInterstitialTime { get; }
        double LastAdTime { get; }

        IEnumerator ShowInterstitial();
        IEnumerator ShowRewarded();
    }

    public enum Result
    {
        Failure = 1,
        Success = 2
    }
}