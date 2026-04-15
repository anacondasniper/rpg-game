using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GridMover : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 8f;
    public float moveDelay = 0.05f;

    public Vector3 FacingDirection { get; private set; } = Vector3.forward;

    private bool isMoving = false;
    private float inputHoldTimer = 0f;
    private GridManager grid;

    private PlayerInputActions inputActions;
    private Vector2 moveInput;

    void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    void OnEnable()
    {
        inputActions.Player.Enable();

        inputActions.Player.Move.performed += OnMovePerformed;
        inputActions.Player.Move.canceled += OnMoveCanceled;
    }

    void OnDisable()
    {
        inputActions.Player.Move.performed -= OnMovePerformed;
        inputActions.Player.Move.canceled -= OnMoveCanceled;

        inputActions.Player.Disable();
    }

    void Start()
    {
        grid = GridManager.Instance;

        transform.position = grid.SnapToGrid(transform.position);
    }

    void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }
    
    void OnMoveCanceled(InputAction.CallbackContext ctx)
    {
        moveInput = Vector2.zero;
        inputHoldTimer = 0f;
    }

    void Update()
    {
        if (isMoving || moveInput == Vector2.zero) return;

        Vector3 inputDir = new Vector3(moveInput.x, 0f, moveInput.y);

        if (inputDir.sqrMagnitude > 1f)
            inputDir.Normalize();

        FacingDirection = inputDir;

        if (FacingDirection != Vector3.zero)
            transform.forward = FacingDirection;

        inputHoldTimer += Time.deltaTime;
        if (inputHoldTimer > 0.01f && inputHoldTimer < moveDelay)
            return;

        TryMove(inputDir);
    }

    void TryMove(Vector3 direction)
    {
        Vector3 rawTarget = transform.position + direction * grid.tileSize;

        Vector3 clamped = grid.ClampToBounds(rawTarget);
        Vector3 targetPos = grid.SnapToGrid(clamped);

        if (grid.IsWalkable(targetPos))
            StartCoroutine(SlideTo(targetPos));
    }

    IEnumerator SlideTo(Vector3 targetPos)
    {
        isMoving = true;
        Vector3 startPos = transform.position;
        float elapsed = 0f;
        float duration = grid.tileSize / moveSpeed;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;
        inputHoldTimer = 0f;
        isMoving = false;
    }
}
