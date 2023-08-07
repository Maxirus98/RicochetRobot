using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
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
    private NetworkObject networkObject;

    [SerializeField]
    private GameObject selectedRobot;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) Destroy(this);
        placementSystem = GameObject.Find("PlacementSystem").GetComponent<PlacementSystem>();
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
        grid = GameObject.Find("Grid").GetComponent<Grid>();
        networkObject = GetComponent<NetworkObject>();
    }

    private void FixedUpdate()
    {
        if (!IsHost) return;

        foreach (var client in NetworkManager.Singleton.ConnectedClients)
        {
            if (networkObject.IsNetworkVisibleTo(client.Key) && client.Key != NetworkManager.LocalClientId)
            {
                print($"Hide for client with key {client.Key}");
                networkObject.NetworkHide(client.Key);
            }
        }
    }

    private void Update()
    {
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        mouseIndicator.transform.position = mousePosition;
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);
        if(Input.GetMouseButtonDown(0))
        {
            selectedRobot = inputManager.GetSelectedRobot();
            // ONLY VISIBLE TO THE LOCAL CLIENT because you instantiate it on the player network who's a network object only visible to the local client
            var cloneSelectedRobot = Instantiate(selectedRobot);
            var mat = cloneSelectedRobot.GetComponentInChildren<Renderer>().material;
            Color oldColor = mat.color;
            Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, 0.5f);
            mat.SetColor("_Color", newColor);

            // clone a temporary "ghost" of the selected robot 
            // move the temporary ghost with a trail following the grid to see the movements
        }
    }
}
