using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ScreenFlash : MonoBehaviour
{
    [SerializeField] private float duration = 0.15f;
    [SerializeField] private Color playingColor;
    [SerializeField] private Color refuseColor;
    [SerializeField] private Color lostColor;
    [SerializeField] private Color wonColor;

    private bool playing = false;
    private Image image;
    private Coroutine flash;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

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
        switch (state.state)
        {
            case GameState.State.Playing:
                if (!playing) StartFlash(playingColor);
                playing = true;
                break;
            case GameState.State.Lost:
                StartFlash(lostColor);
                playing = false;
                break;
            case GameState.State.Won:
                StartFlash(wonColor);
                playing = false;
                break;
        }
    }

    private void StartFlash(Color color)
    {
        if (flash != null) StopCoroutine(flash);

        flash = StartCoroutine(Flash(color));
    }

    private IEnumerator Flash(Color color)
    {
        float fadeTime = duration / 2;
        Color frameColor = new Color(color.r, color.g, color.b, 0);

        // Fade in
        for (float t = 0; t <= fadeTime; t += Time.deltaTime)
        {
            frameColor.a = Mathf.Lerp(0, color.a, t / fadeTime);
            image.color = frameColor;
            yield return null;
        }

        // Fade out
        for (float t = 0; t <= fadeTime; t += Time.deltaTime)
        {
            frameColor.a = Mathf.Lerp(color.a, 0, t / fadeTime);
            image.color = frameColor;
            yield return null;
        }

        // Ensure alpha is set to zero
        image.color = new Color(0, 0, 0, 0);
    }
}
