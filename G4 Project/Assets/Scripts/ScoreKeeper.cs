using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreKeeper : MonoBehaviour
{
    [SerializeField] private GameObject GameOverScreen;
    [SerializeField] private Text scoreText;
    private int score;
    
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D col){
        if (col.gameObject.GetComponent<BallController>() != null){
            score++;
            scoreText.text = score.ToString();
            col.gameObject.GetComponent<BallController>().ResetBall();
        }

        if (score >= 7){
            col.gameObject.SetActive(false);
            GameOverScreen.SetActive(true);
        }
    }
}
