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

        Handheld.Vibrate();
    }
#endif
}