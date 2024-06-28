using System.Collections;

namespace YellowSquad.GamePlatformSdk
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