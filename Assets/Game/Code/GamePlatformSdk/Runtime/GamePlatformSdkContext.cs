namespace YellowSquad.GamePlatformSdk
{
    public static class GamePlatformSdkContext
    {
        static GamePlatformSdkContext()
        {
#if YANDEX
            Current = new YandexGamesSdk();
#else
            Current = new DefaultGamePlatformSdk();
#endif
        }
        
        public static IGamePlatformSdk Current { get; }
    }
}