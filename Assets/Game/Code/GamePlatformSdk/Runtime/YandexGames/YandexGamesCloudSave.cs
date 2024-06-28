using System.Collections;
using UnityEngine;
using PlayerPrefs = Agava.YandexGames.Utility.PlayerPrefs;

namespace YellowSquad.GamePlatformSdk
{
    internal class YandexGamesCloudSave : ISave
    {
        public IEnumerator Initialize()
        {
            bool saveLoadEnded = false;
            
            PlayerPrefs.Load(onSuccessCallback: () => saveLoadEnded = true, onErrorCallback: _ => saveLoadEnded = true);
            yield return new WaitUntil(() => saveLoadEnded);
        }

        public bool HasKey(string key)
        {
            return PlayerPrefs.HasKey(key);
        }

        public void Save(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
            PlayerPrefs.Save();
        }

        public string Load(string key)
        {
            return PlayerPrefs.GetString(key);
        }
    }
}