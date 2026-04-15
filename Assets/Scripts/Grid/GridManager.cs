using UnityEditor.Rendering;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }

    [Header("Grid Settings")]
    public float tileSize = 1f;
    public int gridWidth = 20;
    public int gridHeight = 20;

    public GridTile[,] tiles;

    public Vector3 Origin => transform.position;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        tiles = new GridTile[gridWidth, gridHeight];

        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            {
                Vector3 pos = Origin + new Vector3(x * tileSize, 0f, z * tileSize);
                tiles[x, z] = new GridTile();
            }
        }
    }
    private void Start()
    {
        RegisterObstacles(); // <-- ADD THIS
    }

    private void RegisterObstacles()
    {
        GridObstacle[] obstacles = FindObjectsOfType<GridObstacle>();

        foreach (var obs in obstacles)
        {
            Vector2Int tile = WorldToGrid(obs.transform.position);

            if (tile.x < 0 || tile.x >= gridWidth ||
                tile.y < 0 || tile.y >= gridHeight)
                continue;

            tiles[tile.x, tile.y].type = TileType.Obstacle;
        }
    }

   public void RegisterInteractable(Vector2Int pos, IInteractable obj)
    {
        if (pos.x < 0 || pos.x >= gridWidth ||
            pos.y < 0 || pos.y >= gridHeight)
            return;

        tiles[pos.x, pos.y].interactables.Add(obj);
    }

    public Vector3 SnapToGrid(Vector3 worldPos)
    {
        Vector3 local = worldPos - Origin;

        float x = Mathf.Round(local.x / tileSize) * tileSize;
        float z = Mathf.Round(local.z / tileSize) * tileSize;

        return Origin + new Vector3(x, worldPos.y, z);
    }

    public Vector2Int WorldToGrid(Vector3 worldPos)
    {
        Vector3 local = worldPos - Origin;

        int x = Mathf.RoundToInt(local.x / tileSize);
        int z = Mathf.RoundToInt(local.z / tileSize);

        return new Vector2Int(x, z);
    }

    public Vector3 GridToWorld(Vector2Int gridPos)
    {
        return Origin + new Vector3(gridPos.x * tileSize, 0f, gridPos.y * tileSize);
    }

    public bool IsWalkable(Vector3 worldPos)
    {
        Vector2Int gridPos = WorldToGrid(worldPos);

        if (gridPos.x < 0 || gridPos.x >= gridWidth ||
            gridPos.y < 0 || gridPos.y >= gridHeight)
            return false;

        return tiles[gridPos.x, gridPos.y].IsWalkable();
    }

    public void SetTileType(int x, int z, TileType type)
    {
        if (tiles == null)
            return;

        if (x < 0 || x >= gridWidth || z < 0 || z >= gridHeight)
            return;

        tiles[x, z].type = type;
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
    void OnDrawGizmos()
    {
        if (gridWidth <= 0 || gridHeight <= 0) return;

        Gizmos.color = new Color(1f, 1f, 1f, 0.15f);

        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            {
                Vector3 center = Origin + new Vector3(x * tileSize, 0f, z * tileSize);
                Gizmos.DrawWireCube(center, new Vector3(tileSize, 0.05f, tileSize));
            }
        }
    }
}