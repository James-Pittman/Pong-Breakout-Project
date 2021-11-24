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
        livesText.text = lives.ToString();

        score = 0;

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<BallController>() != null)
        {
            if (!debugToggle)
                lives--;
            livesText.text = lives.ToString();

            int ownerID = col.gameObject.GetComponent<BallController>().ownerID;
            
            coordinator.activeBalls.Remove(col.gameObject);
            if (ownerID == 0)
            {
                coordinator.activeBallsP1.Remove(col.gameObject);
            }
            else if (ownerID == 1)
            {
                coordinator.activeBallsP2.Remove(col.gameObject);
            }
            
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


            coordinator.GameOver();
            coordinator.gameActive = false;
            col.gameObject.SetActive(false);
            GameOverScreen.SetActive(true);
        }
    }

    // This function adds points to the player's score.
    // If adding the points causes integer overflow, the game
    // should terminate.
    private void AddScore(int points)
    {
        if (score + points < 0)
        {
            // Stop game somehow
        }

        score += points;
    }

    // This method adds points whenever the player's ball hits a block.
    public void AddBlockPoints()
    {
        AddScore(100);
    }

    // This method adds points whenever the player gains a power-up.
    public void AddPowerUpPoints()
    {
        AddScore(500);
    }

    public int getOwnerID()
    {
        return ownerID;
    }

    public void StartGame()
    {
        lives = 5;
        livesText.text = lives.ToString();
    }
}
