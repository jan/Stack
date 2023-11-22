using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraZoom : MonoBehaviour
{
    [SerializeField] private float smoothTime = .25f;

    private new Camera camera;
    private Bounds bounds = new Bounds();
    private Vector3 idealPosition;
    private SmoothDampedVector3 smoothPosition;

    private void Awake()
    {
        camera = GetComponent<Camera>();
        idealPosition = camera.transform.position;
        smoothPosition = new SmoothDampedVector3(idealPosition, smoothTime, true);
    }

    private void OnEnable()
    {
        Stackable.OnStackablePlaced += HandleStackablePlaced;
    }

    private void OnDisable()
    {
        Stackable.OnStackablePlaced -= HandleStackablePlaced;
    }

    private void Start()
    {
        foreach (SpriteRenderer spriteRenderer in FindObjectsOfType<SpriteRenderer>())
        {
            bounds.Encapsulate(spriteRenderer.bounds);
        }
        bounds.Encapsulate(FindObjectOfType<StackableManager>().transform.position);

        Refocus();
    }

    private void Update()
    {
        camera.transform.position = smoothPosition.Update(idealPosition);
    }

    private void HandleStackablePlaced(Stackable stackable)
    {
        bounds.Encapsulate(stackable.SpriteRenderer.bounds);

        Refocus();
    }

    private void Refocus()
    {
        Vector3 size = bounds.size;
        float diagonal = Mathf.Sqrt(size.x * size.x + size.y * size.y + size.z * size.z);

        camera.orthographicSize = diagonal / 2 + 1;

        idealPosition = new Vector3(0, bounds.center.y + 1.5f, -10);
    }
}
