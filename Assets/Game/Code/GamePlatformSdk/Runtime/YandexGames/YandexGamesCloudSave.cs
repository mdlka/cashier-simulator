using System.Collections;
using UnityEngine;
using PlayerPrefs = Agava.YandexGames.Utility.PlayerPrefs;

namespace YellowSquad.GamePlatformSdk
{
    internal class YandexGamesCloudSave : ISave
    {
        public IEnumerator Load()
        {
            bool saveLoadEnded = false;
            
            PlayerPrefs.Load(onSuccessCallback: () => saveLoadEnded = true, onErrorCallback: _ => saveLoadEnded = true);
            yield return new WaitUntil(() => saveLoadEnded);
        }

        public bool HasKey(string key)
        {
            return PlayerPrefs.HasKey(key);
        }

        public void SetString(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
        }

        public string GetString(string key, string defaultValue = "")
        {
            return PlayerPrefs.GetString(key, defaultValue);
        }

        public void Save()
        {
            PlayerPrefs.Save();
        }
    }
}