using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manage the grid and the interactions with it. Only the owner will be able to use this until the client sends it's answer to the server.
/// </summary>
public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    GameObject mouseIndicator, cellIndicator;
    [SerializeField]
    private InputManager inputManager;
    [SerializeField]
    private Grid grid;

    private void Update()
    {
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        mouseIndicator.transform.position = mousePosition;
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);
    }

    public Vector3 GetCellPosition()
    {
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        return grid.CellToWorld(gridPosition);
    }
}
