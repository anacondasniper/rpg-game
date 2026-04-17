using UnityEngine;

public class EncounterTile : MonoBehaviour
{
    public WildEncounterData wildEncounterData;
    void Start()
    {
        var grid = GridManager.Instance;
        grid.RegisterEncounter(grid.WorldToGrid(transform.position), wildEncounterData);
    }
}
