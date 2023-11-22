using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsScreen : UIScreen
{
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Toggle screenFlashToggle;
    [SerializeField] private Toggle screenShakeToggle;
    [SerializeField] private Toggle vibrationToggle;

    private void OnEnable()
    {
        SettingsManager.OnSettingChanged += HandleSettingsChanged;
        HighscoreManager.OnSignedIn += HandleSignedIn;
    }

    private void OnDisable()
    {
        SettingsManager.OnSettingChanged -= HandleSettingsChanged;
        HighscoreManager.OnSignedIn -= HandleSignedIn;
    }

    protected override void OnActivate()
    {
        nameInput.text = HighscoreManager.PlayerName;
        musicSlider.value = SettingsManager.GetFloat(SettingsManager.KEY_MUSIC_VOLUME);
        sfxSlider.value = SettingsManager.GetFloat(SettingsManager.KEY_SFX_VOLUME);
        screenFlashToggle.isOn = SettingsManager.GetBool(SettingsManager.KEY_SCREEN_FLASH_TOGGLE);
        screenShakeToggle.isOn = SettingsManager.GetBool(SettingsManager.KEY_SCREEN_SHAKE_TOGGLE);
        vibrationToggle.isOn = SettingsManager.GetBool(SettingsManager.KEY_VIBRATION_TOGGLE);
    }

    private void HandleSettingsChanged(string key, object value)
    {
        if (key == SettingsManager.KEY_MUSIC_VOLUME && value is float musicVolume)
        {
            musicSlider.value = musicVolume;
        }
        else if (key == SettingsManager.KEY_SFX_VOLUME && value is float sfxVolume)
        {
            sfxSlider.value = sfxVolume;
        }
        else if (key == SettingsManager.KEY_SCREEN_FLASH_TOGGLE && value is bool screenFlashEnabled)
        {
            screenFlashToggle.isOn = screenFlashEnabled;
        }
        else if (key == SettingsManager.KEY_SCREEN_SHAKE_TOGGLE && value is bool screenShakeEnabled)
        {
            screenShakeToggle.isOn = screenShakeEnabled;
        }
        else if (key == SettingsManager.KEY_VIBRATION_TOGGLE && value is bool vibrationEnabled)
        {
            vibrationToggle.isOn = vibrationEnabled;
        }
    }

    private void HandleSignedIn(string name)
    {
        nameInput.text = name;
    }

    public async void SaveName()
    {
        string desiredName = nameInput.text;

        if (desiredName == HighscoreManager.PlayerName) return;

        if (!HighscoreManager.IsNameValid(desiredName))
        {
            return;
        }

        nameInput.text = await HighscoreManager.SetPlayerName(desiredName);
    }

    public void SaveMusicVolume()
    {
        SettingsManager.SetFloat(SettingsManager.KEY_MUSIC_VOLUME, musicSlider.value);
    }

    public void SaveSFXVolume()
    {
        SettingsManager.SetFloat(SettingsManager.KEY_SFX_VOLUME, sfxSlider.value);
    }

    public void SaveScreenFlashToggle()
    {
        SettingsManager.SetBool(SettingsManager.KEY_SCREEN_FLASH_TOGGLE, screenFlashToggle.isOn);
    }

    public void SaveScreenShakeToggle()
    {
        SettingsManager.SetBool(SettingsManager.KEY_SCREEN_SHAKE_TOGGLE, screenShakeToggle.isOn);
    }

    public void SaveVibrationToggle()
    {
        SettingsManager.SetBool(SettingsManager.KEY_VIBRATION_TOGGLE, vibrationToggle.isOn);
    }
}
