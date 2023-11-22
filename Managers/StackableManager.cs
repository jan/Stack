using System;
using System.Collections;
using UnityEngine;

public class StackableManager : MonoBehaviour
{
    public static event Action<Stackable> OnNextStackableChosen = (stackable) => { };

    [SerializeField] private Transform container;
    [SerializeField] private Stackable[] prefabs;
    [SerializeField] private ParticleSystem explosionPrefab;
    [SerializeField] private AudioClip explosionClip;
    [SerializeField][Range(0, 1)] private float explosionVolume = 1;
    [SerializeField] private AudioClip placedClip;
    [SerializeField][Range(0, 1)] private float placedVolume = 1;
    [SerializeField] private AudioClip collisionClip;
    [SerializeField][Range(0, 1)] private float collisionVolume = 1;
    [SerializeField] private float contraction = 1;
    [SerializeField] private float explosionDuration = 1;
    [SerializeField] private float yOffset = 3;

    private Stackable nextPrefab;

    private void OnEnable()
    {
        GameManager.OnGameStateChanged += HandleGameStateChanged;
        Stackable.OnStackablePlaced += HandleStackablePlaced;
        Stackable.OnStackableCollided += HandleStackableCollided;
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= HandleGameStateChanged;
        Stackable.OnStackablePlaced -= HandleStackablePlaced;
        Stackable.OnStackableCollided -= HandleStackableCollided;
    }

    private void RandomNextPrefab()
    {
        int index = UnityEngine.Random.Range(0, prefabs.Length);
        nextPrefab = prefabs[index];

        OnNextStackableChosen(nextPrefab);
    }

    public Stackable SpawnNextStackable()
    {
        if (nextPrefab == null) RandomNextPrefab();

        Stackable stackable = Instantiate(nextPrefab, transform.position, Quaternion.identity, container);

        RandomNextPrefab();

        return stackable;
    }

    private void HandleGameStateChanged(GameState state)
    {
        if (state.state != GameState.State.Lost) return;

        StartCoroutine(DelayedExplode());
    }

    private void HandleStackablePlaced(Stackable stackable)
    {
        SFXPlayer.PlayOneshot(placedClip, placedVolume);

        float y = stackable.transform.position.y + yOffset;
        Vector3 position = transform.position;
        if (y > position.y)
        {
            position.y = y;
            transform.position = position;
        }
    }

    private void HandleStackableCollided(Stackable stackable, Collision2D collision)
    {
        float volume = collisionVolume * Mathf.Clamp01(collision.relativeVelocity.magnitude / 10);
        SFXPlayer.PlayOneshot(collisionClip, volume);
    }

    private IEnumerator DelayedExplode()
    {
        Stackable[] stackables = container.GetComponentsInChildren<Stackable>();
        float delayPerStackable = explosionDuration / stackables.Length;

        foreach (Stackable stackable in stackables)
        {
            Vector3 initialScale = stackable.transform.localScale;
            float elapsedTime = 0;
            while (elapsedTime < delayPerStackable)
            {
                float scale = Mathf.SmoothStep(1, 1 - contraction, elapsedTime / delayPerStackable);
                stackable.transform.localScale = initialScale * scale;

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            ParticleSystem explosion = Instantiate(explosionPrefab, stackable.transform.position, Quaternion.identity, transform);
            ParticleSystem.MainModule settings = explosion.main;
            settings.startColor = new ParticleSystem.MinMaxGradient(stackable.ExplosionColor);

            SFXPlayer.PlayOneshot(explosionClip, explosionVolume);

            Destroy(stackable.gameObject);
        }
    }
}