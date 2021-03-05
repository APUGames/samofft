using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    [SerializeField] int playerScore = 0;

    [SerializeField] Text lives;
    [SerializeField] Text score;

    private void Awake()
    {
        // Will find the number of occurrences of theis Game Object
        int numGameSessions = FindObjectsOfType<GameSession>().Length;

        if (numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }

    }

    void Start()
    {
        lives.text = playerLives.ToString();
        score.text = playerScore.ToString();
    }

    public void ProcessPlayerDeath()
    {
        if(playerLives > 1)
        {
            SubtractLife();
        }
        else
        {
            ResetGameSession();
        }
    }

    public void ProcessPlayerScore(int points)
    {
        playerScore += points;
        score.text = playerScore.ToString();
    }

    private void ResetGameSession()
    {
        SceneManager.LoadScene(0);

        Destroy(gameObject);
    }

    private void SubtractLife()
    {
        playerLives--;

        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);

        lives.text = playerLives.ToString();
    }

    public void AddLife()
    {
        int tmp = playerScore % 700;
        if (tmp == 0 || tmp == 700)
        {
            playerLives++;
            lives.text = playerLives.ToString();
        }
    }
}
