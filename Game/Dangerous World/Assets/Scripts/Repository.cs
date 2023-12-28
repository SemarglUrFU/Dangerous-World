using Newtonsoft.Json;
using UnityEngine;

public static class Repository
{   
    public static void SaveValue<T>(string key, T value) where T : class
    {
        var serializedData = JsonConvert.SerializeObject(value);
        PlayerPrefs.SetString(key, serializedData);
        PlayerPrefs.Save();
    }

    public static bool TryReadValue<T>(string key, out T value) where T : class
    {
        if (!PlayerPrefs.HasKey(key))
        {
            value = default;
            return false;
        }
        var serializedData = PlayerPrefs.GetString(key);
        value = JsonConvert.DeserializeObject<T>(serializedData);
        return true;
    }

    public static void SaveValue(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
        PlayerPrefs.Save();
    }

    public static int ReadInt(string key, int defaultValue = 0) => PlayerPrefs.GetInt(key, defaultValue);
    public static bool TryReadValue(string key, out int value)
    {
        if (!PlayerPrefs.HasKey(key))
        {
            value = default;
            return false;
        }
        value = PlayerPrefs.GetInt(key);
        return true;
    }

    public static void SaveValue(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
        PlayerPrefs.Save();
    }

    public static string ReadString(string key, string defaultValue = "") => PlayerPrefs.GetString(key, defaultValue);
    public static bool TryReadValue(string key, out string value)
    {
        if (!PlayerPrefs.HasKey(key))
        {
            value = default;
            return false;
        }
        value = PlayerPrefs.GetString(key);
        return true;
    }

    public static bool RemoveData(string key)
    {
        if (!PlayerPrefs.HasKey(key))
            return false;
        PlayerPrefs.DeleteKey(key);
        return true;
    }
}
