using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreKeeper : MonoBehaviour
{
    [SerializeField]
    private GameObject GameOverScreen;

    [SerializeField]
    private Text livesText;

    private int lives;
    
    // Start is called before the first frame update
    void Start()
    {
        lives = 5;
        livesText.text = lives.ToString();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.GetComponent<BallController>() != null)
        {
            lives--;
            livesText.text = lives.ToString();
            col.gameObject.GetComponent<BallController>().ResetBall();
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

            col.gameObject.SetActive(false);
            GameOverScreen.SetActive(true);
        }
    }
}
