using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// PlayerManger will manage the actions of the player for the client.
/// Therefore, only the Owner will be able to do and see what he's doing until he sends its answer to the server.
/// </summary>
public class PlayerManager : NetworkBehaviour
{
    [SerializeField]
    private GameObject mouseIndicator, cellIndicator;

    private PlacementSystem placementSystem;
    private InputManager inputManager;
    private Grid grid;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) Destroy(this);
        placementSystem = GameObject.Find("PlacementSystem").GetComponent<PlacementSystem>();
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
        grid = GameObject.Find("Grid").GetComponent<Grid>();
    }

    private void Update()
    {
        var input = Input.GetMouseButton(0);
        if (input)
        {
        }

        // Done by client
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        mouseIndicator.transform.position = mousePosition;
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);
    }
}
