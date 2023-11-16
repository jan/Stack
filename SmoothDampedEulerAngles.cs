using UnityEngine;

[System.Serializable]
public class SmoothDampedEulerAngles
{
    public float smoothTime;

    private Vector3 current;
    private Vector3 velocity;
    private bool scaled;

    public SmoothDampedEulerAngles(Vector3 initial, float smoothTime, bool scaled)
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
            Mathf.SmoothDampAngle(current.x, ideal.x, ref velocity.x, smoothTime, maxSpeed, deltaTime),
            Mathf.SmoothDampAngle(current.y, ideal.y, ref velocity.y, smoothTime, maxSpeed, deltaTime),
            Mathf.SmoothDampAngle(current.z, ideal.z, ref velocity.z, smoothTime, maxSpeed, deltaTime)
        );

        return current;
    }
}
