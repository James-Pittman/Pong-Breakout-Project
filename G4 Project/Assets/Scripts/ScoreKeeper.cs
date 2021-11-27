using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// THIS SCRIPT NEEDS TO BE MERGED INTO GAME COORDINATOR.
// CANNOT HAVE TWO OF THEM.
public class ScoreKeeper : MonoBehaviour
{
    [SerializeField]
    private GameObject GameOverScreen;

    private GameCoordinator coordinator;

    [SerializeField]
    private int ownerID;

    [SerializeField]
    private Text livesText;
    private int lives;

    [SerializeField]
    private Text scoreText;
    private int score = 0;

    public bool debugToggle = false;
    
    // Start is called before the first frame update
    void Start()
    {
        coordinator = FindObjectOfType<GameCoordinator>();

        lives = 5;
        livesText.text = "Lives: " + lives.ToString();

        score = 0;

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<BallController>() != null)
        {
            if (!debugToggle)
                lives--;
            livesText.text = "Lives: " + lives.ToString();

            int ownerID = col.gameObject.GetComponent<BallController>().originID;
            
            coordinator.activeBalls.Remove(col.gameObject);
            if (ownerID == 0)
            {
                coordinator.activeBallsP1.Remove(col.gameObject);
            }
            else if (ownerID == 1)
            {
                coordinator.activeBallsP2.Remove(col.gameObject);
            }

            // Add points for losing a life to the other player's score.
            AddLostLifePoints();

            Destroy(col.gameObject);
        }

        if (lives == 0)
        {
            // Below is the code I used to test the leaderboard.
            // It can be removed at any time, but I am leaving it here for
            // now in case it is useful in the future.

            // Leaderboard leaderboard = Leaderboard.LoadRecords();

            // List<Record> recordList = leaderboard.getTopRecords();
            // for (int i = 0; i < recordList.Count; i++)
            // {
            //     Debug.Log(recordList[i].score);
            // }

            // if (leaderboard.isTopRecord(1))
            // {
            //     Record newRecord = new Record(1, "testName");
            //     leaderboard.addRecord(newRecord);
            //     leaderboard.SaveRecords();
            // }


            EndGame();
        }
    }


    // Add points for breaking multiple blocks (or for breaking multiple health
    // levels of a single block).
    public void AddBlockPoints(int numBlocks)
    {
        AddScore(numBlocks * 100);
    }

    // This method adds points whenever the player's ball hits a block.
    public void AddBlockPoints()
    {
        AddBlockPoints(1);
    }
    
    // This method adds points whenever the player gains a power-up.
    public void AddPowerUpPoints()
    {
        AddScore(500);
    }

    // This method gives points to the OTHER player's score when the
    // current player loses a life.
    public void AddLostLifePoints()
    {
        ScoreKeeper keeper = coordinator.GetScoreKeeper(1 - ownerID);
        keeper.AddScore(1000);
    }

    public int GetOwnerID()
    {
        return ownerID;
    }

    public void StartGame()
    {
        lives = 5;
        livesText.text = "Lives: " + lives.ToString();
    }

    public int GetScore()
    {
        return score;
    }

    // This function adds points to the player's score.
    // If adding the points causes integer overflow, the game
    // should terminate.
    private void AddScore(int points)
    {
        // Check for overflow
        if (score + points < 0)
        {
            EndGame();
        }
        else
        {
            score += points;
            scoreText.text = "Score: " + score.ToString();
        }
    }

    private void EndGame()
    {
        coordinator.GameOver(ownerID);
        coordinator.gameActive = false;
        //col.gameObject.SetActive(false);
        //GameOverScreen.SetActive(true);
    }
}
