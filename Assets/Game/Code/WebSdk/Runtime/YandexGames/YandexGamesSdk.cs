using System.Collections;

namespace YellowSquad.WebSdk
{
    public class YandexGamesSdk : IGamePlatformSdk
    {
        public IEnumerator Initialize()
        {
            yield return Agava.YandexGames.YandexGamesSdk.Initialize();
        }

        public bool Initialized => Agava.YandexGames.YandexGamesSdk.IsInitialized;
        public IAdvertisement Advertisement { get; } = new YandexGamesAdvertisement();
    }
}