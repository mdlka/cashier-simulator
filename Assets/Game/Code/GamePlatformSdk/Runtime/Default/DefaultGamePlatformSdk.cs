using System.Collections;
using UnityEngine;

namespace YellowSquad.GamePlatformSdk
{
    internal class DefaultGamePlatformSdk : IGamePlatformSdk
    {
        public IEnumerator Initialize()
        {
            Debug.Log("Sdk initialized");
            yield break;
        }

        public bool Initialized => true;
        public IAdvertisement Advertisement { get; } = new DefaultAdvertisement();
    }
}