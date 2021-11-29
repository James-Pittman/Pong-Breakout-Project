using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// This class is responsible for coordinating all vital gameplay classes
// while the game is running.
public class GameCoordinator : MonoBehaviour
{
    // Singleton
    public static GameCoordinator instance;

    // All Prefabs
    public GameObject ballPrefab;
    public GameObject blockSetPrefab;
    public GameObject player1;
    public GameObject player2;
    private GameObject blockSet;

    // References to other objects/scripts.
    public List<GameObject> activeBalls = new List<GameObject>();
    public List<GameObject> activeBallsP1 = new List<GameObject>();
    public List<GameObject> activeBallsP2 = new List<GameObject>();
    public List<GameObject> activeBlocks = new List<GameObject>();
    public List<GameObject> inactiveBlocks = new List<GameObject>();

    // Array of Players and score areas.
    private ScoreKeeper[] scoreKeepers;

    // Flag for determining if this device is acting as the server.
    public bool serverFlag;

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

    //
    // METHODS FOR THE SERVER DEVICE
    //

    // Does all required actions for the game to properly start
    public void OnGameStart()
    {
        // Set difficulty to 0 (beginner) for now.
        difficulty = ImportantData.powerupFreq;
        if (difficulty == 0)
        {
            ballThrust = 100;
            powerUpRandomVal = 0.1f;
        }
        else if (difficulty == 1)
        {
            ballThrust = 600;
            powerUpRandomVal = 0.125f;
        }

        // The following code block sets the player names for both devices.
        if (serverFlag)
        {
            SendNames();
        }

        // Generate blocks and balls.
        GenerateBlocks();
        GenerateBall(0);
        GenerateBall(1);
        gameActive = true;
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

        NetworkCoordinator.instance.Disconnect();
        SceneManager.LoadScene("Win");
    }

    // Generate a new ball. If ownerID = 0, the ball is generated in front of player 1.
    // If ownerID = 1, it is generated in front of player 2.
    public void GenerateBall(int ownerID)
    {
        // Instantiate the new ball.
        GameObject newBall = Instantiate(ballPrefab, new Vector2(4, 0), transform.rotation);

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
        newBallStats.ballID = activeBalls.IndexOf(newBall);

        // If device isn't the server, stop here. The position of the ball will be updated
        // by the server.
        if (!serverFlag)
            return;

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
            movesLeft = (UnityEngine.Random.value < 0.5f) ? true : false;
        }
        newBall.transform.position = new Vector2(ballSpawnX, 0f);

        float horizontalMovement = movesLeft ? -1 : 1;
        float verticalComponent = UnityEngine.Random.Range(-1f, 1f);

        Vector2 direction = new Vector2(horizontalMovement, verticalComponent);
        newBallStats.thrust = ballThrust;

        newBallStats.ApplyForce(direction);
    }

    // Generates the set of blocks at the start of the game.
    public void GenerateBlocks()
    {
        blockSet = Instantiate(blockSetPrefab);
        int i = 0;
        foreach (Transform block in blockSet.transform)
        {
            activeBlocks.Add(block.gameObject);

            block.GetComponent<BlockController>().ActivateBlock(i);

            i++;
        }
    }

    // Respawns a random block from the block set.
    public void RespawnBlocks()
    {
        // Select a random block from the list of inactive blocks.
        int randomIndex = UnityEngine.Random.Range(0, inactiveBlocks.Count - 1);

        // Activate the selected block, and update lists accordingly.
        inactiveBlocks[randomIndex].GetComponent<BlockController>().ActivateBlock(-1);
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
        if (!serverFlag)
            return;

        BallController ballStats = ball.GetComponent<BallController>();
        int randomVal = UnityEngine.Random.Range(1, 4);

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

    private void SendNames()
    {
        playerNames = new string[] { ImportantData.player1Name, ImportantData.player2Name };
        byte[] player1 = Encoding.ASCII.GetBytes(playerNames[0]);
        byte[] player2 = Encoding.ASCII.GetBytes(playerNames[1]);

        byte[] player1Message = new byte[2 + (player1.Length)];
        byte[] player2Message = new byte[2 + (player2.Length)];

        player1Message[0] = (byte)0;
        player1Message[1] = (byte)1;
        for (int i = 2; i < player1.Length; i++)
        {
            player1Message[i] = player1[i - 2];
        }

        player2Message[0] = (byte)0;
        player2Message[1] = (byte)2;
        for (int i = 2; i < player2.Length; i++)
        {
            player2Message[i] = player2[i - 2];
        }

        NetworkCoordinator.instance.WriteMessage(player1Message);
        NetworkCoordinator.instance.WriteMessage(player2Message);
    }

    //
    // METHODS FOR THE CLIENT DEVICE
    //
    public void UpdateStartData(byte[] message)
    {
        byte[] name = new byte[message.Length - 2];
        Array.Copy(message, 2, name, 0, message.Length - 2);
        switch ((int)message[1])
        {
            case 1:
                playerNames[0] = Encoding.ASCII.GetString(name);
                break;
            case 2:
                playerNames[1] = Encoding.ASCII.GetString(name);
                break;
            default:
                return;
        }
    }

    public void UpdateBlockData(byte[] message)
    {
        int blockID = (int)message[1];
        bool flag = Convert.ToBoolean(message[2]);
        int health = (int)message[3];
        activeBlocks[blockID].GetComponent<BlockController>().UpdateBlock(flag, health);
    }

    public void UpdateBallData(byte[] message)
    {
        int ballID = (int)message[1];
        float xPos;
        float yPos;

        byte[] posX = new byte[4];
        byte[] posY = new byte[4];
        posX[0] = message[2];
        posX[1] = message[3];
        posX[2] = message[4];
        posX[3] = message[5];
        posY[0] = message[6];
        posY[1] = message[7];
        posY[2] = message[8];
        posY[3] = message[9];
        xPos = BitConverter.ToSingle(posX, 0);
        yPos = BitConverter.ToSingle(posY, 0);

        activeBalls[ballID].GetComponent<BallController>().UpdateBall(xPos, yPos);
    }

    public void UpdatePaddleData(byte[] message)
    {
        float yPos;

        byte[] posY = new byte[4];
        posY[0] = message[2];
        posY[1] = message[3];
        posY[2] = message[4];
        posY[3] = message[5];
        yPos = BitConverter.ToSingle(posY, 0);

        switch ((int)message[1])
        {
            case 0:
                player1.GetComponent<PaddleController>().UpdatePosition(yPos);
                break;
            case 1:
                player2.GetComponent<PaddleController>().UpdatePosition(yPos);
                break;
            default:
                return;
        }
    }


    // Initialize all object references as needed.
    private void Start()
    {
        if (instance == null)
            instance = this;

        scoreKeepers = FindObjectsOfType<ScoreKeeper>();

        serverFlag = ImportantData.serverFlag;

        OnGameStart();
    }

    // Update is called once per frame
    private void Update()
    {
        if (gameActive)
        {
            foreach (GameObject ball in activeBalls)
                ball.GetComponent<BallController>().ballID = activeBalls.IndexOf(ball);

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
