using System;
using System.Collections;

namespace YellowSquad.GamePlatformSdk
{
    internal class YandexGamesSdk : IGamePlatformSdk
    {
        private readonly IAdvertisement _advertisement = new YandexGamesAdvertisement();
        private readonly ISave _save = new YandexGamesCloudSave();

        public IEnumerator Initialize()
        {
            yield return Agava.YandexGames.YandexGamesSdk.Initialize();
            yield return _save.Load();

            Initialized = true;
        }

        public bool Initialized { get; private set; }

        public IAdvertisement Advertisement
        {
            get
            {
                ThrowIfNotInitialized();
                return _advertisement;
            }
        }

        public ISave Save
        {
            get
            {
                ThrowIfNotInitialized();
                return _save;
            }
        }

        private void ThrowIfNotInitialized()
        {
            if (Initialized == false)
                throw new InvalidOperationException("Sdk not initialized.\nMake sure you call Initialize() and wait for Initialized == true");
        }
    }
}