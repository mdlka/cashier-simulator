using System;
using System.Collections;

namespace YellowSquad.GamePlatformSdk
{
    internal class YandexGamesSdk : IGamePlatformSdk
    {
        private readonly IAdvertisement _advertisement = new YandexGamesAdvertisement();
        
        public IEnumerator Initialize()
        {
            yield return Agava.YandexGames.YandexGamesSdk.Initialize();
        }

        public bool Initialized => Agava.YandexGames.YandexGamesSdk.IsInitialized;

        public IAdvertisement Advertisement
        {
            get
            {
                ThrowIfNotInitialized();
                return _advertisement;
            }
        }

        private void ThrowIfNotInitialized()
        {
            if (Initialized == false)
                throw new InvalidOperationException("Sdk not initialized.\nMake sure you call Initialize() and wait for Initialized == true");
        }
    }
}