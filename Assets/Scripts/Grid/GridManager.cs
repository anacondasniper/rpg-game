using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }

    public float tileSize = 1f;
    public int gridWidth = 20;
    public int gridHeight = 20;

    public GridTile[,] tiles;

    public Vector3 Origin => transform.position;

    void Awake()
    {
        Instance = this;

        tiles = new GridTile[gridWidth, gridHeight];

        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            {
                tiles[x, z] = new GridTile();
            }
        }
    }

    public Vector2Int WorldToGrid(Vector3 worldPos)
    {
        Vector3 local = worldPos - Origin;

        return new Vector2Int(
            Mathf.RoundToInt(local.x / tileSize),
            Mathf.RoundToInt(local.z / tileSize)
        );
    }

    public Vector3 GridToWorld(Vector2Int pos)
    {
        return Origin + new Vector3(pos.x * tileSize, 0f, pos.y * tileSize);
    }

    public bool InBounds(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < gridWidth &&
               pos.y >= 0 && pos.y < gridHeight;
    }

    public Vector3 SnapToGrid(Vector3 worldPos)
    {
        Vector3 local = worldPos - Origin;

        float x = Mathf.Round(local.x / tileSize) * tileSize;
        float z = Mathf.Round(local.z / tileSize) * tileSize;

        return Origin + new Vector3(x, worldPos.y, z);
    }

    public Vector3 ClampToBounds(Vector3 worldPos)
    {
        Vector3 local = worldPos - Origin;

        float maxX = (gridWidth - 1) * tileSize;
        float maxZ = (gridHeight - 1) * tileSize;

        local.x = Mathf.Clamp(local.x, 0f, maxX);
        local.z = Mathf.Clamp(local.z, 0f, maxZ);

        return Origin + new Vector3(local.x, worldPos.y, local.z);
    }

    public bool IsWalkable(Vector2Int pos)
    {
        return InBounds(pos) && tiles[pos.x, pos.y].walkable;
    }

    public void SetObstacle(Vector2Int pos)
    {
        if (!InBounds(pos)) return;
        tiles[pos.x, pos.y].walkable = false;
    }

    public void RegisterInteractable(Vector2Int pos, IInteractable obj)
    {
        if (!InBounds(pos)) return;
        tiles[pos.x, pos.y].interactable = obj;
        tiles[pos.x, pos.y].walkable = false;
    }
}