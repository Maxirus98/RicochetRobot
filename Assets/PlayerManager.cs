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
    private PlacementSystem placementSystem;

    private void Update()
    {
        if (!IsOwner) return;
        var input = Input.GetMouseButton(0);

        Vector3 cellPosition = placementSystem.GetCellPosition();
        if(input)
        {
        }
    }
}
