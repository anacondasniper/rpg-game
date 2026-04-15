using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    private GridMover mover;
    private GridManager grid;
    private PlayerInputActions inputActions;

    void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Interact.started += OnInteract;
    }

    void OnDisable()
    {
        inputActions.Player.Interact.started -= OnInteract;
        inputActions.Player.Disable();
    }

    void Start()
    {
        mover = GetComponent<GridMover>();
        grid = GridManager.Instance;
    }

    void OnInteract(InputAction.CallbackContext ctx)
    {
        Vector2Int playerTile = grid.WorldToGrid(transform.position);

        Vector2Int dir = Vector2Int.RoundToInt(
            new Vector2(mover.FacingDirection.x, mover.FacingDirection.z)
        );

        if (dir == Vector2Int.zero)
            dir = Vector2Int.up;

        Vector2Int targetTile = playerTile + dir;

        if (targetTile.x < 0 || targetTile.x >= grid.gridWidth ||
            targetTile.y < 0 || targetTile.y >= grid.gridHeight)
            return;

        var tile = grid.tiles[targetTile.x, targetTile.y];

        foreach (var obj in tile.interactables)
        {
            obj.Interact(gameObject);
            return;
        }
    }

    void OnDrawGizmos()
    {
        if (mover == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, mover.FacingDirection);
    }
}