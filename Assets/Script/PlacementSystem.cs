using System.Collections.Generic;
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
    private HashSet<Vector3> usedPositions = new HashSet<Vector3>();

    /// <summary>
    /// Checks if robots are spawned to clear them
    /// </summary>
    private bool spawned = false;

    private void Start()
    {
        InitializeRobotsPosition();
    }

    public void InitializeRobotsPosition()
    {
        if (spawned)
        {
            var spawnedRobots = GameObject.FindGameObjectsWithTag("Robot");
            foreach (var robot in spawnedRobots)
            {
                Destroy(robot.transform.parent.gameObject);
            }
            usedPositions.Clear();
        }

        SpawnRobot();
    }

    private void SpawnRobot()
    {
        int i = 0;
        while (usedPositions.Count < robots.Count)
        {
            float randomX = Random.Range(-gridMaxX, gridMaxX);
            float randomY = Random.Range(-gridMaxY, gridMaxY);
            Vector3 spawnPosition = new Vector3(randomX, 0f, randomY);
            var robot = robots[i];
            usedPositions.Add(spawnPosition);
            var robotClone = Instantiate(robot, spawnPosition, Quaternion.identity);
            var robotPosition = robotClone.transform.position;
            var cellPosition = grid.WorldToCell(robotPosition);
            robotClone.transform.position = grid.GetCellCenterWorld(cellPosition);
            spawned = true;
            i++;
        }
    }
}
