using UnityEngine;

public class GridObstacle : MonoBehaviour
{
    public TileType tileType = TileType.Obstacle;

    void Start()
    {
        GridManager grid = GridManager.Instance;
        Vector2Int tile = grid.WorldToGrid(transform.position);
        if (!grid.InBounds(tile)) return;
        grid.SetTile(tile, tileType);
    }
}