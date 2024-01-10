using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private GameObject movementIndicator;
    [SerializeField]
    private MissionManager missionManager;
    [SerializeField]
    private GameObject cellIndicator;

    private GameObject selectedRobot;
    private InputManager inputManager;
    private Grid grid;
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
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);

        if(selectedRobot != null)
        {
            DrawDebugRays();
        }

        if (Input.GetMouseButton(1))
        {
            selectedRobot = null;
            DestroyAllMovementIndicators();
        }

        if (Input.GetMouseButtonDown(0))
        {
            DestroyAllMovementIndicators();
            if (inputManager.IsRobot())
            {
                CheckForObstacles();
            }

            if (inputManager.IsMoveable())
            {
                MoveRobotToCell(gridPosition);
            }
        }
    }

    private void DrawDebugRays()
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
        var RAY_LENGHT = 10f;
        selectedRobot = inputManager.GetSelectedRobotIndicator();
        var robotTransfo = selectedRobot.transform;
        var rayTargets = new[] {
            robotTransfo.forward * RAY_LENGHT,
            robotTransfo.right * RAY_LENGHT,
            robotTransfo.right * -RAY_LENGHT,
            robotTransfo.forward * -RAY_LENGHT
        };

        foreach (var target in rayTargets)
        {
            CheckForFirstObstacle(target);
        }
    }

    private void CheckForFirstObstacle(Vector3 rayTarget)
    {
        var robotPos = selectedRobot.transform.position;
        if (Physics.Raycast(robotPos, rayTarget, out RaycastHit hit))
        {
            Vector3Int gridPosition = grid.WorldToCell(hit.point);
            Vector3 rayHitPos = grid.GetCellCenterWorld(gridPosition);
            if (hit.collider.CompareTag("Robot"))
            {
                rayHitPos += hit.normal * grid.cellSize.x;
            }

            rayHitPos.y = movementIndicator.transform.localScale.y;

            if ((rayHitPos - robotPos).sqrMagnitude < 0.1f) return;
            var cloneIndicator = Instantiate(movementIndicator, rayHitPos, Quaternion.identity);
            movementIndicators.Add(cloneIndicator);
        }
    }

    private void MoveRobotToCell(Vector3Int gridPosition)
    {
        selectedRobot.transform.position = grid.GetCellCenterWorld(gridPosition);
        missionManager.IncrementMoveCount();
    }
}
