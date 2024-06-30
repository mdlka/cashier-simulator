﻿using System;
using Agava.YandexGames;
using UnityEngine;

namespace YellowSquad.GamePlatformSdk
{
    internal class YandexGamesAdvertisement : BaseAdvertisement
    {
        protected override void OnShowInterstitial(Action onEnd)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            onEnd += () =>
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            };
            
            InterstitialAd.Show(
                onCloseCallback: _ => onEnd.Invoke(), 
                onErrorCallback: _ => onEnd.Invoke(), 
                onOfflineCallback: onEnd.Invoke);
        }

        protected override void OnShowRewarded(Action<Result> onEnd)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            onEnd += _ =>
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            };
            
            VideoAd.Show(
                onRewardedCallback: () => onEnd.Invoke(Result.Success), 
                onCloseCallback: () => onEnd.Invoke(Result.Failure), 
                onErrorCallback: _ => onEnd.Invoke(Result.Failure));
        }
    }
}