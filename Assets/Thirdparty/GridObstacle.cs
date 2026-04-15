using UnityEngine;

public class GridObstacle : MonoBehaviour
{
    private void Start()
    {
        GridManager grid = GridManager.Instance;

        Vector2Int tile = grid.WorldToGrid(transform.position);

        grid.SetTileType(tile.x, tile.y, TileType.Obstacle);
    }
}