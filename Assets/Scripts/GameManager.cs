using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum gameState
    {
        Menu,
        Playing,
        Pause
    }

    public gameState actualState;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void Pause() 
    {
        if (actualState == gameState.Playing)
        {
            actualState = gameState.Pause;
            Time.timeScale = 0f;
            gameState.
        }
    }
}
