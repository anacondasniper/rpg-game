using Unity.Collections;
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
        for (int x = 0 ; x < gridWidth; x++)
        for (int z = 0; z < gridHeight; z++)
            tiles[x, z] = new GridTile();
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
    // Tiles
    public TileType GetTileType(Vector2Int pos) =>
        InBounds(pos) ? tiles[pos.x, pos.y].type : TileType.Obstacle;

    public void SetTile(Vector2Int pos, TileType type)
    {
        if (InBounds(pos)) tiles[pos.x, pos.y].type = type;
    }

    public bool IsWalkable(Vector2Int pos) =>
        InBounds(pos) && tiles[pos.x, pos.y].IsWalkable;

    public void RegisterInteractable(Vector2Int pos, IInteractable obj)
    {
        if (!InBounds(pos)) return;
        tiles[pos.x, pos.y].interactable = obj;
        tiles[pos.x, pos.y].type = TileType.Interactable;
    }

    public void RegisterEncounter(Vector2Int pos, WildEncounterData data)
    {
        if (!InBounds(pos)) return;
        tiles[pos.x, pos.y].encounterData = data;
        tiles[pos.x, pos.y].type = TileType.Encounter;
    }

    public WildEncounterData GetEncounterTable(Vector2Int pos)
    {
        if (tiles[pos.x, pos.y].type != TileType.Encounter) return null;
        return tiles[pos.x, pos.y].encounterData;
    }

    void OnDrawGizmos()
    {
        if (tiles == null) return;
        for (int x = 0; x < gridWidth; x++)
        for (int z = 0; z < gridHeight; z++)
        {
            Gizmos.color = tiles[x, z].type switch
            {
                TileType.Obstacle     => new Color(1f,   0.2f, 0.2f, 0.5f),
                TileType.Encounter    => new Color(0.2f, 0.8f, 0.2f, 0.5f),
                TileType.Water        => new Color(0.2f, 0.4f, 1f,   0.5f),
                TileType.Ice          => new Color(0.6f, 0.9f, 1f,   0.5f),
                TileType.Warp         => new Color(1f,   0.8f, 0f,   0.5f),
                TileType.Interactable => new Color(0.8f, 0.2f, 0.8f, 0.5f),
                _                     => new Color(1f,   1f,   1f,   0.04f),
            };
            Vector3 center = GridToWorld(new Vector2Int(x, z));
            Gizmos.DrawCube(center, new Vector3(tileSize * 0.9f, 0.02f, tileSize * 0.9f));
        }
    }
}