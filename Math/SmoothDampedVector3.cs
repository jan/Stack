using UnityEngine;

[System.Serializable]
public class SmoothDampedVector3
{
    public float smoothTime;

    private Vector3 current;
    private Vector3 velocity;
    private bool scaled;

    public SmoothDampedVector3(Vector3 initial, float smoothTime, bool scaled)
    {
        current = initial;
        this.smoothTime = smoothTime;
        this.scaled = scaled;
    }

    public Vector3 Update(Vector3 ideal)
    {
        float maxSpeed = float.PositiveInfinity;
        float deltaTime = scaled ? Time.deltaTime : Time.unscaledDeltaTime;

        current = new Vector3(
            Mathf.SmoothDamp(current.x, ideal.x, ref velocity.x, smoothTime, maxSpeed, deltaTime),
            Mathf.SmoothDamp(current.y, ideal.y, ref velocity.y, smoothTime, maxSpeed, deltaTime),
            Mathf.SmoothDamp(current.z, ideal.z, ref velocity.z, smoothTime, maxSpeed, deltaTime)
        );

        return current;
    }
}
