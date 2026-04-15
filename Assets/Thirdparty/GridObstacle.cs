using UnityEngine;

public class GridObstacle : MonoBehaviour
{
    void Start()
    {
        GridManager grid = GridManager.Instance;

        Vector2Int tile = grid.WorldToGrid(transform.position);

        if (!grid.InBounds(tile)) return;

        grid.SetObstacle(tile);
    }
}