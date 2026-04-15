using UnityEngine;
using System.Collections.Generic;

public enum TileType
{
    Walkable,
    Obstacle,
    Interactable   
}

public class GridTile
{
    public TileType type = TileType.Walkable;

    public List<IInteractable> interactables = new List<IInteractable>();

    public bool IsWalkable()
    {
        return type != TileType.Obstacle;
    }
}