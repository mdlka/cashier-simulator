using System.Collections;

namespace YellowSquad.WebSdk
{
    public class DefaultGamePlatformSdk : IGamePlatformSdk
    {
        public IEnumerator Initialize()
        {
            yield break;
        }

        public bool Initialized => true;
        public IAdvertisement Advertisement { get; } = new DefaultAdvertisement();
    }
}