using UnityEngine;

public class Vibration : MonoBehaviour
{
#if !PLATFORM_WEBGL && !UNITY_EDITOR
    private void OnEnable()
    {
        GameManager.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState state)
    {
        if (state.state != GameState.State.Lost) return;
        if (!SettingsManager.GetBool(SettingsManager.KEY_VIBRATION_TOGGLE)) return;

        Handheld.Vibrate();
    }
#endif
}