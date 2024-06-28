using System;
using System.Collections;
using UnityEngine;

namespace YellowSquad.GamePlatformSdk
{
    internal class DefaultSave : ISave
    {
        public IEnumerator Initialize()
        {
            yield break;
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
            if (HasKey(key) == false)
                throw new InvalidOperationException("Key doesn't exist");

            return PlayerPrefs.GetString(key);
        }
    }
}