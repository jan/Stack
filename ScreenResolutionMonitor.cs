using System;
using UnityEngine;

public class ScreenResolutionMonitor : MonoBehaviour
{
    public static event Action<Vector2, bool, DeviceOrientation> OnScreenResolutionChanged = (resolution, landscape, deviceOrientation) => { };

    private Vector2 resolution = new Vector2();

    private void Update()
    {
        Vector2 resolution = new Vector2(Screen.width, Screen.height);

        if (resolution != this.resolution)
        {
            this.resolution = resolution;

            bool landscape = resolution.x > resolution.y;
            DeviceOrientation deviceOrientation = Input.deviceOrientation;
            if (deviceOrientation == DeviceOrientation.Unknown) deviceOrientation = landscape ? DeviceOrientation.LandscapeLeft : DeviceOrientation.Portrait;

            OnScreenResolutionChanged(resolution, landscape, deviceOrientation);
        }
    }
}
