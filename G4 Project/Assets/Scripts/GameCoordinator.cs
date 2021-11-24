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
    public GameObject blockSetPrefab;

    private GameObject blockSet;

    // References to other objects/scripts.
    private ScoreKeeper[] scoreKeepers;
    public List<GameObject> activeBalls = new List<GameObject>();
    public List<GameObject> activeBallsP1 = new List<GameObject>();
    public List<GameObject> activeBallsP2 = new List<GameObject>();
    public List<GameObject> activeBlocks = new List<GameObject>();
    public List<GameObject> inactiveBlocks = new List<GameObject>();

    // Difficulty
    public int difficulty;

    // Ball Thrust Value
    public int ballThrust;

    // Determines what number of block must remain before blocks begin being respawned.
    public int respawnThreshold = 30;

    // Power-Up Frequency
    public float powerUpRandomVal = 0.1f;

    // Bool to track if the game is currently running
    [HideInInspector]
    public bool gameActive = false;

    // Number of blocks remaining on the screen
    public int blocksRemaining;

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
            if (activeBallsP1.Count == 0)
            {
                GenerateBall(0);
            }
            if (activeBallsP2.Count == 0)
            {
                GenerateBall(1);
            }
            if (activeBlocks.Count < 30)
            {
                RespawnBlocks();
            }
        }
    }

    // Does all required actions for the game to properly start
    public void OnGameStart()
    {
        GenerateBlocks();
        GenerateBall(0);
        GenerateBall(1);
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

        // Clear all balls from the scene.
        foreach (GameObject ball in activeBalls)
        {
            Destroy(ball);
        }
        activeBalls.Clear();
        activeBallsP1.Clear();
        activeBallsP2.Clear();
    }

    // Generate a new ball. If ownerID = 0, the ball is generated in front of player 1.
    // If ownerID = 1, it is generated in front of player 2.
    public void GenerateBall(int ownerID)
    {
        GameObject newBall = Instantiate(ballPrefab);

        activeBalls.Add(newBall);
        if (ownerID == 0)
        {
            activeBallsP1.Add(newBall);
        }
        else if (ownerID == 1)
        {
            activeBallsP2.Add(newBall);
        }

        BallController newBallStats = newBall.GetComponent<BallController>();

        newBallStats.ownerID = ownerID;
        newBallStats.thrust = ballThrust;

        // Determine the x-coordinate where the ball is spawned (ballSpawnX)
        // and which direction the ball will move.
        int ballSpawnX;
        bool movesLeft;
        if (ownerID == 0)
        {
            ballSpawnX = -6;
            movesLeft = false;
        }
        else if (ownerID == 1)
        {
            ballSpawnX = 6;
            movesLeft = true;
        }
        else
        {
            ballSpawnX = 0;
            movesLeft = (Random.value < 0.5f) ? true : false;
        }

        newBall.transform.position = new Vector2(ballSpawnX, 0);

        // Determine angle that the ball will move.
        float randomAngle;
        if (movesLeft)
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


    public void GenerateBlocks()
    {
        blockSet = Instantiate(blockSetPrefab);
        foreach (Transform block in blockSet.transform)
        {
            activeBlocks.Add(block.gameObject);
            block.GetComponent<BlockController>().ActivateBlock();
        }
    }

    public void RespawnBlocks()
    {
        // Select a random block from the list of inactive blocks.
        int randomIndex = Random.Range(0, inactiveBlocks.Count - 1);

        // Activate the selected block, and update lists accordingly.
        inactiveBlocks[randomIndex].GetComponent<BlockController>().ActivateBlock();
        activeBlocks.Add(inactiveBlocks[randomIndex]);
        inactiveBlocks.RemoveAt(randomIndex);
    }
}
