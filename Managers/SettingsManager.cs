using System.Collections.Generic;
using UnityEngine;

public static class SettingsManager
{
    public const string KEY_MUSIC_VOLUME = "MUSIC_VOLUME";
    public const string KEY_SFX_VOLUME = "SFX_VOLUME";
    public const string KEY_SCREEN_FLASH_TOGGLE = "SCREEN_FLASH_TOGGLE";
    public const string KEY_SCREEN_SHAKE_TOGGLE = "SCREEN_SHAKE_TOGGLE";
    public const string KEY_VIBRATION_TOGGLE = "VIBRATION_TOGGLE";

    public static event System.Action<string, object> OnSettingChanged = (key, value) => { };

    private static Dictionary<string, bool> defaultBools = new Dictionary<string, bool>
    {
        { KEY_SCREEN_FLASH_TOGGLE, true },
        { KEY_SCREEN_SHAKE_TOGGLE, true },
        { KEY_VIBRATION_TOGGLE, true },
    };

    private static Dictionary<string, float> defaultFloats = new Dictionary<string, float>
    {
        { KEY_MUSIC_VOLUME, .3f },
        { KEY_SFX_VOLUME, 1.0f },
    };

    public static void LoadDefaults()
    {
        foreach (KeyValuePair<string, bool> entry in defaultBools)
        {
            OnSettingChanged(entry.Key, GetBool(entry.Key));
        }

        foreach (KeyValuePair<string, float> entry in defaultFloats)
        {
            OnSettingChanged(entry.Key, GetBool(entry.Key));
        }
    }

    public static bool GetBool(string key)
    {
        return PlayerPrefs.GetInt(key, defaultBools.GetValueOrDefault(key, true) ? 1 : 0) > 0;
    }

    public static void SetBool(string key, bool value)
    {
        if (GetBool(key) == value) return;

        PlayerPrefs.SetInt(key, value ? 1 : 0);
        OnSettingChanged(key, value);
    }

    public static float GetFloat(string key)
    {
        return PlayerPrefs.GetFloat(key, defaultFloats.GetValueOrDefault(key, 1));
    }

    public static void SetFloat(string key, float value)
    {
        if (GetFloat(key) == value) return;

        PlayerPrefs.SetFloat(key, value);
        OnSettingChanged(key, value);
    }
}
