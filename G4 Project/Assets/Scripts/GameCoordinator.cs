using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// This class is responsible for coordinating all vital gameplay classes
// while the game is running.
public class GameCoordinator : MonoBehaviour
{
    // All Prefabs
    public GameObject ballPrefab;
    public GameObject blockSetPrefab;
    private GameObject blockSet;

    // References to other objects/scripts.
    public List<GameObject> activeBalls = new List<GameObject>();
    public List<GameObject> activeBallsP1 = new List<GameObject>();
    public List<GameObject> activeBallsP2 = new List<GameObject>();
    public List<GameObject> activeBlocks = new List<GameObject>();
    public List<GameObject> inactiveBlocks = new List<GameObject>();

    // Array of Players and score areas.
    private PaddleController[] players;
    private ScoreKeeper[] scoreKeepers;

    // Difficulty (0: beginner; 1: advanced)
    public int difficulty;

    // Ball Thrust Value
    public int ballThrust;

    // Determines what number of block must remain before blocks begin being respawned.
    public int respawnThreshold = 30;

    // Power-Up Frequency
    public float powerUpRandomVal;

    // Player names
    public string[] playerNames = new string[] {"Player 1", "Player 2"};

    // Bool to track if the game is currently running
    [HideInInspector]
    public bool gameActive = false;

    [SerializeField] private Text winner;

    // Does all required actions for the game to properly start
    public void OnGameStart()
    {
        // Set difficulty to 0 (beginner) for now.
        difficulty = ImportantData.powerupFreq;
        if (difficulty == 0)
        {
            ballThrust = 500;
            powerUpRandomVal = 0.1f;
        }
        else if (difficulty == 1)
        {
            ballThrust = 600;
            powerUpRandomVal = 0.125f;
        }

        playerNames = new string [] {ImportantData.player1Name, ImportantData.player2Name};

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

    // Game Over (the ownerID is for the player who lost)
    public void GameOver(int ownerID)
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

        Leaderboard leaderboard = Leaderboard.LoadRecords();
        int winnerScore = GetScoreKeeper(1 - ownerID).GetScore();
        string winnerName = playerNames[1 - ownerID];
        ImportantData.winnerName = winnerName;

        if (leaderboard.IsTopRecord(winnerScore))
        {
            Record newRecord = new Record(winnerScore, winnerName);
            leaderboard.AddRecord(newRecord);
            leaderboard.SaveRecords();
        }

        SceneManager.LoadScene("Win");
    }

    // Generate a new ball. If ownerID = 0, the ball is generated in front of player 1.
    // If ownerID = 1, it is generated in front of player 2.
    public void GenerateBall(int ownerID)
    {
        // Instantiate the new ball.
        GameObject newBall = Instantiate(ballPrefab);

        // Update ball lists accordingly.
        activeBalls.Add(newBall);
        if (ownerID == 0)
        {
            activeBallsP1.Add(newBall);
        }
        else if (ownerID == 1)
        {
            activeBallsP2.Add(newBall);
        }

        // Assign ownerID and thrust to the new ball.
        BallController newBallStats = newBall.GetComponent<BallController>();
        newBallStats.ownerID = ownerID;
        newBallStats.thrust = ballThrust;
        newBallStats.originID = ownerID;

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

        float horizontalMovement = movesLeft ? -1 : 1;
        float verticalComponent = Random.Range(-1f, 1f);

        Vector2 direction = new Vector2(horizontalMovement, verticalComponent);
        newBallStats.thrust = ballThrust;

        newBallStats.ApplyForce(direction);
    }

    // Generates the set of blocks at the start of the game.
    public void GenerateBlocks()
    {
        blockSet = Instantiate(blockSetPrefab);
        foreach (Transform block in blockSet.transform)
        {
            activeBlocks.Add(block.gameObject);
            block.GetComponent<BlockController>().ActivateBlock();
        }
    }

    // Respawns a random block from the block set.
    public void RespawnBlocks()
    {
        // Select a random block from the list of inactive blocks.
        int randomIndex = Random.Range(0, inactiveBlocks.Count - 1);

        // Activate the selected block, and update lists accordingly.
        inactiveBlocks[randomIndex].GetComponent<BlockController>().ActivateBlock();
        activeBlocks.Add(inactiveBlocks[randomIndex]);
        inactiveBlocks.RemoveAt(randomIndex);
    }

    public ScoreKeeper GetScoreKeeper(int ownerID)
    {
        if (ownerID == 0 || ownerID == 1)
        {
            return scoreKeepers[ownerID];
        }
        else
        {
            return null;
        }
    }

    public void SelectPowerUp(GameObject ball)
    {
        BallController ballStats = ball.GetComponent<BallController>();
        int randomVal = Random.Range(1, 4);

        // Switch statement to control which power up is selected.
        switch (randomVal)
        {
            case 1:
                ballStats.UpdateThrust(2 * ballThrust);
                break;
            case 2:
                GenerateBall(ballStats.ownerID);
                break;
            default:
                ballStats.AddSuperCharges(5);
                break;
        }
    }

    // Initialize all object references as needed.
    private void Start()
    {
        players = FindObjectsOfType<PaddleController>();
        scoreKeepers = FindObjectsOfType<ScoreKeeper>();
    }

    // Update is called once per frame
    private void Update()
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
}
