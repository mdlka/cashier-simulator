using System.Collections;

namespace YellowSquad.GamePlatformSdk
{
    public interface ISave
    {
        IEnumerator Initialize();

        bool HasKey(string key);
        
        void Save(string key, string value);
        string Load(string key);
    }
}