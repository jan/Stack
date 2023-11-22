using UnityEngine;
using UnityEngine.UI;

public class UIScreen : MonoBehaviour
{
    [SerializeField] private Selectable selectFirst;
    [SerializeField] protected GameObject visual;
    [SerializeField] private bool startActive = false;

    private bool select = false;

    private void Start()
    {
        SetActive(startActive);
    }

    protected virtual void Update()
    {
        if (select)
        {
            select = false;
            SetInitialSelection();
        }

        if (Input.GetMouseButtonDown(0))
        {
            Canvas canvas = GetComponentInParent<Canvas>();
            Camera camera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : Camera.main;
            if (!RectTransformUtility.RectangleContainsScreenPoint(visual.GetComponent<RectTransform>(), Input.mousePosition, camera))
            {
                SetActive(false);
            }
        }
    }

    public virtual void SetActive(bool active)
    {
        if (visual.activeSelf == active) return;

        visual.SetActive(active);
        if (active)
        {
            select = true;
            OnActivate();
        }
    }

    protected virtual void OnActivate() { }

    private void SetInitialSelection()
    {
        if (selectFirst) selectFirst.Select();
    }
}
