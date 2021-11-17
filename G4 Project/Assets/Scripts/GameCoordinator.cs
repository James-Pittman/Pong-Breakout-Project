using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// This class is responsible for coordinating all vital gameplay classes
// while the game is running.
public class GameCoordinator : MonoBehaviour
{
    // List of all active balls in the scene.
    private List<BallController> activeBalls = new List<BallController>();

    // Array of Players
    private PaddleController[] players = new PaddleController[2];

    // Difficulty
    public int difficulty;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Does all required actions for the game to properly start
    void OnGameStart()
    {

    }

    // Restarts the game
    public void RestartGame()
    {
        // Need to redo this to make it better.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
