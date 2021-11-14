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

            if (lives == 0)
            {
                col.gameObject.SetActive(false);
                GameOverScreen.SetActive(true);
            }
        }
    }
}
