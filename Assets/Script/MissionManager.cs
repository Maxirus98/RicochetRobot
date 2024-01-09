using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI moveCountText;
    [SerializeField]
    private Image currentMissionImage;
    [SerializeField]
    private List<Sprite> missionTokens = new List<Sprite>();

    private int movementCount = 0;
    private List<int> randomizedMissionList = new List<int>();

    private int currentMissionIndex = 0;

    private void Start()
    {
        SetRandomMissions();
        currentMissionImage.sprite = missionTokens[randomizedMissionList[currentMissionIndex]];
    }

    private void SetRandomMissions()
    {
        for(int i = 0; i < missionTokens.Count; i++)
        {
            randomizedMissionList.Add(i);
        }

        ShuffleList(randomizedMissionList);

        for (int i = 0; i < randomizedMissionList.Count; i++)
        {
            print(randomizedMissionList[i]);
        }

    }

    private void ShuffleList(List<int> list)
    {
        System.Random random = new System.Random();

        int n = list.Count;
        for (int i = n - 1; i > 0; i--)
        {
            // Generate a random index between 0 and i (inclusive)
            int randomIndex = random.Next(0, i + 1);

            // Swap the elements at randomIndex and i
            int temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    public void NextMission()
    {
        if(currentMissionIndex < missionTokens.Count - 1) 
        { 
            currentMissionIndex++; 
        } else
        {
            currentMissionIndex = 0;
        }

        currentMissionImage.sprite = missionTokens[randomizedMissionList[currentMissionIndex]];
        var robots = GameObject.FindGameObjectsWithTag("Robot");
        for (int i = 0; i < robots.Length; i++)
        {
            var startPosIndicator = robots[i].transform.parent.GetChild(0);
            startPosIndicator.position = robots[i].transform.position;
        }
    }

    public void RestartMission()
    {
        movementCount = 0;
        moveCountText.text = movementCount.ToString();

        var robots = GameObject.FindGameObjectsWithTag("Robot");
        for (int i = 0;i<robots.Length;i++)
        {
            var startPos = robots[i].transform.parent.GetChild(0).position;
            robots[i].transform.position = startPos;
        }
    }

    public void IncrementMoveCount()
    {
        movementCount++;
        moveCountText.text = movementCount.ToString();
    }
}
