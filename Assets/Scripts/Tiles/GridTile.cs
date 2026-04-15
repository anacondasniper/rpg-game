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
    public bool walkable = true;
    public IInteractable interactable; 
}