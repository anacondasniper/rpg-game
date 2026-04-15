using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    void OnEnable()
    {
        GridManager grid = GridManager.Instance;

        Vector2Int tile = grid.WorldToGrid(transform.position);
        grid.RegisterInteractable(tile, this);
    }

    public void Interact(GameObject interactor)
    {
        Debug.Log(interactor.name + " opened chest!");
    }
}