using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Manage the grid on the server. Spawns the same robots for all players
/// </summary>
public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    private Grid grid;
    [SerializeField]
    private List<GameObject> robots;

    /// <summary>
    /// Grid max coordinates according to the size of the ground/plane
    /// </summary>
    private readonly float gridMaxX = 5f;
    private readonly float gridMaxY = 5f;

    /// <summary>
    /// Saved position to make sure two robots are never spawned in the same cell
    /// </summary>
    private List<Vector3> usedPositions = new List<Vector3>();

    private void Start()
    {
        InitializeRobotsPosition();
        Debug.Log("Sarting");
    }

    public void InitializeRobotsPosition()
    {
        for (int i=0;i<robots.Count;i++)
        {
            var robot = robots[i];
            SpawnRobot(robot);
        }

        usedPositions.Clear();
    }

    public void SpawnRobot(GameObject robot)
    {
        float randomX = Random.Range(-gridMaxX, gridMaxX);
        float randomY = Random.Range(-gridMaxY, gridMaxY);
        Vector3 spawnPosition = new Vector3(randomX, 0f, randomY);

        foreach (var position in usedPositions)
        {
            if (position == spawnPosition)
            {
                Debug.Log("Robot position exists, retrying");
                SpawnRobot(robot);
                return;
            }
        }

        usedPositions.Add(spawnPosition);
        var robotClone = Instantiate(robot, spawnPosition, Quaternion.identity);
        var robotPosition = robotClone.transform.position;
        
        var cellPosition = grid.WorldToCell(robotPosition);
        robotClone.transform.position = grid.GetCellCenterWorld(cellPosition);
    }
}
