using System.Collections;
using UnityEngine;

namespace YellowSquad.GamePlatformSdk
{
    internal class DefaultSave : ISave
    {
        public IEnumerator Load()
        {
            yield break;
        }

        public void DeleteAll()
        {
            PlayerPrefs.DeleteAll();
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

        public long GetLeaderboardScore(string leaderboardName)
        {
            return long.Parse(PlayerPrefs.GetString(leaderboardName, "0"));
        }

        public void SetLeaderboardScore(string leaderboardName, long value)
        {
            PlayerPrefs.SetString(leaderboardName, value.ToString());
        }

        public void Save()
        {
            PlayerPrefs.Save();
        }
    }
}