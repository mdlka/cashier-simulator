using System.Collections;

namespace YellowSquad.WebSdk
{
    public interface IGamePlatformSdk
    {
        IEnumerator Initialize();

        bool Initialized { get; }
        IAdvertisement Advertisement { get; }
    }
}