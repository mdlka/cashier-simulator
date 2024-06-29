using System.Collections;

namespace YellowSquad.GamePlatformSdk
{
    public interface ISave
    {
        IEnumerator Load();
        void Save();

        bool HasKey(string key);

        void SetString(string key, string value);
        string GetString(string key, string defaultValue = "");
    }
}