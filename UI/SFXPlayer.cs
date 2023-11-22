using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SFXPlayer : MonoBehaviour
{
    public static SFXPlayer instance;

    private AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    public static void PlayOneshot(AudioClip clip)
    {
        if (clip) instance.source.PlayOneShot(clip);
    }

    public static void PlayOneshot(AudioClip clip, float volumeScale)
    {
        if (clip) instance.source.PlayOneShot(clip, volumeScale);
    }

    private void OnEnable()
    {
        instance = this;
    }
}
