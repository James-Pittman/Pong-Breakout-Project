using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// This class is responsible for coordinating all vital gameplay classes
// while the game is running.
public class GameCoordinator : MonoBehaviour
{
    // Array of Players
    private PaddleController[] players;

    // All Prefabs
    public GameObject ballPrefab;

    // References to other objects/scripts.
    private ScoreKeeper[] scoreKeepers;
    public List<GameObject> activeBalls = new List<GameObject>();

    // Difficulty
    public int difficulty;

    // Ball Thrust Value
    public int ballThrust;

    // Bool to track if the game is currently running
    public bool gameActive = false;

    // Initialize all object references as needed.
    void Start()
    {
        players = FindObjectsOfType<PaddleController>();
        scoreKeepers = FindObjectsOfType<ScoreKeeper>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameActive)
        {
            if (activeBalls.Count == 0)
            {
                GenerateBall(2);
            }
        }
    }

    // Does all required actions for the game to properly start
    public void OnGameStart()
    {
        GenerateBall(2);
        gameActive = true;
    }

    // Restarts the game
    public void RestartGame()
    {
        // Need to redo this to make it better.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        /* WIP
        foreach (ScoreKeeper sk in scoreKeepers)
        {
            sk.StartGame();
        }
        */
    }

    // Game Over
    public void GameOver()
    {
        gameActive = false;
        foreach (GameObject ball in activeBalls)
        {
            Destroy(ball);
        }
        activeBalls.Clear();
    }

    public void GenerateBall(int ownerID)
    {
        GameObject newBall = Instantiate(ballPrefab);
        activeBalls.Add(newBall);
        BallController newBallStats = newBall.GetComponent<BallController>();

        newBallStats.ownerID = ownerID;
        newBallStats.thrust = ballThrust;

        // Randomly choose which side to send the ball to
        float randomAngle;
        if (Random.value < 0.5f)
        {
            // Random angle between 2/3*pi and 4/3*pi
            randomAngle = Random.Range(2.094f, 4.189f);
        }
        else
        {
            // Random angle between 5/3*pi and 7/3*pi
            randomAngle = Random.Range(5.236f, 7.329f);
        }

        newBallStats.xForce = Mathf.Cos(randomAngle);
        newBallStats.yForce = Mathf.Sin(randomAngle);

        newBallStats.ApplyForce();
    }
}
