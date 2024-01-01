using System.Collections.Generic;
using System.Linq;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// PlayerManger will manage the actions of the player for the client.
/// Therefore, only the Owner will be able to do and see what he's doing until he sends its answer to the server.
/// </summary>
public class PlayerManager : MonoBehaviour
{
    public UnityEvent OnClickEvent;
    public LayerMask robotLayerMask;
    [SerializeField]
    private GameObject mouseIndicator, cellIndicator;
    [SerializeField]
    private MouseManager mouseManager;

    private InputManager inputManager;
    private Grid grid;

    [SerializeField]
    private GameObject selectedRobot;
    [SerializeField]
    private GameObject movementIndicator;

    private List<GameObject> movementIndicators;

    private void Awake()
    {
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
        grid = GameObject.Find("Grid").GetComponent<Grid>();
        movementIndicators = new List<GameObject>();
    }

    private void Update()
    {
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        mouseIndicator.transform.position = mousePosition;
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);

        if(selectedRobot != null)
        {
            var robotTransfo = selectedRobot.transform;
            var rayTargets = new[] {
            robotTransfo.forward * 10,
            robotTransfo.right * 10,
            robotTransfo.right * -10,
            robotTransfo.forward * -10
        };

            foreach (var target in rayTargets)
            {
                Debug.DrawRay(selectedRobot.transform.position, target);
            }
        }

        if (Input.GetMouseButton(1))
        {
            selectedRobot = null;
            DestroyAllMovementIndicators();
        }

        if (inputManager.IsRobot())
        {
            DestroyAllMovementIndicators();
            CheckForObstacles();
            if (inputManager.IsMoveable())
            {
                MoveRobotToCell(gridPosition);
            }
        }
    }

    private void DestroyAllMovementIndicators()
    {
        foreach (var moveIndic in movementIndicators)
        {
            Destroy(moveIndic);
        };

        movementIndicators.Clear();
    }

    private void CheckForObstacles()
    {
        print("clicked robot");
        selectedRobot = inputManager.GetSelectedRobotIndicator();
        var robotTransfo = selectedRobot.transform;
        var rayTargets = new[] {
            robotTransfo.forward * 10,
            robotTransfo.right * 10,
            robotTransfo.right * -10,
            robotTransfo.forward * -10
        };

        foreach (var target in rayTargets)
        {
            CheckForFirstObstacle(target);
        }

    }


    private void CheckForFirstObstacle(Vector3 rayTarget)
    {
        if (Physics.Raycast(selectedRobot.transform.position, rayTarget, out RaycastHit hit))
        {
            Vector3Int gridPosition = grid.WorldToCell(hit.point);
            Vector3 pos1 = grid.GetCellCenterWorld(gridPosition);
            if (hit.collider.CompareTag("Robot"))
            {
                pos1 += hit.normal * grid.cellSize.x;
            }

            pos1.y = movementIndicator.transform.localScale.y;
            var cloneIndicator = Instantiate(movementIndicator, pos1, Quaternion.identity);
            movementIndicators.Add(cloneIndicator);
        }
    }

    private void MoveRobotToCell(Vector3Int gridPosition)
    {
        selectedRobot.transform.position = grid.GetCellCenterWorld(gridPosition);
    }
}
