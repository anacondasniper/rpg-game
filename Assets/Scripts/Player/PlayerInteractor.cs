using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    private GridMover mover;
    private GridManager grid;
    private PlayerInputActions input;

    void Awake()
    {
        input = new PlayerInputActions();
    }

    void OnEnable()
    {
        input.Player.Enable();
        input.Player.Interact.started += Interact;
    }

    void OnDisable()
    {
        input.Player.Interact.started -= Interact;
        input.Player.Disable();
    }

    void Start()
    {
        mover = GetComponent<GridMover>();
        grid = GridManager.Instance;
    }

    void Interact(InputAction.CallbackContext ctx)
    {
        Vector2Int player = grid.WorldToGrid(transform.position);

        Vector2Int dir = Vector2Int.RoundToInt(
            new Vector2(mover.FacingDirection.x, mover.FacingDirection.z)
        );

        if (dir == Vector2Int.zero)
            dir = Vector2Int.up;

        Vector2Int target = player + dir;

        if (!grid.InBounds(target)) return;

        var tile = grid.tiles[target.x, target.y];

        if (tile.interactable != null)
            tile.interactable.Interact(gameObject);
    }

    void OnDrawGizmos()
    {
        if (mover == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, mover.FacingDirection);
    }
}