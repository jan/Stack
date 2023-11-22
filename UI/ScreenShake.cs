using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class CameraShake : MonoBehaviour
{
    [SerializeField] private float duration = .1f;
    [SerializeField] private float magnitude = .5f;

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
        if (!SettingsManager.GetBool(SettingsManager.KEY_SCREEN_SHAKE_TOGGLE)) return;

        StartCoroutine(Shake(duration, magnitude));
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 orignalPosition = transform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            float z = 0;

            transform.localPosition = orignalPosition + new Vector3(x, y, -z);
            elapsed += Time.deltaTime;
            yield return 0;
        }

        transform.position = orignalPosition;
    }
}