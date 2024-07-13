using System.Collections;

namespace YellowSquad.GamePlatformSdk
{
    internal class YandexGamesSdk : IGamePlatformSdk
    {
        public IEnumerator Initialize()
        {
            yield return Agava.YandexGames.YandexGamesSdk.Initialize();
            yield return Save.Load();

            Initialized = true;
        }

        public bool Initialized { get; private set; }

        public IAdvertisement Advertisement { get; } = new YandexGamesAdvertisement();
        public ISave Save { get; } = new YandexGamesCloudSave();
    }
}