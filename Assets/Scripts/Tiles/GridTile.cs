using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public enum TileType
{
    Walkable,
    Obstacle,
    Interactable,
    Encounter,
    Water,
    Ice,
    Warp   
}

public class GridTile
{
    public TileType type = TileType.Walkable;
    public IInteractable interactable; 
    public WildEncounterData encounterData;
    public bool IsWalkable => type == TileType.Walkable || type == TileType.Encounter || type == TileType.Ice || type == TileType.Warp;
}