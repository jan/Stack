using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(PolygonCollider2D))]
public class Stackable : MonoBehaviour
{
    public static event Action<Stackable> OnStackableFallen = (stackable) => { };
    public static event Action<Stackable> OnStackableSpawned = (stackable) => { };
    public static event Action<Stackable> OnStackablePlaced = (stackable) => { };
    public static event Action<Stackable, Collision2D> OnStackableCollided = (stackable, collision) => { };

    public SpriteRenderer SpriteRenderer => ren;
    public Collider2D Collider2D => col;
    public Sprite Sprite => GetComponent<SpriteRenderer>().sprite;
    public Color Color => GetComponent<SpriteRenderer>().color;
    public Color ExplosionColor => explosionColor;
    public bool Active { get; private set; } = false;
    public bool Placed { get; private set; } = false;
    public int Value => value;

    [SerializeField] private float boundaryY = -2;
    [SerializeField] private float alphaBeforePlacement = .5f;
    [SerializeField] private Color explosionColor = Color.black;
    [SerializeField] private int value = 110;

    private SpriteRenderer ren;
    private Rigidbody2D rb;
    private Collider2D col;
    private Color originalColor;

    protected virtual void Awake()
    {
        ren = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        originalColor = ren.color;
        if (explosionColor == Color.black) explosionColor = originalColor;
        ren.color = ColorUtil.Alpha(ren.color, alphaBeforePlacement);

        rb.isKinematic = true; // Start as kinematic, not affected by physics
    }

    private void Start()
    {
        OnStackableSpawned(this);
    }

    private void Update()
    {
        if (!Placed) return;

        if (transform.position.y < boundaryY)
        {
            enabled = false;
            OnStackableFallen(this);
        }
    }

    private void OnBecameInvisible()
    {
        enabled = false;
        OnStackableFallen(this);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnStackableCollided(this, collision);
    }

    public void Place()
    {
        ren.color = originalColor;
        rb.isKinematic = false; // Allow physics to affect the object
        Placed = true;
        OnStackablePlaced(this);
    }
}