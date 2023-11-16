using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static event Action<GameState> OnGameStateChanged = (state) => { };

    [SerializeField] private StackableSpawner stackableSpawner;
    [SerializeField] private LayerMask placementLayerMask;
    [SerializeField] private float spawnDelay = .5f;
    [SerializeField] private float restartDelay = 1.25f;

    private Stackable currentStackable;
    private GameState state;
    private bool dragging = false;
    private Vector3 lastCursorPosition;
    private ContactFilter2D overlapFilter;
    private Collider2D[] overlapResults = new Collider2D[1];

    private void Awake()
    {
        overlapFilter = new ContactFilter2D();
        overlapFilter.SetLayerMask(placementLayerMask);
        overlapFilter.useTriggers = false; // Don't include triggers in the overlap check

        state = new GameState(GameState.State.Playing);
        OnGameStateChanged(state);
    }

    private void OnEnable()
    {
        Stackable.OnStackableSpawned += HandleStackableSpawned;
        Stackable.OnStackablePlaced += HandleStackablePlaced;
        Stackable.OnStackableFallen += HandleStackableFallen;
    }

    private void OnDisable()
    {
        Stackable.OnStackableSpawned -= HandleStackableSpawned;
        Stackable.OnStackablePlaced -= HandleStackablePlaced;
        Stackable.OnStackableFallen -= HandleStackableFallen;
    }

    private void Start()
    {
        SpawnNextObject();
    }

    private void Update()
    {
        if (state.state != GameState.State.Playing) return;
        if (currentStackable == null) return;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentStackable.transform.Rotate(0, 0, +15);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            currentStackable.transform.Rotate(0, 0, -15);
        }

        // Handle rotation for touch input
        if (Input.touchCount == 2)
        {
            dragging = false;

            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the vector (magnitude and direction) in the previous frame and current frame.
            Vector2 prevVector = touchOnePrevPos - touchZeroPrevPos;
            Vector2 currentVector = touchOne.position - touchZero.position;

            // Calculate the rotation angle difference between these two vectors.
            float angle = Vector2.SignedAngle(prevVector, currentVector);

            // Rotate the stackable object around the z-axis, at a pivot in the center.
            currentStackable.transform.Rotate(0, 0, angle);
        }
        else if (Input.GetMouseButtonDown(0))
        {
            dragging = true;
            lastCursorPosition = GetCursorPosition();
        }

        if (dragging)
        {
            MoveCurrentStackableWithCursor();
            // MoveCurrentStackableToCursor();
        }
    }

    private Vector3 GetCursorPosition()
    {
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursorPosition.z = 0; // Set z to 0 for 2D
        return cursorPosition;
    }

    private void MoveCurrentStackableWithCursor()
    {
        if (currentStackable == null) return;

        Vector3 cursorPosition = GetCursorPosition();
        Vector3 delta = cursorPosition - lastCursorPosition;
        Vector3 position = currentStackable.transform.position;
        currentStackable.transform.position = position + delta;

        lastCursorPosition = cursorPosition;

        if (IsStackableColliding(currentStackable.Collider2D))
        {
            PlaceCurrentStackable();
        }
    }

    private void MoveCurrentStackableToCursor()
    {
        if (currentStackable == null) return;

        currentStackable.transform.position = GetCursorPosition();

        if (IsStackableColliding(currentStackable.Collider2D))
        {
            PlaceCurrentStackable();
        }
    }

    private bool IsStackableColliding(Collider2D stackableCollider)
    {
        // OverlapCollider returns the number of colliders overlapping.
        int numColliders = stackableCollider.OverlapCollider(overlapFilter, overlapResults);
    
        // If numColliders > 0, at least one other collider overlaps with the stackableCollider.
        return numColliders > 0; // Can place if no overlap found
    }

    private void PlaceCurrentStackable()
    {
        Debug.Log("PlaceCurrentStackable " + currentStackable, currentStackable);

        currentStackable.Place();

        state.stackables += 1;
        state.score = state.stackables * 110;
        OnGameStateChanged(state);

        currentStackable = null;
        dragging = false;
    }

    private void HandleStackableSpawned(Stackable stackable)
    {
        Debug.Log("HandleStackableSpawned " + stackable, stackable);

        currentStackable = stackable;
    }

    private void HandleStackablePlaced(Stackable stackable)
    {
        Debug.Log("HandleStackablePlaced " + stackable, stackable);

        Invoke("SpawnNextObject", spawnDelay);
    }

    private void HandleStackableFallen(Stackable stackabke)
    {
        if (state.state != GameState.State.Playing) return;

        state.state = GameState.State.Lost;
        OnGameStateChanged(state);

        Invoke("Restart", restartDelay);
    }

    private void SpawnNextObject()
    {
        if (state.state != GameState.State.Playing) return;

        stackableSpawner.SpawnNextStackable();
    }

    private void Restart()
    {
        state = new GameState(GameState.State.Playing);
        OnGameStateChanged(state);

        Invoke("SpawnNextObject", spawnDelay);
    }
}