using UnityEngine;

[System.Serializable]
public class SmoothDampedFloat
{
    public float smoothTime;

    public float current { get; private set; }
    private float velocity;

    public SmoothDampedFloat(float initial, float smoothTime)
    {
        current = initial;
        this.smoothTime = smoothTime;
    }

    public float Update(float ideal)
    {
        current = Mathf.SmoothDamp(current, ideal, ref velocity, smoothTime);

        return current;
    }
}
