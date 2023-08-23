using UnityEngine;

/// <summary>
/// PlayerManger will manage the actions of the player for the client.
/// Therefore, only the Owner will be able to do and see what he's doing until he sends its answer to the server.
/// </summary>
public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mouseIndicator, cellIndicator;

    private PlacementSystem placementSystem;
    private InputManager inputManager;
    private Grid grid;

    [SerializeField]
    private GameObject selectedRobot;

    private void Awake()
    {
        placementSystem = GameObject.Find("PlacementSystem").GetComponent<PlacementSystem>();
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
        grid = GameObject.Find("Grid").GetComponent<Grid>();
    }

    private void Update()
    {
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        mouseIndicator.transform.position = mousePosition;
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);

        if (Input.GetMouseButton(1))
        {
            selectedRobot = null;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if(selectedRobot == null)
            {
                selectedRobot = inputManager.GetSelectedRobotIndicator();
                return;
            }

            MoveRobotIndicatorToCellPosition(gridPosition);
        }
    }

    private void MoveRobotIndicatorToCellPosition(Vector3Int gridPosition)
    {
        selectedRobot.transform.position = grid.GetCellCenterWorld(gridPosition);
    }
}
