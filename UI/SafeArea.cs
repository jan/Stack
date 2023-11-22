using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class SafeArea : MonoBehaviour
{
    [SerializeField] private float minX = 40;
    [SerializeField] private float minY = 40;

    private void OnEnable()
    {
        ScreenResolutionMonitor.OnScreenResolutionChanged += OnScreenResolutionChanged;
    }

    private void OnDisable()
    {
        ScreenResolutionMonitor.OnScreenResolutionChanged -= OnScreenResolutionChanged;
    }

    private void OnScreenResolutionChanged(Vector2 _resolution, bool _landscape, DeviceOrientation _deviceOrientation)
    {
        Apply();
    }

    [ContextMenu("Apply Safe Area")]
    private void Apply()
    {
        Canvas canvas = GetComponentInParent<Canvas>();
        RectTransform rectTransform = GetComponent<RectTransform>();

        Rect safeArea = Screen.safeArea;

        if (safeArea.xMin < minX) safeArea.xMin = minX;
        if (safeArea.xMax > canvas.pixelRect.xMax - minX) safeArea.xMax = canvas.pixelRect.xMax - minX;

        if (safeArea.yMin < minY) safeArea.yMin = minY;
        if (safeArea.yMax > canvas.pixelRect.yMax - minY) safeArea.yMax = canvas.pixelRect.yMax - minY;

        Vector2 position = safeArea.position;
        Vector2 size = safeArea.size;

        Vector2 anchorMin = position;
        Vector2 anchorMax = position + size;
        anchorMin.x /= canvas.pixelRect.width;
        anchorMin.y /= canvas.pixelRect.height;
        anchorMax.x /= canvas.pixelRect.width;
        anchorMax.y /= canvas.pixelRect.height;

        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;
    }
}
