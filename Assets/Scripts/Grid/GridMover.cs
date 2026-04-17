using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GridMover : MonoBehaviour
{
    public float moveSpeed = 8f;
    public float moveDelay = 0.05f;

    public Vector3 FacingDirection { get; private set; } = Vector3.forward;

    private bool isMoving;
    private float timer;
    private GridManager grid;
    private PlayerInputActions input;
    private Vector2 move;

    void Awake() => input = new PlayerInputActions();

    void OnEnable()
    {
        input.Player.Enable();
        input.Player.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        input.Player.Move.canceled  += _ => { move = Vector2.zero; timer = 0f; };
    }

    void OnDisable() => input.Player.Disable();

    void Start()
    {
        grid = GridManager.Instance;
        transform.position = grid.SnapToGrid(transform.position);
    }

    void Update()
    {
        if (BattleManager.Instance.InBattle) return;
        
        if (isMoving || move == Vector2.zero) return;

        Vector3 dir = new Vector3(move.x, 0f, move.y);
        if (dir.sqrMagnitude > 1f) dir.Normalize();

        FacingDirection = dir;
        if (dir != Vector3.zero) transform.forward = dir;

        timer += Time.deltaTime;
        if (timer < moveDelay) return;

        TryMove(dir);
    }

    void TryMove(Vector3 dir)
    {
        Vector3 snapped = grid.SnapToGrid(grid.ClampToBounds(transform.position + dir * grid.tileSize));
        Vector2Int target = grid.WorldToGrid(snapped);

        if (!grid.IsWalkable(target)) return;

        StartCoroutine(MoveTo(snapped));
    }

    IEnumerator MoveTo(Vector3 target)
    {
        isMoving = true;
        Vector3 start = transform.position;
        float t = 0f, duration = grid.tileSize / moveSpeed;

        while (t < duration)
        {
            transform.position = Vector3.Lerp(start, target, t / duration);
            t += Time.deltaTime;
            yield return null;
        }

        transform.position = target;
        timer = 0f;
        isMoving = false;

        OnTileLanded(grid.WorldToGrid(target));
    }

    void OnTileLanded(Vector2Int cell)
    {
        switch (grid.GetTileType(cell))
        {
            case TileType.Encounter:
                var table = grid.GetEncounterTable(cell);
                if (table != null && Random.value < 0.1f)
                    BattleManager.Instance.StartWildBattle(table.Roll());
                break;

            case TileType.Warp:
                Debug.Log("Warp triggered!"); // WarpManager.Instance?.Warp(cell);
                break;

            case TileType.Ice:
                TryMove(FacingDirection); // Keep sliding
                break;
        }
    }
}