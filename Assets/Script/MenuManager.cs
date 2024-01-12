using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[System.Serializable] public class EventGameState : UnityEvent<GameState, GameState> { }
public enum GameState
{
    Running,
    Pause
}

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject menu;

    public EventGameState gameStateHandler;
    public GameState currentGameState = GameState.Running;

    private void Update()
    {
         if(Input.GetKeyDown(KeyCode.Escape))
        {
            menu.SetActive(!menu.activeInHierarchy);
            UpdateGameState(currentGameState == GameState.Running ? GameState.Pause : GameState.Running);
        }
    }

    void UpdateGameState(GameState newGameState)
    {
        var previousGameState = currentGameState;
        currentGameState = newGameState;
        switch (currentGameState)
        {
            case GameState.Running:
                Time.timeScale = 1;
                break;
            case GameState.Pause:
                Time.timeScale = 0;
                break;
            default:
                break;
        }

        gameStateHandler.Invoke(currentGameState, previousGameState);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
